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
using System.Collections.Generic;
using Sapper.Types;

namespace Sapper.Kernel {
	#region Help
	/// <summary>
	/// Represents a field with cells.
	/// </summary>	
	#endregion Help
	public abstract class Field<TCell> : IField where TCell : ICell, new() {
		private CellsCollection<TCell> collectionCells;
		private ICell cellSelected;
		private int countErrors;
		private int countLabels;
		private int countOpenedCells;
		private bool frozen;
		private EventHandler<FieldMessageEventArgs> messageDelegate;
		private void CheckCells() {
			IsFrozen = countOpenedCells == (CellCount - MineCount + countErrors);
			if(IsFrozen) {
				if(countLabels == 0) {
					SendFieldMessage(FieldMessage.AllMinesLabeled);
				} else {
					foreach(ICell cell in Cells)
						if(cell.MarkLabel()) {
							countLabels--;
						}
					SendFieldMessage(FieldMessage.ChangeLabelsCount);
					SendFieldMessage(FieldMessage.AllEmptyCellsOpened);
				}
			}
		}
		private void ShowAllMines() {
			IsFrozen = true;
			foreach(ICell cellMine in Cells) {
				cellMine.ShowMine();
			}
			SendFieldMessage(FieldMessage.AllMinesDetonated);
		}
		private void SetMines(int count, int minesNumber) {
			int i = 0, index = 0;
			Random randomObj = new Random();
			while(i < minesNumber) {
				index = randomObj.Next(count);
				if(!Cells[index].HasMine) {
					Cells[index].HasMine = true;
					i++;
				}
			}
		}
		protected CellsCollection<TCell> Cells { get { return collectionCells; } }
		protected abstract void InitializeCells(int count);
		protected abstract void SetFieldSettings(FieldSettings settings);
		protected abstract void SetLinks(int count);
		protected abstract void SetVisualParameters();
		protected bool IsFrozen { get { return frozen; } set { frozen = value; } }
		protected virtual void OnFieldMessage(FieldMessageEventArgs e) {
			if(messageDelegate != null) {
				messageDelegate(this, e);
			}
		}
		protected void SendFieldMessage(FieldMessage fieldMessage) {
			OnFieldMessage(new FieldMessageEventArgs(fieldMessage, LabelCount, cellSelected != null));
		}
		public void Initialize(FieldSettings settings) {
			SetFieldSettings(settings);
			collectionCells = new CellsCollection<TCell>();
			countLabels = MineCount;
			int count = CellCount;
			InitializeCells(count);
			SetMines(count, countLabels);
			SetLinks(count);
			SetVisualParameters();
			SendFieldMessage(FieldMessage.Initialized);
			SendFieldMessage(FieldMessage.RegenerationNeeded);
		}
		public void NextStateCell(ICell cell) {
			if(!IsFrozen && cell != null && cell.Closed) {
				int inc = cell.SetNextState();
				if(inc != 0) {
					countLabels += inc;
					SendFieldMessage(FieldMessage.ChangeLabelsCount);
				}
				CheckCells();
			}
		}
		public void OpenCell(ICell cell, bool isWithAround) {
			SelectCell(null, isWithAround);
			if(IsFrozen || cell == null) 
				return;
			CellOperationOpening opening = cell.Open(isWithAround);
			countOpenedCells += opening.OpenedCount;
			if(opening.HasMines) {
				countErrors += opening.MineCount;
				SendFieldMessage(FieldMessage.MinesDetonated);
				countLabels -= opening.MineCount;
				SendFieldMessage(FieldMessage.ChangeLabelsCount);
			}
			if(countErrors > MaxErrorCount) {
				ShowAllMines();
			} else {
				CheckCells();
			}
		}
		public void SelectWithCheck(ICell cell, bool selected, bool isWithAround) {
			if(cell != null) {
				if(isWithAround) {
					cell.IsSelectedAround = selected;
				} else {
					cell.IsSelected = selected;
				}
			}
		}
		public void SelectCell(ICell cell, bool isWithAround) {
			if((cellSelected != cell) || (isWithAround && (cell != null) && !cell.IsSelectedAround)) {
				SelectWithCheck(cellSelected, false, isWithAround);
				cellSelected = cell;
				SelectWithCheck(cellSelected, true, isWithAround);
				SendFieldMessage(FieldMessage.SelectCell);
			}
		}
		public virtual int MaxErrorCount { get { return 0; } }
		public abstract int MineCount { get; }
		public abstract int CellCount { get; }
		public int LabelCount { get { return countLabels; } }
		public event EventHandler<FieldMessageEventArgs> Message {
			add { messageDelegate = (EventHandler<FieldMessageEventArgs>)Delegate.Combine(messageDelegate, value); }
			remove { messageDelegate = (EventHandler<FieldMessageEventArgs>)Delegate.Remove(messageDelegate, value); }
		}
		public void ClearMessage() {
			messageDelegate = (EventHandler<FieldMessageEventArgs>)Delegate.RemoveAll(messageDelegate, messageDelegate);
		}
	}

	public abstract class VisualField<TCell, TVertex, TBox> : Field<TCell>, IVisualField<TVertex, TBox>
		where TCell : IVisualCell<TVertex, TBox>, new()
		where TVertex : struct
		where TBox : struct {
		private VisualFieldSettings<TBox> settingsField;
		protected VisualFieldSettings<TBox> Settings { get { return settingsField; } }
		protected VisualField()
			: base() {
		}
		protected override void InitializeCells(int count) {
			for(int i = 0; i < count; i++) {
				Cells.Add(new TCell());
			}
		}
		protected override void SetFieldSettings(FieldSettings settings) {
			settingsField = settings.Clone() as VisualFieldSettings<TBox>;
		}
		protected override void SetLinks(int count) {
			return;// do nothing
		}
		protected override void SetVisualParameters() {
			return;// do nothing
		}
		public virtual void ChangeBox(TBox box) {
			Settings.BoundsBox = box;
			SetVisualParameters();
			SendFieldMessage(FieldMessage.RegenerationNeeded);
		}
		public override int MaxErrorCount { get { return Settings.ErrorCount; } }
		public override int MineCount { get { return Settings.MineCount; } }
		public override int CellCount { get { return 0; } }
		public void Draw(IPainter<TVertex, TBox> painter, ISelector<TVertex, TBox> selector) {
			if(painter == null) return;
			if(IsFrozen) {
				selector = null;
			} else if(selector != null) {
				selector.Initialize(Settings.BoundsBox);
			}
			painter.BeginPaint(Settings.BoundsBox);
			foreach(TCell visualCell in Cells) {
				visualCell.Draw(painter, selector);
			}
			painter.EndPaint();
		}
	}

	public abstract class TabularField<TCell, TVertex, TBox> : VisualField<TCell, TVertex, TBox>
		where TCell : IVisualCell<TVertex, TBox>, new()
		where TVertex : struct
		where TBox : struct {
		protected TabularField()
			: base() {
		}
		protected int RowCount { get { return Settings.RowCount; } }
		protected int ColumnCount { get { return Settings.ColumnCount; } }
		protected bool AddLinkToCell(int indexCell, int indexLink, int countLimit, int currentRow, bool useRowChecking) {
			bool hasAdded = (indexCell != indexLink) && (indexLink >= 0) && (indexLink < countLimit);
			if(useRowChecking) {
				hasAdded = hasAdded && (indexLink / ColumnCount == currentRow);
			}
			if(hasAdded) {
				Cells[indexCell].AddAroundCell(Cells[indexLink]);
			}
			return hasAdded;
		}
		#region Help
		/// <summary>
		/// Set links between the cells by next linking sheme:
		///      i/cols-1 ->   | i-cols-1 | i-cols | i-cols+1 |
		///        i/cols ->   |   i-1    |   i    |   i+1    |
		///      i/cols+1 ->   | i+cols-1 | i+cols | i+cols+1 |
		/// use row-checking for each cell with "i"-index.
		/// </summary>        
		#endregion Help
		protected override void SetLinks(int count) {
			int i, j, k, index, row;
			for(i = 0; i < count; i++) {
				index = i - ColumnCount - 1;
				row = i / ColumnCount - 1;
				for(j = 0; j < 3; j++) {
					for(k = 0; k < 3; k++) {
						AddLinkToCell(i, index + k, count, row, true);
					}
					index += ColumnCount;
					row += 1;
				}
			}
		}
		public override int CellCount { get { return RowCount * ColumnCount; } }
	}
}
