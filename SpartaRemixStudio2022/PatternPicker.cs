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
    public partial class PatternPicker : UserControl
    {
        public int VerticalSpacing = 15;
        public Font DisplayFont = new Font("Arial", 11);
        Brush WhiteBrush = Brushes.White;

        public Project p = null;
        int offset = 0;

        public PatternPicker()
        {
            InitializeComponent();
        }

        private void PatternPicker_Paint(object sender, PaintEventArgs e)
        {
            int y = offset;
            foreach(Pattern pat in p.GetPatterns)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(pat.NameColor.R, pat.NameColor.G, pat.NameColor.B)), new Rectangle(0, y, Width, VerticalSpacing));
                e.Graphics.DrawString(pat.NameColor.Name, DisplayFont, WhiteBrush, 0, y);
                y += VerticalSpacing;
            }
        }

        private void PatternPicker_MouseDown(object sender, MouseEventArgs e)
        {

        }
        private void PatternPicker_MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void PatternPicker_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
