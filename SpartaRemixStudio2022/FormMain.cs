using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SpartaRemixStudio2022
{
    public partial class FormMain : Form
    {
        Project p = new Project();
        string lastSaveFilename;

        CMDVideoReader cvr;
        public FormMain()
        {
            InitializeComponent();

            // TODO: Not public
            mediaLibraryControl1.p = p;

            // DEBUG
            p.timeline.Tracks.Add(new Track());
            p.timeline.Tracks[0].SetType(new RegularTrackFactory().CreateNewInstance(p.timeline.Tracks[0]));
            TimelineControl<Track> tlc = new TimelineControl<Track>(p.timeline);

            tlc.Mlc = mediaLibraryControl1;
            tlc.Parent = xTimeline;
            tlc.Dock = DockStyle.Fill;
        }
        public FormMain(string pathToProject)
        {
            InitializeComponent();

            lastSaveFilename = pathToProject;
            button3.Enabled = true;
        }
        void LoadProject(string filename)
        {
            FileStream fs = new FileStream(lastSaveFilename, FileMode.Open);
            p = UniLoad.CreateObject<Project>(fs);
            fs.Close();
            fs.Dispose();
            p.DoPostLoadActions();
            mediaLibraryControl1.p = p;
            TimelineControl<Track> tlc = new TimelineControl<Track>(p.timeline);
            tlc.Mlc = mediaLibraryControl1;
            tlc.Parent = xTimeline;
            tlc.Dock = DockStyle.Fill;
        }

        // DEBUG
        ITrackVideoReader itvr = null;
        long timeIn = 0;
        private int tex;
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            /*GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(BeginMode.Triangles);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, -1);
            GL.TexCoord2(1, 1);
            GL.Vertex2(1, -1);
            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 1);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, -1);
            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 1);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, 1);

            GL.End();

            glControl1.SwapBuffers();*/


            TextureInfo ti = TextureInfo.None;
            // TODO: Post load
            if (itvr != null)
            {
                ti = itvr.Read(timeIn);
                timeIn += timer1.Interval * 48;
                if (tisp.CurrentPosition > timeIn) timeIn = tisp.CurrentPosition;
            }

            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            if (!ti.IsNone)
            {
                GL.BindTexture(TextureTarget.Texture2D, ti.TextureIndex);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0, 0);
                GL.Vertex2(-1, -1);
                GL.TexCoord2(0, 1);
                GL.Vertex2(-1, 1);
                GL.TexCoord2(1, 1);
                GL.Vertex2(1, 1);
                GL.TexCoord2(1, 0);
                GL.Vertex2(1, -1);
                GL.End();
            }
            glControl1.SwapBuffers();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            sw.Start();

            FfmpegInfo.LookForFFMPEG();

            tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tex);

            if (lastSaveFilename != null) LoadProject(lastSaveFilename);
        }

        Stopwatch sw = new Stopwatch();
        private void timer1_Tick(object sender, EventArgs e)
        {
            //TODO

            /*GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, cvr.Width, cvr.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, cvr.Buffer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);*/

            
            glControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormSourceViewer fsv = new FormSourceViewer(p);
            fsv.Show();
        }

        // DEBUG: Play / Stop
        WaveOut wo = null;
        TrackReaderISP tisp = null;
        private void button2_Click(object sender, EventArgs e)
        {
            if (wo == null)
            {
                // Play
                wo = new WaveOut(Handle);
                tisp = new TrackReaderISP(p.timeline.Tracks[0], 0);
                wo.Init(tisp, true);
                wo.DesiredLatency = 100;
                wo.Play();

                itvr = p.timeline.Tracks[0].ExtType.GetVideo(0);

                timeIn = 0;
            }
            else
            {
                // Stop
                wo.Stop();
                wo.Dispose();
                wo = null;
                itvr = null;
                tisp = null;
                timeIn = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UniLoad.Save(lastSaveFilename, p);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            UniLoad.Save(saveFileDialog1.FileName, p);
            lastSaveFilename = saveFileDialog1.FileName;
            button3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormPatternEdit fpe = new FormPatternEdit(p);
            fpe.ShowDialog();
        }
    }
}
