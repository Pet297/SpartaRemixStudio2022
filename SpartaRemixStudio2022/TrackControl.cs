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

        public TrackControl(TimelineControl tlc, Timeline timeline, Track track)
        {
            InitializeComponent();
            t = track;
            tl = timeline;
            this.tlc = tlc;
        }
        private void pictureBoxMedia_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkGray, 0, 0, pictureBoxMedia.Width, pictureBoxMedia.Height);
            int r = -1;

            if (tl != null)
            {
                foreach (TimelineGridline tg in tl.Gridlines)
                {
                    if (tlc.SamplesPerPixel <= tg.MaxSamplesPerPixel && tg.Position - tlc.StartTime > 0 && (tg.Position - tlc.StartTime) / tlc.SamplesPerPixel < pictureBoxMedia.Width)
                    {
                        r++;
                        int px = (int)((tg.Position - tlc.StartTime) / tlc.SamplesPerPixel);
                        e.Graphics.FillRectangle((r % 2 == 0 ? Brushes.LightGray : Brushes.DarkGray), px, 0, pictureBoxMedia.Width, pictureBoxMedia.Height);
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
                    // TODO nice media look
                    e.Graphics.FillRectangle(Brushes.Orange, clamp0, 0, clamp1 - clamp0, pictureBoxMedia.Height);
                }
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
            Media media = t.ColideMedia(tlc.CursorToMediaPosition(e.X));
            if (media != null)
            {
                int pixelStart = (int)tlc.MediaToCursorPosition(media.Position);
                int pixelEnd = (int)tlc.MediaToCursorPosition(media.Position + media.Length);
                int pixelDown = e.X;

                // TODO: konstanta 10, co s ni jak nastavit
                if (pixelDown - pixelStart < 10)
                {
                    stretching = true;
                    leftStretch = true;
                    stretchedMedia = media;
                }
                else if (pixelEnd - pixelDown < 10)
                {
                    stretching = true;
                    rightStretch = true;
                    stretchedMedia = media;
                }
                else
                {
                    t.RemoveMedia(media);
                    SrsCursor.CarriedObject = media;
                    pictureBoxMedia.Invalidate();
                    DoDragDrop(0, DragDropEffects.Move);
                }
            }
            else
            {
                Media newMedia = mlc.GetMedia();
                if (newMedia != null)
                {
                    long pos = tlc.CursorToMediaPosition(e.X);
                    newMedia.Position = tlc.RoundPosition(pos);
                    newMedia.Length = 1000; // TODO: Default based on settings
                    pictureBoxMedia.Invalidate();
                }
            }
        }
        private void pictureBoxMedia_MouseMove(object sender, MouseEventArgs e)
        {
            if (rightStretch)
            {
                long ending = tlc.CursorToMediaPosition(e.X);
                stretchedMedia.Length = ending - stretchedMedia.Position;
                if (stretchedMedia.Length < 0) stretchedMedia.Length = 0;
                pictureBoxMedia.Invalidate();
            }
            if (leftStretch)
            {
                //TODO: 
                pictureBoxMedia.Invalidate();
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
