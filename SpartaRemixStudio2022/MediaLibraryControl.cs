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
    public partial class MediaLibraryControl : UserControl
    {
        // TODO: Not public
        public Project p = null;
        public MediaLibraryControl()
        {
            InitializeComponent();
        }

        // TODO: Generailze
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SamplePicker sp = new SamplePicker(p);
                sp.Parent = panel1;
                sp.Dock = DockStyle.Fill;
                this.sp = sp;
            }
        }

        // DEBUG
        SamplePicker sp;

        // TODO: Generailze
        public Media GetMedia()
        {
            if (sp != null)
            {
                return sp.GetMedia(p);
            }
            return null;
        }
    }
}
