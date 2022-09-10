
namespace SpartaRemixStudio2022
{
    partial class VideoPlayer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.ButtonForward = new System.Windows.Forms.Button();
            this.ButtonBack = new System.Windows.Forms.Button();
            this.LabelTimecode = new System.Windows.Forms.Label();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.ButtonPause = new System.Windows.Forms.Button();
            this.ButtonPlay = new System.Windows.Forms.Button();
            this.ProgressPictureBox = new System.Windows.Forms.PictureBox();
            this.VideoPictureBox = new System.Windows.Forms.PictureBox();
            this.LabelStatus = new System.Windows.Forms.Label();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ControlPanel
            // 
            this.ControlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ControlPanel.Controls.Add(this.ButtonForward);
            this.ControlPanel.Controls.Add(this.ButtonBack);
            this.ControlPanel.Controls.Add(this.LabelTimecode);
            this.ControlPanel.Controls.Add(this.ButtonStop);
            this.ControlPanel.Controls.Add(this.ButtonPause);
            this.ControlPanel.Controls.Add(this.ButtonPlay);
            this.ControlPanel.Controls.Add(this.ProgressPictureBox);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ControlPanel.Location = new System.Drawing.Point(0, 290);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(655, 58);
            this.ControlPanel.TabIndex = 0;
            // 
            // ButtonForward
            // 
            this.ButtonForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonForward.ForeColor = System.Drawing.Color.White;
            this.ButtonForward.Location = new System.Drawing.Point(387, 22);
            this.ButtonForward.Name = "ButtonForward";
            this.ButtonForward.Size = new System.Drawing.Size(52, 33);
            this.ButtonForward.TabIndex = 6;
            this.ButtonForward.Text = "+10s";
            this.ButtonForward.UseVisualStyleBackColor = true;
            this.ButtonForward.Click += new System.EventHandler(this.ButtonForward_Click);
            // 
            // ButtonBack
            // 
            this.ButtonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonBack.ForeColor = System.Drawing.Color.White;
            this.ButtonBack.Location = new System.Drawing.Point(155, 22);
            this.ButtonBack.Name = "ButtonBack";
            this.ButtonBack.Size = new System.Drawing.Size(52, 33);
            this.ButtonBack.TabIndex = 5;
            this.ButtonBack.Text = "-10s";
            this.ButtonBack.UseVisualStyleBackColor = true;
            this.ButtonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // LabelTimecode
            // 
            this.LabelTimecode.AutoSize = true;
            this.LabelTimecode.ForeColor = System.Drawing.Color.White;
            this.LabelTimecode.Location = new System.Drawing.Point(516, 32);
            this.LabelTimecode.Name = "LabelTimecode";
            this.LabelTimecode.Size = new System.Drawing.Size(102, 13);
            this.LabelTimecode.TabIndex = 4;
            this.LabelTimecode.Text = "00:00:00 / 00:00:00";
            // 
            // ButtonStop
            // 
            this.ButtonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonStop.ForeColor = System.Drawing.Color.White;
            this.ButtonStop.Location = new System.Drawing.Point(329, 22);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(52, 33);
            this.ButtonStop.TabIndex = 3;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // ButtonPause
            // 
            this.ButtonPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonPause.ForeColor = System.Drawing.Color.White;
            this.ButtonPause.Location = new System.Drawing.Point(271, 22);
            this.ButtonPause.Name = "ButtonPause";
            this.ButtonPause.Size = new System.Drawing.Size(52, 33);
            this.ButtonPause.TabIndex = 2;
            this.ButtonPause.Text = "Pause";
            this.ButtonPause.UseVisualStyleBackColor = true;
            this.ButtonPause.Click += new System.EventHandler(this.ButtonPause_Click);
            // 
            // ButtonPlay
            // 
            this.ButtonPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonPlay.ForeColor = System.Drawing.Color.White;
            this.ButtonPlay.Location = new System.Drawing.Point(213, 22);
            this.ButtonPlay.Name = "ButtonPlay";
            this.ButtonPlay.Size = new System.Drawing.Size(52, 33);
            this.ButtonPlay.TabIndex = 1;
            this.ButtonPlay.Text = "Play";
            this.ButtonPlay.UseVisualStyleBackColor = true;
            this.ButtonPlay.Click += new System.EventHandler(this.ButtonPlay_Click);
            // 
            // ProgressPictureBox
            // 
            this.ProgressPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProgressPictureBox.Location = new System.Drawing.Point(0, 0);
            this.ProgressPictureBox.Name = "ProgressPictureBox";
            this.ProgressPictureBox.Size = new System.Drawing.Size(655, 16);
            this.ProgressPictureBox.TabIndex = 0;
            this.ProgressPictureBox.TabStop = false;
            // 
            // VideoPictureBox
            // 
            this.VideoPictureBox.BackColor = System.Drawing.Color.Black;
            this.VideoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VideoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.VideoPictureBox.Name = "VideoPictureBox";
            this.VideoPictureBox.Size = new System.Drawing.Size(655, 290);
            this.VideoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.VideoPictureBox.TabIndex = 1;
            this.VideoPictureBox.TabStop = false;
            // 
            // LabelStatus
            // 
            this.LabelStatus.AutoSize = true;
            this.LabelStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LabelStatus.ForeColor = System.Drawing.Color.White;
            this.LabelStatus.Location = new System.Drawing.Point(13, 11);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(96, 13);
            this.LabelStatus.TabIndex = 7;
            this.LabelStatus.Text = "No video file open.";
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 30;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // VideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelStatus);
            this.Controls.Add(this.VideoPictureBox);
            this.Controls.Add(this.ControlPanel);
            this.Name = "VideoPlayer";
            this.Size = new System.Drawing.Size(655, 348);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.Button ButtonPause;
        private System.Windows.Forms.Button ButtonPlay;
        private System.Windows.Forms.PictureBox ProgressPictureBox;
        private System.Windows.Forms.PictureBox VideoPictureBox;
        private System.Windows.Forms.Button ButtonForward;
        private System.Windows.Forms.Button ButtonBack;
        private System.Windows.Forms.Label LabelTimecode;
        private System.Windows.Forms.Label LabelStatus;
        private System.Windows.Forms.Timer Timer1;
    }
}
