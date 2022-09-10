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
            this.SuspendLayout();
            // 
            // HScrollTime
            // 
            this.HScrollTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.HScrollTime.Location = new System.Drawing.Point(0, 381);
            this.HScrollTime.Name = "HScrollTime";
            this.HScrollTime.Size = new System.Drawing.Size(829, 17);
            this.HScrollTime.TabIndex = 0;
            this.HScrollTime.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HScrollTime_Scroll);
            // 
            // PanelTracks
            // 
            this.PanelTracks.AutoScroll = true;
            this.PanelTracks.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.PanelTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTracks.Location = new System.Drawing.Point(0, 0);
            this.PanelTracks.Name = "PanelTracks";
            this.PanelTracks.Size = new System.Drawing.Size(829, 381);
            this.PanelTracks.TabIndex = 1;
            // 
            // TimelineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelTracks);
            this.Controls.Add(this.HScrollTime);
            this.Name = "TimelineControl";
            this.Size = new System.Drawing.Size(829, 398);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TimelineControl_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseUp);
            this.Resize += new System.EventHandler(this.TimelineControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar HScrollTime;
        private System.Windows.Forms.Panel PanelTracks;
    }
}
