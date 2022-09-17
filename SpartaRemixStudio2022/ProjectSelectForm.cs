using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpartaRemixStudio2022
{
    public partial class ProjectSelectForm : Form
    {
        public ProjectSelectForm()
        {
            InitializeComponent();
        }

        private void ProjectSelectForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("Projects"))
            {
                Directory.CreateDirectory("Projects");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("Projects/Test.srsp"))
            {
                FormMain fm = new FormMain("Projects/Test.srsp");
                Hide();
                fm.ShowDialog();
                Close();
            }
            else
            {
                FormMain fm = new FormMain();
                Hide();
                fm.ShowDialog();
                Close();
            }
        }
    }
}
