namespace ClayBotScreenshotHelper
{
    partial class HelperForm
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
            this.savePathLabel = new System.Windows.Forms.Label();
            this.savePathTextBox = new System.Windows.Forms.TextBox();
            this.savePathButton = new System.Windows.Forms.Button();
            this.targetLabel = new System.Windows.Forms.Label();
            this.targetComboBox = new System.Windows.Forms.ComboBox();
            this.thresholdCheckBox = new System.Windows.Forms.CheckBox();
            this.screenshotButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // savePathLabel
            // 
            this.savePathLabel.AutoSize = true;
            this.savePathLabel.Location = new System.Drawing.Point(12, 9);
            this.savePathLabel.Name = "savePathLabel";
            this.savePathLabel.Size = new System.Drawing.Size(62, 12);
            this.savePathLabel.TabIndex = 0;
            this.savePathLabel.Text = "Save Path";
            // 
            // savePathTextBox
            // 
            this.savePathTextBox.Location = new System.Drawing.Point(12, 24);
            this.savePathTextBox.Name = "savePathTextBox";
            this.savePathTextBox.ReadOnly = true;
            this.savePathTextBox.Size = new System.Drawing.Size(179, 21);
            this.savePathTextBox.TabIndex = 1;
            // 
            // savePathButton
            // 
            this.savePathButton.Location = new System.Drawing.Point(197, 22);
            this.savePathButton.Name = "savePathButton";
            this.savePathButton.Size = new System.Drawing.Size(75, 23);
            this.savePathButton.TabIndex = 2;
            this.savePathButton.Text = "...";
            this.savePathButton.UseVisualStyleBackColor = true;
            this.savePathButton.Click += new System.EventHandler(this.savePathButton_Click);
            // 
            // targetLabel
            // 
            this.targetLabel.AutoSize = true;
            this.targetLabel.Location = new System.Drawing.Point(12, 48);
            this.targetLabel.Name = "targetLabel";
            this.targetLabel.Size = new System.Drawing.Size(80, 12);
            this.targetLabel.TabIndex = 3;
            this.targetLabel.Text = "Select Target";
            // 
            // targetComboBox
            // 
            this.targetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetComboBox.FormattingEnabled = true;
            this.targetComboBox.Items.AddRange(new object[] {
            "Patcher",
            "Client",
            "Game"});
            this.targetComboBox.Location = new System.Drawing.Point(12, 63);
            this.targetComboBox.Name = "targetComboBox";
            this.targetComboBox.Size = new System.Drawing.Size(260, 20);
            this.targetComboBox.TabIndex = 4;
            // 
            // thresholdCheckBox
            // 
            this.thresholdCheckBox.AutoSize = true;
            this.thresholdCheckBox.Location = new System.Drawing.Point(12, 89);
            this.thresholdCheckBox.Name = "thresholdCheckBox";
            this.thresholdCheckBox.Size = new System.Drawing.Size(87, 16);
            this.thresholdCheckBox.TabIndex = 5;
            this.thresholdCheckBox.Text = "Threshold?";
            this.thresholdCheckBox.UseVisualStyleBackColor = true;
            // 
            // screenshotButton
            // 
            this.screenshotButton.Location = new System.Drawing.Point(12, 226);
            this.screenshotButton.Name = "screenshotButton";
            this.screenshotButton.Size = new System.Drawing.Size(260, 23);
            this.screenshotButton.TabIndex = 6;
            this.screenshotButton.Text = "Take Screenshot";
            this.screenshotButton.UseVisualStyleBackColor = true;
            this.screenshotButton.Click += new System.EventHandler(this.screenshotButton_Click);
            // 
            // HelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.screenshotButton);
            this.Controls.Add(this.thresholdCheckBox);
            this.Controls.Add(this.targetComboBox);
            this.Controls.Add(this.targetLabel);
            this.Controls.Add(this.savePathButton);
            this.Controls.Add(this.savePathTextBox);
            this.Controls.Add(this.savePathLabel);
            this.MaximizeBox = false;
            this.Name = "HelperForm";
            this.ShowIcon = false;
            this.Text = "ClayBot Screenshot Helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label savePathLabel;
        private System.Windows.Forms.TextBox savePathTextBox;
        private System.Windows.Forms.Button savePathButton;
        private System.Windows.Forms.Label targetLabel;
        private System.Windows.Forms.ComboBox targetComboBox;
        private System.Windows.Forms.CheckBox thresholdCheckBox;
        private System.Windows.Forms.Button screenshotButton;
    }
}

