namespace Sapper.WinUI {
	partial class AboutBox {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.tblLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.lblProductName = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.linkEMail = new System.Windows.Forms.LinkLabel();
			this.okButton = new System.Windows.Forms.Button();
			this.tblLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tblLayoutPanel
			// 
			this.tblLayoutPanel.ColumnCount = 1;
			this.tblLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutPanel.Controls.Add(this.lblProductName, 0, 0);
			this.tblLayoutPanel.Controls.Add(this.lblVersion, 0, 1);
			this.tblLayoutPanel.Controls.Add(this.lblCopyright, 0, 2);
			this.tblLayoutPanel.Controls.Add(this.okButton, 0, 4);
			this.tblLayoutPanel.Controls.Add(this.linkEMail, 0, 3);
			this.tblLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblLayoutPanel.Location = new System.Drawing.Point(9, 9);
			this.tblLayoutPanel.Name = "tblLayoutPanel";
			this.tblLayoutPanel.RowCount = 5;
			this.tblLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.64706F));
			this.tblLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.64706F));
			this.tblLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.64706F));
			this.tblLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.52941F));
			this.tblLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.52941F));
			this.tblLayoutPanel.Size = new System.Drawing.Size(133, 143);
			this.tblLayoutPanel.TabIndex = 0;
			// 
			// lblProductName
			// 
			this.lblProductName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblProductName.Location = new System.Drawing.Point(6, 0);
			this.lblProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.lblProductName.MaximumSize = new System.Drawing.Size(0, 17);
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.Size = new System.Drawing.Size(124, 17);
			this.lblProductName.TabIndex = 19;
			this.lblProductName.Text = "Sapper";
			this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblVersion
			// 
			this.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblVersion.Location = new System.Drawing.Point(6, 25);
			this.lblVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.lblVersion.MaximumSize = new System.Drawing.Size(0, 17);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(124, 17);
			this.lblVersion.TabIndex = 0;
			this.lblVersion.Text = "Version";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblCopyright
			// 
			this.lblCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblCopyright.Location = new System.Drawing.Point(6, 50);
			this.lblCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.lblCopyright.MaximumSize = new System.Drawing.Size(0, 17);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(124, 17);
			this.lblCopyright.TabIndex = 21;
			this.lblCopyright.Text = "© 2010 Poltavets Pavel.";
			this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// linkEMail
			// 
			this.linkEMail.AutoSize = true;
			this.linkEMail.Location = new System.Drawing.Point(6, 75);
			this.linkEMail.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.linkEMail.Name = "linkEMail";
			this.linkEMail.Size = new System.Drawing.Size(120, 13);
			this.linkEMail.TabIndex = 26;
			this.linkEMail.TabStop = true;
			this.linkEMail.Text = "poltavets-pavel@mail.ru";
			this.linkEMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkEMail_LinkClicked);
			// 
			// okButton
			// 
			this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(29, 117);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 25;
			this.okButton.Text = "&OK";
			// 
			// AboutBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(151, 161);
			this.Controls.Add(this.tblLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutBox";
			this.Padding = new System.Windows.Forms.Padding(9);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.tblLayoutPanel.ResumeLayout(false);
			this.tblLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tblLayoutPanel;
		private System.Windows.Forms.LinkLabel linkEMail;
		private System.Windows.Forms.Label lblProductName;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblCopyright;
		private System.Windows.Forms.Button okButton;		
	}
}