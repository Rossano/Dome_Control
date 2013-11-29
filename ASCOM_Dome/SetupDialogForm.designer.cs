namespace ASCOM.Arduino
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.TelesChooseLabel = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.DomeCOMLabel = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.TelescopeChooserButton = new System.Windows.Forms.Button();
            this.DomeCOMcomboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(281, 112);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(281, 142);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // TelesChooseLabel
            // 
            this.TelesChooseLabel.Location = new System.Drawing.Point(12, 66);
            this.TelesChooseLabel.Name = "TelesChooseLabel";
            this.TelesChooseLabel.Size = new System.Drawing.Size(162, 39);
            this.TelesChooseLabel.TabIndex = 2;
            this.TelesChooseLabel.Text = "Construct your driver\'s setup dialog here.";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Arduino.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(292, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // DomeCOMLabel
            // 
            this.DomeCOMLabel.AutoSize = true;
            this.DomeCOMLabel.Location = new System.Drawing.Point(12, 27);
            this.DomeCOMLabel.Name = "DomeCOMLabel";
            this.DomeCOMLabel.Size = new System.Drawing.Size(58, 13);
            this.DomeCOMLabel.TabIndex = 5;
            this.DomeCOMLabel.Text = "Comm Port";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(15, 142);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // TelescopeChooserButton
            // 
            this.TelescopeChooserButton.Location = new System.Drawing.Point(194, 66);
            this.TelescopeChooserButton.Name = "TelescopeChooserButton";
            this.TelescopeChooserButton.Size = new System.Drawing.Size(75, 23);
            this.TelescopeChooserButton.TabIndex = 7;
            this.TelescopeChooserButton.Text = "button1";
            this.TelescopeChooserButton.UseVisualStyleBackColor = true;
            this.TelescopeChooserButton.Click += new System.EventHandler(this.TelescopeChooserButton_Click);
            // 
            // DomeCOMcomboBox
            // 
            this.DomeCOMcomboBox.FormattingEnabled = true;
            this.DomeCOMcomboBox.Location = new System.Drawing.Point(194, 24);
            this.DomeCOMcomboBox.Name = "DomeCOMcomboBox";
            this.DomeCOMcomboBox.Size = new System.Drawing.Size(75, 21);
            this.DomeCOMcomboBox.TabIndex = 8;
            this.DomeCOMcomboBox.SelectedValueChanged += new System.EventHandler(this.DomeCOMcomboBox_SelectedValueChanged);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 175);
            this.Controls.Add(this.DomeCOMcomboBox);
            this.Controls.Add(this.TelescopeChooserButton);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.DomeCOMLabel);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.TelesChooseLabel);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arduino Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label TelesChooseLabel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label DomeCOMLabel;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.Button TelescopeChooserButton;
        private System.Windows.Forms.ComboBox DomeCOMcomboBox;
    }
}