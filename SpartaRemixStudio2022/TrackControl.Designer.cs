namespace SpartaRemixStudio2022
{
    partial class TrackControl<T> where T : IEditableTrack
    {
        /// <summary> 
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód vygenerovaný pomocí Návrháře komponent

        /// <summary> 
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelSide = new System.Windows.Forms.Panel();
            this.labelName = new System.Windows.Forms.Label();
            this.buttonParentDown = new System.Windows.Forms.Button();
            this.buttonParentUp = new System.Windows.Forms.Button();
            this.buttonVFX = new System.Windows.Forms.Button();
            this.buttonAFX = new System.Windows.Forms.Button();
            this.buttonSolo = new System.Windows.Forms.Button();
            this.buttonMute = new System.Windows.Forms.Button();
            this.PictureBoxMedia = new System.Windows.Forms.PictureBox();
            this.panelSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMedia)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSide
            // 
            this.panelSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelSide.Controls.Add(this.labelName);
            this.panelSide.Controls.Add(this.buttonParentDown);
            this.panelSide.Controls.Add(this.buttonParentUp);
            this.panelSide.Controls.Add(this.buttonVFX);
            this.panelSide.Controls.Add(this.buttonAFX);
            this.panelSide.Controls.Add(this.buttonSolo);
            this.panelSide.Controls.Add(this.buttonMute);
            this.panelSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSide.Location = new System.Drawing.Point(0, 0);
            this.panelSide.Name = "panelSide";
            this.panelSide.Size = new System.Drawing.Size(189, 99);
            this.panelSide.TabIndex = 0;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 10);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(56, 13);
            this.labelName.TabIndex = 6;
            this.labelName.Text = "Track 001";
            // 
            // buttonParentDown
            // 
            this.buttonParentDown.Location = new System.Drawing.Point(87, 35);
            this.buttonParentDown.Name = "buttonParentDown";
            this.buttonParentDown.Size = new System.Drawing.Size(28, 26);
            this.buttonParentDown.TabIndex = 5;
            this.buttonParentDown.Text = "Pv";
            this.buttonParentDown.UseVisualStyleBackColor = true;
            // 
            // buttonParentUp
            // 
            this.buttonParentUp.Location = new System.Drawing.Point(87, 3);
            this.buttonParentUp.Name = "buttonParentUp";
            this.buttonParentUp.Size = new System.Drawing.Size(28, 26);
            this.buttonParentUp.TabIndex = 4;
            this.buttonParentUp.Text = "P^";
            this.buttonParentUp.UseVisualStyleBackColor = true;
            // 
            // buttonVFX
            // 
            this.buttonVFX.Location = new System.Drawing.Point(121, 35);
            this.buttonVFX.Name = "buttonVFX";
            this.buttonVFX.Size = new System.Drawing.Size(28, 26);
            this.buttonVFX.TabIndex = 3;
            this.buttonVFX.Text = "V";
            this.buttonVFX.UseVisualStyleBackColor = true;
            // 
            // buttonAFX
            // 
            this.buttonAFX.Location = new System.Drawing.Point(121, 3);
            this.buttonAFX.Name = "buttonAFX";
            this.buttonAFX.Size = new System.Drawing.Size(28, 26);
            this.buttonAFX.TabIndex = 2;
            this.buttonAFX.Text = "A";
            this.buttonAFX.UseVisualStyleBackColor = true;
            // 
            // buttonSolo
            // 
            this.buttonSolo.Location = new System.Drawing.Point(155, 35);
            this.buttonSolo.Name = "buttonSolo";
            this.buttonSolo.Size = new System.Drawing.Size(28, 26);
            this.buttonSolo.TabIndex = 1;
            this.buttonSolo.Text = "S";
            this.buttonSolo.UseVisualStyleBackColor = true;
            // 
            // buttonMute
            // 
            this.buttonMute.Location = new System.Drawing.Point(155, 3);
            this.buttonMute.Name = "buttonMute";
            this.buttonMute.Size = new System.Drawing.Size(28, 26);
            this.buttonMute.TabIndex = 0;
            this.buttonMute.Text = "M";
            this.buttonMute.UseVisualStyleBackColor = true;
            // 
            // PictureBoxMedia
            // 
            this.PictureBoxMedia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBoxMedia.Location = new System.Drawing.Point(189, 0);
            this.PictureBoxMedia.Name = "PictureBoxMedia";
            this.PictureBoxMedia.Size = new System.Drawing.Size(424, 99);
            this.PictureBoxMedia.TabIndex = 0;
            this.PictureBoxMedia.TabStop = false;
            this.PictureBoxMedia.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBoxMedia_Paint);
            this.PictureBoxMedia.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMedia_MouseDown);
            this.PictureBoxMedia.MouseLeave += new System.EventHandler(this.PictureBoxMedia_MouseLeave);
            this.PictureBoxMedia.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMedia_MouseMove);
            this.PictureBoxMedia.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMedia_MouseUp);
            // 
            // TrackControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PictureBoxMedia);
            this.Controls.Add(this.panelSide);
            this.DoubleBuffered = true;
            this.Name = "TrackControl";
            this.Size = new System.Drawing.Size(613, 99);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureBoxMedia_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBoxMedia_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.PictureBoxMedia_DragOver);
            this.DragLeave += new System.EventHandler(this.PictureBoxMedia_DragLeave);
            this.panelSide.ResumeLayout(false);
            this.panelSide.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMedia)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSide;
        private System.Windows.Forms.Button buttonParentDown;
        private System.Windows.Forms.Button buttonParentUp;
        private System.Windows.Forms.Button buttonVFX;
        private System.Windows.Forms.Button buttonAFX;
        private System.Windows.Forms.Button buttonSolo;
        private System.Windows.Forms.Button buttonMute;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.PictureBox PictureBoxMedia;
    }
}
