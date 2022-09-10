namespace SpartaRemixStudio2022
{
    partial class SamplePicker
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
            this.ListBoxSamples = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ListBoxSamples
            // 
            this.ListBoxSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListBoxSamples.FormattingEnabled = true;
            this.ListBoxSamples.Location = new System.Drawing.Point(0, 0);
            this.ListBoxSamples.Name = "ListBoxSamples";
            this.ListBoxSamples.Size = new System.Drawing.Size(398, 266);
            this.ListBoxSamples.TabIndex = 0;
            this.ListBoxSamples.SelectedValueChanged += new System.EventHandler(this.ListBoxSamples_SelectedValueChanged);
            // 
            // SamplePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ListBoxSamples);
            this.Name = "SamplePicker";
            this.Size = new System.Drawing.Size(398, 266);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListBoxSamples;
    }
}
