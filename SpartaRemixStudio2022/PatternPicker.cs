using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpartaRemixStudio2022
{
    public partial class PatternPicker : UserControl
    {
        public int VerticalSpacing = 20;
        public Font DisplayFont = new Font("Arial", 11);
        Brush WhiteBrush = Brushes.White;
      
        Project p = null;
        int offset = 0;

        EventHandler handler;

        public event EventHandler PatternPicked;

        public PatternPicker(Project p)
        {
            InitializeComponent();
            handler = (s, e) => { Invalidate(); };
            this.p = p;
            p.PatternsChanged += handler;
            
        }

        private void PatternPicker_Paint(object sender, PaintEventArgs e)
        {
            int y = offset;
            foreach (Pattern pat in p.GetPatterns)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(pat.NameColor.R, pat.NameColor.G, pat.NameColor.B)), new Rectangle(0, y, Width, VerticalSpacing));
                e.Graphics.DrawString($"[P{pat.Index}]" + pat.NameColor.Name, DisplayFont, WhiteBrush, new Rectangle(0, y, Width, VerticalSpacing), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                y += VerticalSpacing;
            }
            if (leftDown)
            {
                if (pickedUp > pickedUpNewPosition)
                {
                    int py0 = pickedUp * VerticalSpacing + offset;
                    int py = pickedUpNewPosition * VerticalSpacing + offset;
                    e.Graphics.DrawLine(Pens.Yellow, 0, py, Width, py);
                    e.Graphics.FillRectangle(Brushes.DarkGray, new Rectangle(0, py0, Width, VerticalSpacing));
                }
                if (pickedUp + 1 < pickedUpNewPosition)
                {
                    int py0 = pickedUp * VerticalSpacing + offset;
                    int py = pickedUpNewPosition * VerticalSpacing + offset;
                    e.Graphics.DrawLine(Pens.Yellow, 0, py, Width, py);
                    e.Graphics.FillRectangle(Brushes.DarkGray, new Rectangle(0, py0, Width, VerticalSpacing));
                }
            }
        }

        int pickedUp = -1;
        int pickedUpNewPosition = -1;
        List<Pattern> patterns = null;
        bool leftDown = false;
        private void PatternPicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                patterns = new List<Pattern>(p.GetPatterns);

                int target = (e.Y - offset) / VerticalSpacing;
                if (target >= 0 && target < patterns.Count)
                {
                    pickedUp = target;
                    pickedUpNewPosition = target;
                    selectedPattern = patterns[pickedUp];
                    leftDown = true;
                }
            }
        }
        private void PatternPicker_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                int mid = pickedUp * VerticalSpacing - VerticalSpacing / 2;
                int diff = e.Y - mid;
                if (diff > 0)
                {
                    pickedUpNewPosition = pickedUp + diff / VerticalSpacing;
                }
                else
                {
                    pickedUpNewPosition = pickedUp - (-diff) / VerticalSpacing;
                }
                if (pickedUpNewPosition < 0) pickedUpNewPosition = 0;
                if (pickedUpNewPosition > patterns.Count) pickedUpNewPosition = patterns.Count;
                Invalidate();
            }
        }
        private void PatternPicker_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                if (pickedUpNewPosition > pickedUp)
                {
                    patterns.Insert(pickedUpNewPosition, patterns[pickedUp]);
                    patterns.RemoveAt(pickedUp);
                }
                else
                {
                    Pattern p = patterns[pickedUp];
                    patterns.RemoveAt(pickedUp);
                    patterns.Insert(pickedUpNewPosition, p);
                }
                leftDown = false;
                for (int i = 0; i < patterns.Count; i++)
                {
                    patterns[i].DisplayIndex = i;
                }
                p.UpdatePatterns();

                EventHandler handler = PatternPicked;
                handler?.Invoke(this, new EventArgs());
            }
            Invalidate();
        }
        Pattern selectedPattern = null;

        override protected void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            p.PatternsChanged -= handler;
        }

        public Pattern SelectedPattern
        {
            get
            {
                return selectedPattern;
            }
        }
    }
}
