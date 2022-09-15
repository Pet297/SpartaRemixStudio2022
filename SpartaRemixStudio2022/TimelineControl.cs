using OpenTK.Audio.OpenAL;
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
    public partial class TimelineControl : UserControl
    {
        public MediaLibraryControl Mlc
        {
            get { return mlc; }
            set
            {
                mlc = value;
                foreach (TrackControl trackControl in trackControls)
                {
                    trackControl.mlc = mlc;
                }
            }
        }
        MediaLibraryControl mlc;
        readonly List<TrackControl> trackControls = new List<TrackControl>();
        readonly Timeline timeline;
        public long StartTime = 0;
        public long SamplesPerPixel = 50;
        public int TrackHeight = 100;
        int scrollDelta = 0;

        // TODO: get set pro posuvnik
        public long MaxTime = 48000L * 600;

        public bool RoundingMode = true;
        public int MaxPixelsForRounding = 40;

        public TimelineControl(Timeline t)
        {
            InitializeComponent();
            timeline = t;

            int y = 0;

            foreach (Track track in timeline.Tracks)
            {
                TrackControl tc = new TrackControl(this, timeline, track)
                {
                    Width = PanelTracks.Width,
                    Height = TrackHeight,
                    Location = new Point(0, y),
                    Parent = PanelTracks
                };

                trackControls.Add(tc);

                y += TrackHeight;
            }
            PanelTracks.MouseWheel += (s, e) => PanelTracks_MouseWheel(s, e);

            // TODO: get set pro posuvnik
            HScrollTime.Maximum = (int)(MaxTime / SamplesPerPixel);
            HScrollTime.SmallChange = 10;
        }
        private void PanelTracks_MouseWheel(object sender, MouseEventArgs e)
        {
            scrollDelta += e.Delta;

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                while (scrollDelta <= -120)
                {
                    SamplesPerPixel = (int)(SamplesPerPixel * 1.1f);
                    scrollDelta += 120;
                }
                while (scrollDelta >= 120)
                {
                    SamplesPerPixel = (int)(SamplesPerPixel / 1.1f);
                    scrollDelta -= 120;
                }
                if (SamplesPerPixel < 20)
                {
                    SamplesPerPixel = 20;
                }
                Invalidate(true);
            }
            // horizontal
            else
            {
                //vertical
            }

            scrollDelta %= 120;
        }

        private void PanelTracks_Scroll(object sender, ScrollEventArgs e)
        {
        }
        private void TimelineControl_Paint(object sender, PaintEventArgs e)
        {
        }
        private void TimelineControl_MouseDown(object sender, MouseEventArgs e)
        {
        }
        private void TimelineControl_MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void TimelineControl_MouseUp(object sender, MouseEventArgs e)
        {
            Invalidate();
        }
        private void TimelineControl_Resize(object sender, EventArgs e)
        {

        }

        public long CursorToMediaPosition(long x)
        {
            return StartTime + x * SamplesPerPixel;
        }
        public long MediaToCursorPosition(long time)
        {
            return (time - StartTime) / SamplesPerPixel;
        }
        public long RoundPosition(long time)
        {
            if (!RoundingMode) return time;

            long maxDiff = SamplesPerPixel * MaxPixelsForRounding;
            long minDiff = maxDiff;
            long best = time;

            foreach (TimelineGridline tc in timeline.Gridlines)
            {
                if (SamplesPerPixel <= tc.MaxSamplesPerPixel)
                {
                    long diff = Math.Abs(tc.Position - time);
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        best = tc.Position;
                    }
                }
            }
            return best;
        }

        public void GenerateGridLines(float BPM, long startTime, int beats)
        {
            timeline.Gridlines.Clear();
            // 32 per beat
            for (int i = 0; i < beats; i++)
            {
                for (int j = 0; j < 96; j++)
                {
                    byte color = 25;

                    if (i % 2 == 0) color -= 5;

                    if (j % 24 >= 18) color -= 0;
                    else if (j % 24 >= 12) color -= 2;
                    else if (j % 24 >= 6) color -= 3;
                    else  color -= 5;

                    if (j % 6 < 3) color /= 2;

                    if (j % 3 == 2) color += 4;
                    if (j % 3 == 1) color += 2;

                    float offset = (i * 96 + j) * (48000L * 240) / (BPM * 96f) + startTime;

                    int visibility = 50; //PLACE HOLDER
                    if (j % 3 == 0) visibility *= 3;
                    if (j % 6 == 0) visibility *= 2;
                    if (j % 12 == 0) visibility *= 2;
                    if (j % 24 == 0) visibility *= 2;
                    if (j % 48 == 0) visibility *= 2;
                    if (j % 96 == 0)
                    {
                        visibility *= 2;
                        int more = i;
                        while (more % 2 == 0 && more != 0)
                        {
                            more /= 2;
                            visibility *= 2;
                        }
                        if (more == 0) visibility = int.MaxValue;
                    }

                    string name = "";
                    if (j % 24 == 0)  name = $"{i}.{j / 24 % 4}";
                    else if (j % 6 == 0) name = $"-";
                    timeline.Gridlines.Add(new TimelineGridline((long)offset, visibility, name, color, color, color));
                }
            }
        }

        private void HScrollTime_Scroll(object sender, ScrollEventArgs e)
        {
            StartTime = SamplesPerPixel * HScrollTime.Value;
            foreach(TrackControl tc in trackControls)
            {
                Invalidate(true);
            }
        }

        private void PanelTracks_SizeChanged(object sender, EventArgs e)
        {
            foreach (TrackControl tc in trackControls)
            {
                tc.Width = PanelTracks.Width;
            }
        }

        // RULER
        private void PanelRuler_Paint(object sender, PaintEventArgs e)
        {
            if (timeline != null)
            {
                List<TimelineGridline> visible = new List<TimelineGridline>(timeline.Gridlines);
                visible.RemoveAll((tg) => (tg.MaxSamplesPerPixel <= SamplesPerPixel * 2));

                for (int i = 0; i < visible.Count; i++)
                {
                    TimelineGridline tg = visible[i];

                    if (tg.Position - StartTime >= 0 &&
                        (tg.Position - StartTime) / SamplesPerPixel <= Width)
                    {
                        int px = (int)((tg.Position - StartTime) / SamplesPerPixel);
                        e.Graphics.DrawLine(Pens.Black, 189 + px, 0, 189 + px, PanelRuler.Height);
                        e.Graphics.DrawString(tg.Name, new Font("Arial", 9), Brushes.Black, 189 + px, 0);
                    }
                }
            }
        }
    }
}
