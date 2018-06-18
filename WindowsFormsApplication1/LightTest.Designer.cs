namespace WindowsFormsApplication1
{
    partial class LightTest
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
            this.testPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // testPanel
            // 
            this.testPanel.Location = new System.Drawing.Point(601, 41);
            this.testPanel.Name = "testPanel";
            this.testPanel.Size = new System.Drawing.Size(640, 360);
            this.testPanel.TabIndex = 0;
            this.testPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.testPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // LightTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 556);
            this.Controls.Add(this.testPanel);
            this.Name = "LightTest";
            this.Text = "LightTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel testPanel;

    }
}