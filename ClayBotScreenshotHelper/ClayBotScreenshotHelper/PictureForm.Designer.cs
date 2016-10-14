namespace ClayBotScreenshotHelper
{
    partial class PictureForm
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
            this.rectangleInfoTextBox = new System.Windows.Forms.TextBox();
            this.panel = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.cropAndSaveButton = new System.Windows.Forms.Button();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // rectangleInfoTextBox
            // 
            this.rectangleInfoTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.rectangleInfoTextBox.Location = new System.Drawing.Point(0, 0);
            this.rectangleInfoTextBox.Name = "rectangleInfoTextBox";
            this.rectangleInfoTextBox.Size = new System.Drawing.Size(284, 21);
            this.rectangleInfoTextBox.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Controls.Add(this.pictureBox);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 44);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(284, 217);
            this.panel.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // cropAndSaveButton
            // 
            this.cropAndSaveButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.cropAndSaveButton.Location = new System.Drawing.Point(0, 21);
            this.cropAndSaveButton.Name = "cropAndSaveButton";
            this.cropAndSaveButton.Size = new System.Drawing.Size(284, 23);
            this.cropAndSaveButton.TabIndex = 2;
            this.cropAndSaveButton.Text = "Crop and Save";
            this.cropAndSaveButton.UseVisualStyleBackColor = true;
            this.cropAndSaveButton.Click += new System.EventHandler(this.cropAndSaveButton_Click);
            // 
            // PictureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.cropAndSaveButton);
            this.Controls.Add(this.rectangleInfoTextBox);
            this.MaximizeBox = false;
            this.Name = "PictureForm";
            this.ShowIcon = false;
            this.Text = "PictureForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox rectangleInfoTextBox;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button cropAndSaveButton;
    }
}