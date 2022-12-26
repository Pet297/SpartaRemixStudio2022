using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpartaRemixStudio2022
{
    public partial class FormPatternEdit : Form
    {
        TimelineControl<SimpleTrack> PatternEditor = null;

        Project p = null; 
        public FormPatternEdit(Project p)
        {
            InitializeComponent();
            mediaLibraryControl1.p = p;
            this.p = p;

            PatternPicker picker = new PatternPicker(p);
            picker.Parent = PatternListBack;
            picker.Dock = DockStyle.Fill;
        }

        void OpenEditor(Pattern pat)
        {
            PatternEditor = new TimelineControl<SimpleTrack>(pat);
            PatternEditor.Parent = PatternEditTimelineBack;
            PatternEditor.Dock = DockStyle.Fill;
        }
        void CloseEditor()
        {
            PatternEditor.Parent = null;
            PatternEditor.Dispose();
            PatternEditor = null;
        }

        private void ButtonNewPattern_Click(object sender, EventArgs e)
        {
            p.AddPattern();
        }
    }
}
