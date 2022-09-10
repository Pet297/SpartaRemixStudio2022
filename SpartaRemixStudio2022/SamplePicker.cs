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
    public partial class SamplePicker : MediaLibraryTab
    {
        AVSample picked = null;
        public SamplePicker(Project p)
        {
            InitializeComponent();
            ListBoxSamples.DataSource = p.GetSamples.ToList();
        }

        private void ListBoxSamples_SelectedValueChanged(object sender, EventArgs e)
        {
            picked = ListBoxSamples.SelectedItem as AVSample;
        }

        public override Media GetMedia(Project p)
        {
            if (picked == null) return null;

            //TODO: Sample ID
            SampleMedia sm = new SampleMedia(picked.Index);
            sm.Init(p);
            return new Media(sm);
        }
    }
}
