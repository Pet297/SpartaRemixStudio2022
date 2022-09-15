namespace SpartaRemixStudio2022
{
    partial class TimelineControl
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
            this.HScrollTime = new System.Windows.Forms.HScrollBar();
            this.PanelTracks = new System.Windows.Forms.Panel();
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.ButtonCorner = new System.Windows.Forms.Button();
            this.PanelRuler = new System.Windows.Forms.Panel();
            this.VScrollBarTracks = new System.Windows.Forms.VScrollBar();
            this.PanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // HScrollTime
            // 
            this.HScrollTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HScrollTime.Location = new System.Drawing.Point(0, 0);
            this.HScrollTime.Name = "HScrollTime";
            this.HScrollTime.Size = new System.Drawing.Size(806, 23);
            this.HScrollTime.TabIndex = 0;
            this.HScrollTime.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HScrollTime_Scroll);
            // 
            // PanelTracks
            // 
            this.PanelTracks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.PanelTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTracks.Location = new System.Drawing.Point(0, 23);
            this.PanelTracks.Name = "PanelTracks";
            this.PanelTracks.Size = new System.Drawing.Size(806, 352);
            this.PanelTracks.TabIndex = 1;
            this.PanelTracks.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PanelTracks_Scroll);
            this.PanelTracks.SizeChanged += new System.EventHandler(this.PanelTracks_SizeChanged);
            // 
            // PanelBottom
            // 
            this.PanelBottom.Controls.Add(this.HScrollTime);
            this.PanelBottom.Controls.Add(this.ButtonCorner);
            this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelBottom.Location = new System.Drawing.Point(0, 375);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(829, 23);
            this.PanelBottom.TabIndex = 0;
            // 
            // ButtonCorner
            // 
            this.ButtonCorner.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonCorner.Location = new System.Drawing.Point(806, 0);
            this.ButtonCorner.Name = "ButtonCorner";
            this.ButtonCorner.Size = new System.Drawing.Size(23, 23);
            this.ButtonCorner.TabIndex = 0;
            this.ButtonCorner.Text = "-";
            this.ButtonCorner.UseVisualStyleBackColor = true;
            // 
            // PanelRuler
            // 
            this.PanelRuler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.PanelRuler.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelRuler.Location = new System.Drawing.Point(0, 0);
            this.PanelRuler.Name = "PanelRuler";
            this.PanelRuler.Size = new System.Drawing.Size(829, 23);
            this.PanelRuler.TabIndex = 2;
            this.PanelRuler.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelRuler_Paint);
            // 
            // VScrollBarTracks
            // 
            this.VScrollBarTracks.Dock = System.Windows.Forms.DockStyle.Right;
            this.VScrollBarTracks.Location = new System.Drawing.Point(806, 23);
            this.VScrollBarTracks.Name = "VScrollBarTracks";
            this.VScrollBarTracks.Size = new System.Drawing.Size(23, 352);
            this.VScrollBarTracks.TabIndex = 3;
            // 
            // TimelineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelTracks);
            this.Controls.Add(this.VScrollBarTracks);
            this.Controls.Add(this.PanelRuler);
            this.Controls.Add(this.PanelBottom);
            this.Name = "TimelineControl";
            this.Size = new System.Drawing.Size(829, 398);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TimelineControl_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseUp);
            this.Resize += new System.EventHandler(this.TimelineControl_Resize);
            this.PanelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar HScrollTime;
        private System.Windows.Forms.Panel PanelTracks;
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Button ButtonCorner;
        private System.Windows.Forms.Panel PanelRuler;
        private System.Windows.Forms.VScrollBar VScrollBarTracks;
    }
}
