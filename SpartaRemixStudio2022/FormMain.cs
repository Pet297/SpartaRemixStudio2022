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
            TimelineControl tlc = new TimelineControl(p.timeline);
            tlc.GenerateGridLines(140, 0, 100);

            tlc.Mlc = mediaLibraryControl1;
            tlc.Parent = xTimeline;
            tlc.Dock = DockStyle.Fill;
        }
        public FormMain(string pathToProject)
        {
            InitializeComponent();

            FileStream fs = new FileStream(pathToProject, FileMode.Open);
            p = UniLoad.CreateObject<Project>(fs);
            fs.Close();
            fs.Dispose();
            lastSaveFilename = pathToProject;
            button3.Enabled = true;

            // TODO: Not public
            mediaLibraryControl1.p = p;

            TimelineControl tlc = new TimelineControl(p.timeline);
            tlc.Mlc = mediaLibraryControl1;
            tlc.Parent = xTimeline;
            tlc.Dock = DockStyle.Fill;
        }

        // DEBUG
        private int tex;
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, tex);
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

            glControl1.SwapBuffers();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            sw.Start();

            FfmpegInfo.LookForFFMPEG();

            tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tex);
        }

        Stopwatch sw = new Stopwatch();
        private void timer1_Tick(object sender, EventArgs e)
        {
            //cvr.ReadTimeAbsolute((float)sw.Elapsed.TotalSeconds);
            //GL.BindTexture(TextureTarget.Texture2D, tex);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, cvr.Width, cvr.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, cvr.Buffer);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);

            glControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormSourceViewer fsv = new FormSourceViewer(p);
            fsv.Show();
        }

        // DEBUG: Play / Stop
        WaveOut wo = null;
        private void button2_Click(object sender, EventArgs e)
        {
            if (wo == null)
            {
                // Play
                wo = new WaveOut(Handle);
                wo.Init(new TrackReaderISP(p.timeline.Tracks[0], 0), true);
                wo.DesiredLatency = 100;
                wo.Play();
            }
            else
            {
                // Stop
                wo.Stop();
                wo.Dispose();
                wo = null;
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
    }
}
