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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sapper.Types;
using Sapper.WinSapper;

namespace Sapper.WinUI {
	public enum ImageStatus : int { Smile = 0, Warning = 1, Success = 2, Sadness = 3 }
	public partial class MainForm : Form {
		private FieldManager fieldManager;
		private SettingsForm settingsForm;
		private ImageList imgsGameStatus;
		private Timer timerGame;
		#region User interface initialization
		public MainForm() {
			InitializeComponent();
			InitializeUI();
		}
		private void InitializeUI() {
			BackColor = SystemColors.GradientInactiveCaption;
			mainMenu.BackColor = this.BackColor;
			pnlGameInfo.BackColor = this.BackColor;
			btnNewGame.BackColor = this.BackColor;
			tbLabels.BackColor = SystemColors.InactiveCaptionText;
			tbTime.BackColor = tbLabels.BackColor;
			pnlGameField.BackColor = GDIPainter.BackColor;
			pnlGameField.Size = new Size(305, 305);
			if(components == null) {
				components = new System.ComponentModel.Container();
			}
			imgsGameStatus = new ImageList(components);
			imgsGameStatus.ImageSize = new Size(32, 32);
			imgsGameStatus.TransparentColor = Color.White;
			imgsGameStatus.Images.Add(Sapper.Properties.Resources.ImageSmile);
			imgsGameStatus.Images.Add(Sapper.Properties.Resources.ImageWarning);
			imgsGameStatus.Images.Add(Sapper.Properties.Resources.ImageSuccess);
			imgsGameStatus.Images.Add(Sapper.Properties.Resources.ImageSadness);
			btnNewGame.ImageList = imgsGameStatus;
		}
		#endregion User interface initialization
		#region Menu Items
		private void miAbout_Click(object sender, EventArgs e) {
			AboutBox about = new AboutBox();
			if(about.ShowDialog(this) == DialogResult.OK) {
				about.Close();
			}
		}
		private void miNew_Click(object sender, EventArgs e) {
			NewField();
		}
		private void miSettings_Click(object sender, EventArgs e) {
			if(settingsForm.ShowForm(this)) {
				NewField();
			}
		}
		private void miExit_Click(object sender, EventArgs e) {
			Close();
		}
		#endregion Menu Items
		#region Game operations
		private void FieldMessageEventHandler(object sender, FieldMessageEventArgs e) {
			switch(e.Message) {
				case FieldMessage.AllMinesDetonated:
					btnNewGame.ImageIndex = (int)ImageStatus.Sadness;
					break;
				case FieldMessage.AllMinesLabeled:
				case FieldMessage.AllEmptyCellsOpened:
					btnNewGame.ImageIndex = (int)ImageStatus.Success;
					break;
				case FieldMessage.SelectCell:
					btnNewGame.ImageIndex = (int)(e.HasSelectedCell ? ImageStatus.Warning : ImageStatus.Smile);
					return;
				case FieldMessage.Initialized:
					btnNewGame.ImageIndex = (int)ImageStatus.Smile;
					timerGame.Start();
					goto case FieldMessage.ChangeLabelsCount;
				case FieldMessage.ChangeLabelsCount:
					tbLabels.Text = Convert.ToString(e.LabelsCount, System.Globalization.CultureInfo.CurrentCulture);
					return;
				default:
					return;
			}
			timerGame.Stop();
		}
		private void NewField() {
			tbTime.Text = "0";
			fieldManager.ChangeFieldType(settingsForm.GetFieldSettings(pnlGameField), settingsForm.FieldType, new EventHandler<Sapper.Types.FieldMessageEventArgs>(FieldMessageEventHandler));
		}
		private void timerGame_Tick(object sender, EventArgs e) {
			int seconds = Convert.ToInt32(tbTime.Text, System.Globalization.CultureInfo.CurrentCulture);
			tbTime.Text = Convert.ToString(seconds + 1, System.Globalization.CultureInfo.CurrentCulture);
		}
		private void MainForm_Load(object sender, EventArgs e) {
			timerGame = new Timer();
			timerGame.Interval = 1000;
			timerGame.Tick += new EventHandler(timerGame_Tick);
			settingsForm = new SettingsForm();
			fieldManager = new FieldManager(pnlGameField.CreateGraphics());
			NewField();
			pnlGameField.MouseDown += new MouseEventHandler(fieldManager.MouseDownEventHandler);
			pnlGameField.MouseMove += new MouseEventHandler(fieldManager.MouseMoveEventHandler);
			pnlGameField.MouseUp += new MouseEventHandler(fieldManager.MouseUpEventHandler);
			pnlGameField.Paint += new PaintEventHandler(fieldManager.PaintEventHandler);
		}
		private void pnlGameField_SizeChanged(object sender, EventArgs e) {
			fieldManager.ChangeFieldSize(pnlGameField.CreateGraphics(), Geometric.GetOptimalFieldSize(pnlGameField.Size));
		}
		#endregion Game operations
	}
}