namespace SpartaRemixStudio2022
{
    partial class PatternPicker
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
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(269, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 502);
            this.vScrollBar1.TabIndex = 0;
            // 
            // PatternPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.vScrollBar1);
            this.DoubleBuffered = true;
            this.Name = "PatternPicker";
            this.Size = new System.Drawing.Size(286, 502);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PatternPicker_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PatternPicker_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PatternPicker_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PatternPicker_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar1;
    }
}
