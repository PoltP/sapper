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
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
				if(fieldManager != null) {
					fieldManager.Dispose();
					fieldManager = null;
				}
				if(timerGame != null) {
					timerGame.Stop();
					timerGame.Dispose();
					timerGame = null;
				}
				if(settingsForm != null) {
					settingsForm.Dispose();
					settingsForm = null;
				}				
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.tbTime = new System.Windows.Forms.TextBox();
			this.pnlGameField = new System.Windows.Forms.Panel();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.miGame = new System.Windows.Forms.ToolStripMenuItem();
			this.miNew = new System.Windows.Forms.ToolStripMenuItem();
			this.separatorNewSettings = new System.Windows.Forms.ToolStripSeparator();
			this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.miExit = new System.Windows.Forms.ToolStripMenuItem();
			this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlGameInfo = new System.Windows.Forms.Panel();
			this.tbLabels = new System.Windows.Forms.TextBox();
			this.btnNewGame = new System.Windows.Forms.Button();
			this.mainMenu.SuspendLayout();
			this.pnlGameInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbTime
			// 
			this.tbTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbTime.Cursor = System.Windows.Forms.Cursors.Default;
			this.tbTime.Enabled = false;
			this.tbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
			this.tbTime.Location = new System.Drawing.Point(254, 8);
			this.tbTime.Name = "tbTime";
			this.tbTime.Size = new System.Drawing.Size(48, 26);
			this.tbTime.TabIndex = 2;
			this.tbTime.TabStop = false;
			this.tbTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// pnlGameField
			// 
			this.pnlGameField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlGameField.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlGameField.Location = new System.Drawing.Point(5, 70);
			this.pnlGameField.Margin = new System.Windows.Forms.Padding(0);
			this.pnlGameField.Name = "pnlGameField";
			this.pnlGameField.Size = new System.Drawing.Size(305, 305);
			this.pnlGameField.TabIndex = 0;
			this.pnlGameField.SizeChanged += new System.EventHandler(this.pnlGameField_SizeChanged);
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGame,
            this.miHelp});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(316, 24);
			this.mainMenu.TabIndex = 1;
			// 
			// miGame
			// 
			this.miGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNew,
            this.separatorNewSettings,
            this.miSettings,
            this.miExit});
			this.miGame.Name = "miGame";
			this.miGame.Size = new System.Drawing.Size(46, 20);
			this.miGame.Text = "Game";
			// 
			// miNew
			// 
			this.miNew.Name = "miNew";
			this.miNew.ShortcutKeyDisplayString = "";
			this.miNew.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.miNew.Size = new System.Drawing.Size(141, 22);
			this.miNew.Text = "New";
			this.miNew.Click += new System.EventHandler(this.miNew_Click);
			// 
			// separatorNewSettings
			// 
			this.separatorNewSettings.Name = "separatorNewSettings";
			this.separatorNewSettings.Size = new System.Drawing.Size(138, 6);
			// 
			// miSettings
			// 
			this.miSettings.Name = "miSettings";
			this.miSettings.Size = new System.Drawing.Size(141, 22);
			this.miSettings.Text = "Settings";
			this.miSettings.Click += new System.EventHandler(this.miSettings_Click);
			// 
			// miExit
			// 
			this.miExit.Name = "miExit";
			this.miExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.miExit.Size = new System.Drawing.Size(141, 22);
			this.miExit.Text = "Exit";
			this.miExit.Click += new System.EventHandler(this.miExit_Click);
			// 
			// miHelp
			// 
			this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.miAbout});
			this.miHelp.Name = "miHelp";
			this.miHelp.Size = new System.Drawing.Size(40, 20);
			this.miHelp.Text = "Help";
			// 
			// miAbout
			// 
			this.miAbout.Name = "miAbout";
			this.miAbout.Size = new System.Drawing.Size(114, 22);
			this.miAbout.Text = "About";
			this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
			// 
			// pnlGameInfo
			// 
			this.pnlGameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlGameInfo.Controls.Add(this.tbTime);
			this.pnlGameInfo.Controls.Add(this.tbLabels);
			this.pnlGameInfo.Controls.Add(this.btnNewGame);
			this.pnlGameInfo.Location = new System.Drawing.Point(3, 24);
			this.pnlGameInfo.Name = "pnlGameInfo";
			this.pnlGameInfo.Size = new System.Drawing.Size(312, 40);
			this.pnlGameInfo.TabIndex = 2;
			// 
			// tbLabels
			// 
			this.tbLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.tbLabels.Cursor = System.Windows.Forms.Cursors.Default;
			this.tbLabels.Enabled = false;
			this.tbLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
			this.tbLabels.Location = new System.Drawing.Point(8, 8);
			this.tbLabels.Name = "tbLabels";
			this.tbLabels.Size = new System.Drawing.Size(48, 26);
			this.tbLabels.TabIndex = 1;
			this.tbLabels.TabStop = false;
			this.tbLabels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnNewGame
			// 
			this.btnNewGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.btnNewGame.Location = new System.Drawing.Point(135, 0);
			this.btnNewGame.Name = "btnNewGame";
			this.btnNewGame.Size = new System.Drawing.Size(40, 40);
			this.btnNewGame.TabIndex = 0;
			this.btnNewGame.UseVisualStyleBackColor = true;
			this.btnNewGame.Click += new System.EventHandler(this.miNew_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(316, 384);
			this.Controls.Add(this.pnlGameInfo);
			this.Controls.Add(this.pnlGameField);
			this.Controls.Add(this.mainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(164, 244);
			this.Name = "MainForm";
			this.Text = "Sapper";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.pnlGameInfo.ResumeLayout(false);
			this.pnlGameInfo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlGameField;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem miGame;
		private System.Windows.Forms.ToolStripMenuItem miNew;
		private System.Windows.Forms.ToolStripMenuItem miExit;
		private System.Windows.Forms.ToolStripMenuItem miHelp;
		private System.Windows.Forms.ToolStripMenuItem miAbout;
		private System.Windows.Forms.ToolStripSeparator separatorNewSettings;
		private System.Windows.Forms.ToolStripMenuItem miSettings;
		private System.Windows.Forms.Panel pnlGameInfo;
		private System.Windows.Forms.Button btnNewGame;
		private System.Windows.Forms.TextBox tbTime;
		private System.Windows.Forms.TextBox tbLabels;
	}
}
