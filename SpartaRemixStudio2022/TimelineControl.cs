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
                PanelTracks.Invalidate(true);
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
            for (int i = 0; i < beats * 16; i++)
            {
                float offset = i * (48000L * 240) / (BPM * 16f) + startTime;
                timeline.Gridlines.Add(new TimelineGridline((long)offset, 200, (i % 4 == 0 ? $"{i / 16} {i / 4 % 4}/4" : "")));
            }
        }

        private void HScrollTime_Scroll(object sender, ScrollEventArgs e)
        {
            StartTime = SamplesPerPixel * HScrollTime.Value;
            foreach(TrackControl tc in trackControls)
            {
                tc.Invalidate(true);
            }
        }
    }
}
