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
using System.Windows.Forms;
using Sapper.Types;
using Sapper.WinSapper;

namespace Sapper.WinUI {
	public partial class SettingsForm : Form {
		private int fieldIndex = -1;
		private TabularFieldSettings fieldSettings;
		private void InitializeControlsFromFieldSettings() {
			if(fieldSettings == null) return;
			nmrHeight.Value = fieldSettings.RowCount;
			nmrWidth.Value = fieldSettings.ColumnCount;
			nmrNumMines.Value = fieldSettings.MineCount;
			nmrNumErrors.Value = fieldSettings.ErrorCount;
			chkFlatView.Checked = !fieldSettings.UseEffect3D;
		}
		private void InitializeFieldSettingsFromControls() {
			if(fieldSettings == null) return;
			fieldSettings.RowCount = Convert.ToInt32(nmrHeight.Value);
			fieldSettings.ColumnCount = Convert.ToInt32(nmrWidth.Value);
			fieldSettings.MineCount = Convert.ToInt32(nmrNumMines.Value);
			fieldSettings.ErrorCount = Convert.ToInt32(nmrNumErrors.Value);
			fieldSettings.UseEffect3D = !chkFlatView.Checked;
		}
		private void InitializeUI() {
			//
			// nmrHeight
			//
			nmrHeight.Minimum = FieldSettings.MinimumRowCount;
			nmrHeight.Maximum = FieldSettings.MaximumRowCount;
			//
			// nmrWidth
			//
			nmrWidth.Minimum = FieldSettings.MinimumColumnCount;
			nmrWidth.Maximum = FieldSettings.MaximumColumnCount;
			//
			// nmrNumMines
			//
			nmrNumMines.Minimum = FieldSettings.MinimumMineCount;
			nmrNumMines.Maximum = FieldSettings.MaximumMineCount;
			//
			// nmrNumErrors
			//
			nmrNumErrors.Minimum = 0;
			nmrNumErrors.Maximum = FieldSettings.MaximumErrorCount;
			//
			// cmbFieldType
			//
			for(int i = 0; i < FieldManager.FieldCount; i++)
				cmbFieldType.Items.Add(FieldManager.GetFieldName(i));
			cmbFieldType.SelectedIndex = 0;// set field settings
		}
		private bool IsChanged {
			get {
				if(fieldSettings == null)
					return false;
				return !(fieldSettings.RowCount == Convert.ToInt32(nmrHeight.Value) &&
					fieldSettings.ColumnCount == Convert.ToInt32(nmrWidth.Value) &&
					fieldSettings.MineCount == Convert.ToInt32(nmrNumMines.Value) &&
					fieldSettings.ErrorCount == Convert.ToInt32(nmrNumErrors.Value) &&
					fieldSettings.UseEffect3D == !chkFlatView.Checked);
			}
		}
		private void cmbFieldType_SelectedIndexChanged(object sender, EventArgs e) {
			if(fieldIndex == cmbFieldType.SelectedIndex) return;
			if(IsChanged) {
				string dlgMessage = String.Format(System.Globalization.CultureInfo.CurrentCulture, "The field \"{0}\" has been changed.\n\rSave changes?", FieldManager.GetFieldName(fieldIndex));
				DialogResult dlgResult = MessageBox.Show(dlgMessage, "Field change", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
				switch(dlgResult) {
					case DialogResult.Yes:
						InitializeFieldSettingsFromControls();
						break;
					case DialogResult.Cancel:
						cmbFieldType.SelectedIndex = fieldIndex;
						return;
				}
			}
			fieldSettings = FieldManager.GetFieldSettings(cmbFieldType.SelectedIndex);
			InitializeControlsFromFieldSettings();
			fieldIndex = cmbFieldType.SelectedIndex;
		}
		private void nmrNumMines_ValueChanged(object sender, EventArgs e) {
			if(Convert.ToInt32(nmrNumMines.Value) <= Convert.ToInt32(nmrNumErrors.Value)) {
				nmrNumMines.Value = nmrNumErrors.Value + 1;
			}
		}
		private void nmrNumErrors_ValueChanged(object sender, EventArgs e) {
			if(Convert.ToInt32(nmrNumErrors.Value) >= Convert.ToInt32(nmrNumMines.Value)) {
				nmrNumErrors.Value = nmrNumMines.Value - 1;
			}
		}
		public SettingsForm() {
			InitializeComponent();
			InitializeUI();
		}
		public bool ShowForm(IWin32Window owner) {
			DialogResult dlgResult = ShowDialog(owner);
			switch(dlgResult) {
				case DialogResult.OK:
					InitializeFieldSettingsFromControls();
					break;
				default:// DialogResult.Cancel:
					InitializeControlsFromFieldSettings();
					break;
			}
			return dlgResult == DialogResult.OK;
		}
		public TabularFieldSettings GetFieldSettings(Control control) {
			if(fieldSettings != null) {
				fieldSettings.BoundsBox = Geometric.GetOptimalFieldSize(control.Size);
			}
			return fieldSettings;
		}
		public Type FieldType { get { return FieldManager.GetFieldType(cmbFieldType.SelectedIndex); } }
	}
}
