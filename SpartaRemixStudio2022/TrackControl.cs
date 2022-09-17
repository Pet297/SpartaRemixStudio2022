using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpenTK.Graphics.OpenGL.GL;

namespace SpartaRemixStudio2022
{
    public partial class TrackControl : UserControl
    {
        public MediaLibraryControl mlc;

        TimelineControl tlc;
        Timeline tl;
        Track t;

        long dropLocation = 0;
        bool draging = false;

        Media stretchedMedia = null;
        bool stretching = false;
        bool leftStretch = false;
        bool rightStretch = false;

        Media cursorMedia;
        Media cursorBorder;
        bool leftBorder;
        bool rightBorder;
        int innerTolerance = 10;
        int outerTolerance = 5;

        public void PlaceMediaClickAtPos(int mouseX)
        {
            Media newMedia = mlc.GetMedia();
            if (newMedia != null)
            {
                long pos = tlc.CursorToMediaPosition(mouseX);
                Tuple<long, long> pos2 = tlc.RoundDownAndNext(pos, 24000); // TODO: Default based on settings
                newMedia.Position = pos2.Item1;
                newMedia.Length = pos2.Item2 - pos2.Item1; 
                t.AddMedia(newMedia);
                pictureBoxMedia.Invalidate();
            }
        }
        private bool UpdateCursorOver(int mouseX)
        {
            cursorMedia = null;

            long best = long.MaxValue;
            Media bestm = null;
            bool left = false;

            foreach(Media media in t.GetMedia)
            {
                long px0 = mouseX - tlc.MediaToCursorPosition(media.Position);
                long px1 = mouseX - tlc.MediaToCursorPosition(media.Position + media.Length);

                if (-outerTolerance <= px0 && px0 <= innerTolerance)
                {
                    long diff = Math.Abs(px0);
                    if (diff < best)
                    {
                        best = diff;
                        bestm = media;
                        left = true;
                    }
                }
                if (-innerTolerance <= px1 && px1 <= outerTolerance)
                {
                    long diff = Math.Abs(px0);
                    if (diff < best)
                    {
                        best = diff;
                        bestm = media;
                        left = false;
                    }
                }
                if (px0 >= 0 && px1 <= 0)
                {
                    cursorMedia = media;
                }
            }

            bool ret = false;

            if (bestm == null)
            {
                if (cursorBorder != null) ret = true;
                cursorBorder = null;
                leftBorder = false;
                rightBorder = false;
            }
            else
            {
                if (cursorBorder != bestm) ret = true;
                if (leftBorder != left) ret = true;
                cursorBorder = bestm;
                leftBorder = left;
                rightBorder = !left;
            }

            return ret;
        }

        public TrackControl(TimelineControl tlc, Timeline timeline, Track track)
        {
            InitializeComponent();
            t = track;
            tl = timeline;
            this.tlc = tlc;
        }
        private void pictureBoxMedia_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, pictureBoxMedia.Width, pictureBoxMedia.Height); ;

            if (tl != null)
            {
                ReadOnlyCollection<TimelineGridline> visible = tlc.VisibleGridlines;

                if (visible.Count > 0)
                {
                    TimelineGridline tgl = visible[visible.Count - 1];
                }

                for (int i = 0; i < visible.Count - 1; i++)
                {
                    TimelineGridline tg0 = visible[i];
                    TimelineGridline tg1 = visible[i + 1];

                    if (tg1.Position - tlc.StartTime > 0 &&
                        (tg0.Position - tlc.StartTime) / tlc.SamplesPerPixel < pictureBoxMedia.Width)
                    {
                        int px0 = (int)((tg0.Position - tlc.StartTime) / tlc.SamplesPerPixel);
                        int px1 = (int)((tg1.Position - tlc.StartTime) / tlc.SamplesPerPixel) - 1;
                        using (SolidBrush sb = new SolidBrush(Color.FromArgb(tg0.R, tg0.G, tg0.B)))
                        {
                            e.Graphics.FillRectangle(sb, px0, 0, px1 - px0, pictureBoxMedia.Height - 1);
                        }
                    }
                }
            }

            foreach (Media m in t.GetMedia)
            {
                long pos0 = (m.Position - tlc.StartTime) / tlc.SamplesPerPixel;
                long pos1 = (m.Position + m.Length - tlc.StartTime) / tlc.SamplesPerPixel;

                if (pos0 < pictureBoxMedia.Width && pos1 > 0)
                {
                    int clamp0 = (int)Math.Max(0, pos0);
                    int clamp1 = (int)Math.Min(pictureBoxMedia.Width, pos1);

                    NameColor nc = m.GetNameColor();
                    using (SolidBrush sb = new SolidBrush(Color.FromArgb(nc.R, nc.G, nc.B)))
                    {
                        e.Graphics.FillRectangle(Brushes.Black, clamp0, 0, clamp1 - clamp0 - 1, pictureBoxMedia.Height - 1);
                        e.Graphics.FillRectangle(sb, clamp0 + 1, 1, clamp1 - clamp0 - 3, pictureBoxMedia.Height - 3);
                        e.Graphics.DrawString(nc.Name,new Font("Arial", 9),Brushes.White,clamp0 + 1, 1); //TODO: Contrasting color
                    }
                }
            }

            if (cursorBorder != null)
            {
                long px = cursorBorder.Position;
                if (rightBorder) px += cursorBorder.Length;

                int px1 = (int)tlc.MediaToCursorPosition(px);
                e.Graphics.FillRectangle(Brushes.Red, px1 - 1, 0, 3, pictureBoxMedia.Height);
            }

            if (draging && SrsCursor.CarriedObject is Media mm)
            {
                long pos0 = (dropLocation - tlc.StartTime) / tlc.SamplesPerPixel;
                long pos1 = (dropLocation + mm.Length - tlc.StartTime) / tlc.SamplesPerPixel;
                // TODO nice media look
                e.Graphics.FillRectangle(Brushes.Black, pos0, 0, pos1 - pos0, pictureBoxMedia.Height);
            }
        }
        private void pictureBoxMedia_MouseDown(object sender, MouseEventArgs e)
        {
            if (cursorBorder != null)
            {
                if (leftBorder)
                {
                    stretching = true;
                    leftStretch = true;
                    stretchedMedia = cursorBorder;
                }
                else if (rightBorder)
                {
                    stretching = true;
                    rightStretch = true;
                    stretchedMedia = cursorBorder;
                }
            }
            else if (cursorMedia != null)
            {
                t.RemoveMedia(cursorMedia);
                SrsCursor.CarriedObject = cursorMedia;
                pictureBoxMedia.Invalidate();
                DoDragDrop(0, DragDropEffects.Move);
            }
            else
            {
                PlaceMediaClickAtPos(e.X);
            }
        }
        private void pictureBoxMedia_MouseMove(object sender, MouseEventArgs e)
        {
            if (rightStretch)
            {
                // TODO ctrl
                long ending = tlc.CursorToMediaPosition(e.X);
                stretchedMedia.Length = ending - stretchedMedia.Position;
                if (stretchedMedia.Length < 0) stretchedMedia.Length = 0;
                pictureBoxMedia.Invalidate();
            }
            else if (leftStretch)
            {
                //TODO: 
                pictureBoxMedia.Invalidate();
            }
            else
            {
                bool r = UpdateCursorOver(e.X);
                if (r) pictureBoxMedia.Invalidate(true);
            }
        }
        private void pictureBoxMedia_MouseUp(object sender, MouseEventArgs e)
        {
            // Snap to gridlines
            if (stretching)
            {
                stretchedMedia.Position = tlc.RoundPosition(stretchedMedia.Position);
                long ending = stretchedMedia.Position + stretchedMedia.Length;
                ending = tlc.RoundPosition(ending);
                stretchedMedia.Length = ending - stretchedMedia.Position;

                if (stretchedMedia.Length == 0) t.RemoveMedia(stretchedMedia);
                stretchedMedia = null;
            }

            // State change
            leftStretch = false;
            rightStretch = false;
            stretching = false;
            pictureBoxMedia.Invalidate();
        }
        private void pictureBoxMedia_MouseLeave(object sender, EventArgs e)
        {
            cursorMedia = null;
            cursorBorder = null;
            leftBorder = false;
            rightBorder = false;
            pictureBoxMedia.Invalidate(true);
        }

        private void TrackControl_DragOver(object sender, DragEventArgs e)
        {
            if (SrsCursor.CarriedObject is Media media)
            {
                e.Effect = DragDropEffects.Move;
                dropLocation = tlc.CursorToMediaPosition(pictureBoxMedia.PointToClient(MousePosition).X);
                draging = true;
                pictureBoxMedia.Invalidate();
            }
        }
        private void TrackControl_DragDrop(object sender, DragEventArgs e)
        {
            if (SrsCursor.CarriedObject is Media media)
            {
                draging = false;
                long time = tlc.CursorToMediaPosition(pictureBoxMedia.PointToClient(MousePosition).X);
                media.Position = tlc.RoundPosition(time);
                t.AddMedia(media);
                SrsCursor.CarriedObject = null;
                pictureBoxMedia.Invalidate();
            }
        }
        private void pictureBoxMedia_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void TrackControl_DragLeave(object sender, EventArgs e)
        {
            draging = false;
            pictureBoxMedia.Invalidate();
        }
        private void pictureBoxMedia_DragLeave(object sender, EventArgs e)
        {
            draging = false;
            pictureBoxMedia.Invalidate();
        }
    }
}
