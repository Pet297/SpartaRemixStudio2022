using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace SpartaRemixStudio2022
{
    public partial class FormMain : Form
    {
        Project p = new Project();

        CMDVideoReader cvr;
        public FormMain()
        {
            InitializeComponent();

            // TODO: Not public
            mediaLibraryControl1.p = p;

            // DEBUG
            p.timeline.Tracks.Add(new Track());
            p.timeline.Tracks[0].AddMedia(new Media() { Position = 0, Length = 1000 });
            p.timeline.Tracks[0].AddMedia(new Media() { Position = 3000, Length = 1000 });
            p.timeline.Tracks[0].AddMedia(new Media() { Position = 5000, Length = 1000 });
            p.timeline.Tracks.Add(new Track());
            p.timeline.Tracks.Add(new Track());
            p.timeline.Tracks.Add(new Track());
            p.timeline.Tracks.Add(new Track());
            TimelineControl tlc = new TimelineControl(p.timeline);
            tlc.GenerateGridLines(140, 0, 100);
            tlc.Mlc = mediaLibraryControl1;

            tlc.Parent = xTimeline;
            tlc.Dock = DockStyle.Fill;
        }

        // DEBUG
        private int tex;
        private string src = @"D:\Media\LPS - ultra 666\lps-g5-s1-e14.mp4";
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
            cvr = new CMDVideoReader();
            cvr.OpenFile(src, 0, 320, 180);

            GL.BindTexture(TextureTarget.Texture2D, tex);

            //MessageBox.Show($"{VideoInfo.IsAudioFile(@"")}{VideoInfo.IsAudioFile(src)}{VideoInfo.IsAudioFile(@"D:\Media\a.wav")}{VideoInfo.IsAudioFile(@"D:\Media\a0.png")}{VideoInfo.IsAudioFile(@"D:\Media\Aduburyus (Almost) Complete Sparta Base Collection (Tribute #2) [360p].mp4")}|{VideoInfo.IsAudioFile(@"D:\Media\crazy_dancin.gif")}");
            //MessageBox.Show($"{VideoInfo.IsVideoFile(@"")}{VideoInfo.IsVideoFile(src)}{VideoInfo.IsVideoFile(@"D:\Media\a.wav")}{VideoInfo.IsVideoFile(@"D:\Media\a0.png")}{VideoInfo.IsVideoFile(@"D:\Media\Aduburyus (Almost) Complete Sparta Base Collection (Tribute #2) [360p].mp4")}|{VideoInfo.IsVideoFile(@"D:\Media\crazy_dancin.gif")}");
            //MessageBox.Show($"{VideoInfo.IsImageFile(@"")}{VideoInfo.IsImageFile(src)}{VideoInfo.IsImageFile(@"D:\Media\a.wav")}{VideoInfo.IsImageFile(@"D:\Media\a0.png")}{VideoInfo.IsImageFile(@"D:\Media\Aduburyus (Almost) Complete Sparta Base Collection (Tribute #2) [360p].mp4")}|{VideoInfo.IsImageFile(@"D:\Media\crazy_dancin.gif")}");
        }

        Stopwatch sw = new Stopwatch();
        private void timer1_Tick(object sender, EventArgs e)
        {
            //cvr.ReadTimeAbsolute((float)sw.Elapsed.TotalSeconds);
            GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, cvr.Width, cvr.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, cvr.Buffer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);

            glControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormSourceViewer fsv = new FormSourceViewer(p);
            fsv.Show();
        }
    }
}
