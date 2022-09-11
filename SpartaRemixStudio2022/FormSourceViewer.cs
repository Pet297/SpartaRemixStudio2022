using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpartaRemixStudio2022
{
    public partial class FormSourceViewer : Form
    {
        Project p;
        EventHandler handler;
        VideoSource current = null;
        CMDVideoReader cvr = null;

        float audioPosSec = 0;
        float waveResolutionSec = 8.0f;

        float redRegionStart = 0;
        float redRegionEnd = 0;

        public FormSourceViewer(Project p)
        {
            InitializeComponent();
            this.p = p;
            listBox1.DataSource = p.GetSources.ToList();
            handler = (s, e) => listBox1.DataSource = p.GetSources.ToList();
            p.SourceAdded += handler;
        }


        //pick source
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            current = listBox1.SelectedItem as VideoSource;
            axWindowsMediaPlayer1.URL =  current.File;
        }

        //add source to project
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        //new source
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // is video or audio
            if (VideoInfo.IsAudioFile(openFileDialog1.FileName) || VideoInfo.IsVideoFile(openFileDialog1.FileName)) p.AddSource(openFileDialog1.FileName);
        }

        private void FormSourceViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            p.SourceAdded -= handler;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox3.Invalidate();
            pictureBox4.Invalidate();
        }

        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            if (current?.PreviewWave != null)
            {
                audioPosSec = (float)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                for (int i = 0; i < pictureBox4.Width; i++)
                {
                    float t = MouseToAudiopos(i);
                    if (t >= redRegionStart && t < redRegionEnd) e.Graphics.DrawLine(Pens.DarkRed, i, 0, i, pictureBox4.Height);
                    float val = current.PreviewWave.GetPoint(t);
                    e.Graphics.DrawLine(Pens.Green, i, pictureBox4.Height / 2f * (1 - val), i, pictureBox4.Height / 2f * (1 + val));
                }
            }
        }

        bool mouseDown = false;
        float mouseDownOn = 0;

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownOn = MouseToAudiopos(e.X);
            redRegionStart = mouseDownOn;
            redRegionEnd = mouseDownOn;
            mouseDown = true;
            pictureBox4.Invalidate();
        }
        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                float time = MouseToAudiopos(e.X);

                if (time < mouseDownOn)
                {
                    redRegionStart = time;
                    redRegionEnd = mouseDownOn;
                }
                else
                {
                    redRegionStart = mouseDownOn;
                    redRegionEnd = time;
                }

                pictureBox4.Invalidate();
            }
        }
        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private float MouseToAudiopos(float x) => MathHelper.LinearMap(0, pictureBox4.Width, x, audioPosSec - waveResolutionSec / 2, audioPosSec + waveResolutionSec / 2);
        private float AudioposToMouse(float secs) => MathHelper.LinearMap(audioPosSec - waveResolutionSec / 2, audioPosSec + waveResolutionSec / 2, secs, 0, pictureBox4.Width);

        // Test cut
        private void button2_Click(object sender, EventArgs e)
        {
            float len = redRegionEnd - redRegionStart;
            int len2 = (int)(len * 96000);
            len2 += len2 % 2;

            //float[] audio = new float[len2];
            ISampleProvider isp = current.GetAudioReader(redRegionStart);

            if (isp != null)
            {
                isp = new NAudio.Wave.SampleProviders.OffsetSampleProvider(isp);
                (isp as NAudio.Wave.SampleProviders.OffsetSampleProvider).TakeSamples = len2;
                WasapiOut wo = new WasapiOut();
                wo.Init(isp);
                wo.PlaybackStopped += (s, ee) => wo.Dispose();
                wo.Play();
            }
        }
        private float[] GetCut()
        {
            float len = redRegionEnd - redRegionStart;
            int len2 = (int)(len * 96000);
            len2 += len2 % 2;
          
            ISampleProvider isp = current.GetAudioReader(redRegionStart);

            if (isp != null)
            {
                float[] audio = new float[len2];
                isp.Read(audio, 0, audio.Length);
                return audio;
            }

            return null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // TODO: not null, use definer
            AudioCutSample acs = new AudioCutSample(2, 48000, GetCut());
            VideoCutSample vcs = new VideoCutSample(current, MouseToAudiopos(redRegionStart) / 48000f, 15);
            p.AddSample(acs, vcs);
        }
    }
}
