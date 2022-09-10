using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpartaRemixStudio2022
{
    public partial class VideoPlayer : UserControl
    {
        public VideoPlayer()
        {
            InitializeComponent();
        }

        bool playing = false;
        Stopwatch sw = new Stopwatch();
        float swOffset = 0;
        float positionVideo;
        float lastPickedPosition;

        bool video = false;
        bool audio = false;
        string source = null;
        CMDVideoReader cvr = new CMDVideoReader();
        Bitmap b = null;

        void SetStatus(string status)
        {
            if (status == null)
            {
                LabelStatus.Visible = false;
            }
            else
            {
                LabelStatus.Visible = true;
                LabelStatus.Text = status;
            }
        }

        public new void Dispose()
        {
            base.Dispose();
            cvr.CloseFile();
            b?.Dispose();
        }

        public void OpenSource(string file)
        {
            CloseSource();

            if (VideoInfo.IsVideoFile(file))
            {
                video = true;
                string size = VideoInfo.GetSize(file);
                string[] size2 = size.Split('x');
                int w = int.Parse(size2[0]);
                int h = int.Parse(size2[1]);
                b = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            if (VideoInfo.IsAudioFile(file))
            {
                audio = true;
            }

            if (video)
            {
                SetStatus(null);
            }
            else if (audio)
            {
                SetStatus("Audio-only file open.");
            }
            else
            {
                SetStatus("No video file open.");
            }

            source = file;
        }
        public void CloseSource()
        {
            SetStatus("No video file open.");
            video = false;
            audio = false;
            cvr?.CloseFile();
            source = null;
            VideoPictureBox.Image = null;
        }
        public void Play()
        {
            if (!playing)
            {
                Seek(positionVideo);
                sw.Restart();
                playing = true;
            }
        }
        public void Pause()
        {
            playing = false;
            sw.Stop();
            // Stop ap
        }
        public void Stop()
        {
            playing = false;
            sw.Stop();
            positionVideo = lastPickedPosition;
            // Stop ap
        }
        public void Seek(float seconds)
        {
            if (seconds < 0) seconds = 0;
            if (video)
            {
                cvr.OpenFile(source, (int)(seconds * 1000));
                positionVideo = seconds;
                swOffset = seconds;
                if (playing) sw.Restart();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (playing)
            {
                positionVideo = swOffset + (float)sw.Elapsed.TotalSeconds;
                if (video)
                {
                    bool readSomething = cvr.ReadTimeAbsolute((float)sw.Elapsed.TotalSeconds);
                    if (readSomething)
                    {
                        BitmapData bd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                        Marshal.Copy(cvr.Buffer, 0, bd.Scan0, b.Width * b.Height * 3);
                        b.UnlockBits(bd);
                        VideoPictureBox.Image = b;
                    }
                }
            }
        }

        private void ButtonPlay_Click(object sender, EventArgs e) => Play();
        private void ButtonPause_Click(object sender, EventArgs e) => Pause();
        private void ButtonStop_Click(object sender, EventArgs e) => Stop();
        private void ButtonForward_Click(object sender, EventArgs e) => Seek(positionVideo + 10);
        private void ButtonBack_Click(object sender, EventArgs e) => Seek(positionVideo - 10);
    }
}
