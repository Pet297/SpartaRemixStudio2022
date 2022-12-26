namespace SpartaRemixStudio2022
{
    partial class FormPatternEdit
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PatternListBack = new System.Windows.Forms.Panel();
            this.PatternEditTimelineBack = new System.Windows.Forms.Panel();
            this.ButtonNewPattern = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SelectedPattern = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.mediaLibraryControl1 = new SpartaRemixStudio2022.MediaLibraryControl();
            this.SuspendLayout();
            // 
            // PatternListBack
            // 
            this.PatternListBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.PatternListBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.PatternListBack.Location = new System.Drawing.Point(0, 0);
            this.PatternListBack.Name = "PatternListBack";
            this.PatternListBack.Size = new System.Drawing.Size(260, 703);
            this.PatternListBack.TabIndex = 2;
            // 
            // PatternEditTimelineBack
            // 
            this.PatternEditTimelineBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.PatternEditTimelineBack.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PatternEditTimelineBack.Location = new System.Drawing.Point(260, 326);
            this.PatternEditTimelineBack.Name = "PatternEditTimelineBack";
            this.PatternEditTimelineBack.Size = new System.Drawing.Size(982, 377);
            this.PatternEditTimelineBack.TabIndex = 3;
            // 
            // ButtonNewPattern
            // 
            this.ButtonNewPattern.Location = new System.Drawing.Point(333, 101);
            this.ButtonNewPattern.Name = "ButtonNewPattern";
            this.ButtonNewPattern.Size = new System.Drawing.Size(123, 23);
            this.ButtonNewPattern.TabIndex = 4;
            this.ButtonNewPattern.Text = "New Pattern";
            this.ButtonNewPattern.UseVisualStyleBackColor = true;
            this.ButtonNewPattern.Click += new System.EventHandler(this.ButtonNewPattern_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(333, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Dupe Pattern";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SelectedPattern
            // 
            this.SelectedPattern.AutoSize = true;
            this.SelectedPattern.Location = new System.Drawing.Point(330, 68);
            this.SelectedPattern.Name = "SelectedPattern";
            this.SelectedPattern.Size = new System.Drawing.Size(117, 13);
            this.SelectedPattern.TabIndex = 6;
            this.SelectedPattern.Text = "Selected pattern: None";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(462, 101);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(123, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Delete Pattern";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // mediaLibraryControl1
            // 
            this.mediaLibraryControl1.Location = new System.Drawing.Point(734, 12);
            this.mediaLibraryControl1.Name = "mediaLibraryControl1";
            this.mediaLibraryControl1.Size = new System.Drawing.Size(505, 312);
            this.mediaLibraryControl1.TabIndex = 8;
            // 
            // FormPatternEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 703);
            this.Controls.Add(this.mediaLibraryControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SelectedPattern);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ButtonNewPattern);
            this.Controls.Add(this.PatternEditTimelineBack);
            this.Controls.Add(this.PatternListBack);
            this.Name = "FormPatternEdit";
            this.Text = "FormPatternEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel PatternListBack;
        private System.Windows.Forms.Panel PatternEditTimelineBack;
        private System.Windows.Forms.Button ButtonNewPattern;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label SelectedPattern;
        private System.Windows.Forms.Button button2;
        private MediaLibraryControl mediaLibraryControl1;
    }
}