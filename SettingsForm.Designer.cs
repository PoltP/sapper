#region Copyright (c) 2010, Pavel Poltavets
/*
{*********************************************************}
{                                                         }
{       Sapper .NET application                            }
{                                                         }
{       Copyright (c) 2010, Pavel Poltavets               }
{       poltavets-pavel@mail.ru                           }
{                                                         }
{       ALL RIGHTS RESERVED                               }
{                                                         }
{*********************************************************}
*/
#endregion Copyright (c) 2010, Pavel Poltavets

namespace Sapper.WinUI {
	partial class SettingsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblFieldType = new System.Windows.Forms.Label();
			this.lblHeight = new System.Windows.Forms.Label();
			this.lblWidth = new System.Windows.Forms.Label();
			this.cmbFieldType = new System.Windows.Forms.ComboBox();
			this.lblNumMines = new System.Windows.Forms.Label();
			this.nmrHeight = new System.Windows.Forms.NumericUpDown();
			this.nmrWidth = new System.Windows.Forms.NumericUpDown();
			this.nmrNumMines = new System.Windows.Forms.NumericUpDown();
			this.chkFlatView = new System.Windows.Forms.CheckBox();
			this.nmrNumErrors = new System.Windows.Forms.NumericUpDown();
			this.lblNumErrors = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nmrHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nmrWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nmrNumMines)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nmrNumErrors)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(82, 228);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(74, 24);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(162, 228);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(74, 24);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblFieldType
			// 
			this.lblFieldType.AutoSize = true;
			this.lblFieldType.Location = new System.Drawing.Point(8, 24);
			this.lblFieldType.Name = "lblFieldType";
			this.lblFieldType.Size = new System.Drawing.Size(55, 13);
			this.lblFieldType.TabIndex = 2;
			this.lblFieldType.Text = "Field type:";
			// 
			// lblHeight
			// 
			this.lblHeight.AutoSize = true;
			this.lblHeight.Location = new System.Drawing.Point(8, 60);
			this.lblHeight.Name = "lblHeight";
			this.lblHeight.Size = new System.Drawing.Size(41, 13);
			this.lblHeight.TabIndex = 3;
			this.lblHeight.Text = "Height:";
			// 
			// lblWidth
			// 
			this.lblWidth.AutoSize = true;
			this.lblWidth.Location = new System.Drawing.Point(8, 94);
			this.lblWidth.Name = "lblWidth";
			this.lblWidth.Size = new System.Drawing.Size(38, 13);
			this.lblWidth.TabIndex = 4;
			this.lblWidth.Text = "Width:";
			// 
			// cmbFieldType
			// 
			this.cmbFieldType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFieldType.Location = new System.Drawing.Point(72, 21);
			this.cmbFieldType.Name = "cmbFieldType";
			this.cmbFieldType.Size = new System.Drawing.Size(164, 21);
			this.cmbFieldType.TabIndex = 5;
			this.cmbFieldType.SelectedIndexChanged += new System.EventHandler(this.cmbFieldType_SelectedIndexChanged);
			// 
			// lblNumMines
			// 
			this.lblNumMines.AutoSize = true;
			this.lblNumMines.Location = new System.Drawing.Point(8, 128);
			this.lblNumMines.Name = "lblNumMines";
			this.lblNumMines.Size = new System.Drawing.Size(38, 13);
			this.lblNumMines.TabIndex = 6;
			this.lblNumMines.Text = "Mines:";
			// 
			// nmrHeight
			// 
			this.nmrHeight.Location = new System.Drawing.Point(165, 58);
			this.nmrHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nmrHeight.Name = "nmrHeight";
			this.nmrHeight.Size = new System.Drawing.Size(71, 20);
			this.nmrHeight.TabIndex = 7;
			// 
			// nmrWidth
			// 
			this.nmrWidth.Location = new System.Drawing.Point(165, 92);
			this.nmrWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nmrWidth.Name = "nmrWidth";
			this.nmrWidth.Size = new System.Drawing.Size(71, 20);
			this.nmrWidth.TabIndex = 8;
			// 
			// nmrNumMines
			// 
			this.nmrNumMines.Location = new System.Drawing.Point(165, 126);
			this.nmrNumMines.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nmrNumMines.Name = "nmrNumMines";
			this.nmrNumMines.Size = new System.Drawing.Size(71, 20);
			this.nmrNumMines.TabIndex = 9;
			this.nmrNumMines.ValueChanged += new System.EventHandler(this.nmrNumMines_ValueChanged);
			// 
			// chkFlatView
			// 
			this.chkFlatView.AutoSize = true;
			this.chkFlatView.Location = new System.Drawing.Point(11, 194);
			this.chkFlatView.Name = "chkFlatView";
			this.chkFlatView.Size = new System.Drawing.Size(68, 17);
			this.chkFlatView.TabIndex = 10;
			this.chkFlatView.Text = "Flat view";
			this.chkFlatView.UseVisualStyleBackColor = true;
			// 
			// nmrNumErrors
			// 
			this.nmrNumErrors.Location = new System.Drawing.Point(165, 162);
			this.nmrNumErrors.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nmrNumErrors.Name = "nmrNumErrors";
			this.nmrNumErrors.Size = new System.Drawing.Size(71, 20);
			this.nmrNumErrors.TabIndex = 12;
			this.nmrNumErrors.ValueChanged += new System.EventHandler(this.nmrNumErrors_ValueChanged);
			// 
			// lblNumErrors
			// 
			this.lblNumErrors.AutoSize = true;
			this.lblNumErrors.Location = new System.Drawing.Point(8, 164);
			this.lblNumErrors.Name = "lblNumErrors";
			this.lblNumErrors.Size = new System.Drawing.Size(37, 13);
			this.lblNumErrors.TabIndex = 11;
			this.lblNumErrors.Text = "Errors:";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(248, 264);
			this.Controls.Add(this.nmrNumErrors);
			this.Controls.Add(this.lblNumErrors);
			this.Controls.Add(this.chkFlatView);
			this.Controls.Add(this.nmrNumMines);
			this.Controls.Add(this.nmrWidth);
			this.Controls.Add(this.nmrHeight);
			this.Controls.Add(this.lblNumMines);
			this.Controls.Add(this.cmbFieldType);
			this.Controls.Add(this.lblWidth);
			this.Controls.Add(this.lblHeight);
			this.Controls.Add(this.lblFieldType);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			((System.ComponentModel.ISupportInitialize)(this.nmrHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nmrWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nmrNumMines)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nmrNumErrors)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblFieldType;
		private System.Windows.Forms.Label lblHeight;
		private System.Windows.Forms.Label lblWidth;
		private System.Windows.Forms.ComboBox cmbFieldType;
		private System.Windows.Forms.Label lblNumMines;
		private System.Windows.Forms.NumericUpDown nmrHeight;
		private System.Windows.Forms.NumericUpDown nmrWidth;
		private System.Windows.Forms.NumericUpDown nmrNumMines;
		private System.Windows.Forms.CheckBox chkFlatView;
		private System.Windows.Forms.NumericUpDown nmrNumErrors;
		private System.Windows.Forms.Label lblNumErrors;
	}
}
