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
	/// Represents a cell in the field of mines.
	/// </summary>
	/// <remarks>A cell in the field of mines is specified by state and mode.</remarks>
	#endregion Help
	public abstract class Cell : ICell {
		protected enum CellState { Normal, Label, Undefined }
		[FlagsAttribute]
		private enum CellModes {
			None = 0,
			Closed = 0x1,
			HasMine = 0x2,
			Selected = 0x4,
			SelectedAround = 0x8,
			DetonatedMine = 0x10,
			Unused01 = 0x20,
			Unused02 = 0x40,
			HasVisited = 0x80
		}
		private static CellState DefaultCellState = CellState.Normal;
		private int minesNumber;
		private CellsCollection<ICell> aroundCells;		
		private CellModes mode;
		private CellState state;
		private void DepthFirstSearch(ICell currentCell, ref int counter) {
			if(currentCell.ChangeState()) {
				counter++;
			}
			if(currentCell.MinesNumber > 0 || currentCell.HasMine)
				return;
			currentCell.HasVisited = true;
			foreach(ICell cell in currentCell.CellsAround)
				if(!cell.HasVisited) {
					DepthFirstSearch(cell, ref counter);
				}
		}
		private bool GetMode(CellModes cellMode) {
			return (mode & cellMode) != 0;
		}
		private void SetMode(CellModes cellMode, bool value) {
			mode = value ? (mode | cellMode) : (mode & ~cellMode);
		}
		private CellState GetNextState() {
			CellState cellState = state + 1;
			if(!Enum.IsDefined(typeof(CellState), cellState))
				cellState = DefaultCellState;
			return cellState;
		}
		private bool CheckAround() {
			int count = 0;
			foreach(Cell cell in aroundCells) {
				if(cell.IsLabelState || cell.IsMineDetonated) {
					count++;
				}
			}
			return count == MinesNumber;
		}
		protected Cell() {
			Closed = true;
			state = DefaultCellState;
			aroundCells = new CellsCollection<ICell>();
		}
		protected abstract void Update();
		#region Help
		/// <summary>
		/// Gets <see cref="Sapper.CellState">state</see> of the cell.
		/// </summary>
		#endregion
		protected CellState State { get { return state; } }
		protected abstract bool UseEffect3D { get; }

		public void AddAroundCell(ICell cell) {
			if(cell.HasMine) {
				minesNumber++;
			}
			aroundCells.Add(cell);
		}
		#region Help
		/// <summary>
		/// Open the cell if his state is not <see cref="Sapper.CellState">Label</see>
		/// </summary>
		#endregion
		public bool ChangeState() {
			bool isChanged = CanOpened;
			if(isChanged) {
				state = CellState.Normal;
				Closed = false;
				Update();
			}
			return isChanged;
		}
		public bool MarkLabel() {
			bool notLabelState = Closed && !IsLabelState;
			if(notLabelState) {
				state = CellState.Label;
				Update();
			}
			return notLabelState;
		}
		#region Help
		/// <summary>
		/// Opens a <see cref="Sapper.Cell">cell</see> with empty cells and arounding cells if checked.
		/// </summary>
		/// <returns>
		/// Returns <see cref="Sapper.CellOpenResults">struct</see> with two integers:
		///		<see cref="Sapper.CellOpenResults.MineCount">count of the opened mines</see>;
		///		<see cref="Sapper.CellOpenResults.OpenedCount">count of the opened cells</see>.
		/// </returns>
		#endregion Help
		public CellOperationOpening Open(bool isWithAround) {
			CellOperationOpening opening = new CellOperationOpening();
			if(isWithAround) {
				if(!Closed && !IsMineDetonated && CheckAround())
					foreach(Cell cell in aroundCells) {
						opening += cell.Open(false);
					}
			} else if(CanOpened) {
				int counter = 0;
				if(HasMine) {
					opening.MineCount = 1;
					IsMineDetonated = true;
				}
				DepthFirstSearch(this, ref counter);
				opening.OpenedCount = counter;
			}
			return opening;
		}
		#region Help
		/// <summary>
		/// Set a next <see cref="Sapper.Cell">Cell</see> <see cref="Sapper.Cell.State">state</see>.
		/// </summary>
		/// <returns>
		/// A integer value which indicates the changed <see cref="Sapper.CellState">Label</see> state:
		///		-1 - current <see cref="Sapper.CellState">state</see> is <see cref="Sapper.CellState">Label</see>,
		///		 0 - not <see cref="Sapper.CellState">Label</see> state changed,
		///		+1 - previous <see cref="Sapper.CellState">state</see> is <see cref="Sapper.CellState">Label</see>.
		/// </returns>
		#endregion Help
		public int SetNextState() {
			int result = 0;
			if(IsLabelState)
				result = +1;
			state = GetNextState();
			if(IsLabelState)
				result = -1;
			Update();
			return result;
		}
		public void ShowMine() {
			if(Closed && ((HasMine && !IsLabelState) || (!HasMine && IsLabelState))) {
				Closed = false;
				Update();
			}
		}
		public int MinesNumber { get { return minesNumber; } }
		public bool CanOpened { get { return Closed && !IsLabelState; } }
		public CellsCollection<ICell> CellsAround {
			get { return aroundCells; }
		}
		public bool Closed {
			get { return GetMode(CellModes.Closed); }
			set { SetMode(CellModes.Closed, value); }
		}
		public bool HasMine {
			get { return GetMode(CellModes.HasMine); }
			set { SetMode(CellModes.HasMine, value); }
		}
		public bool HasVisited {
			get { return GetMode(CellModes.HasVisited); }
			set { SetMode(CellModes.HasVisited, value); }
		}
		public bool IsLabelState { get { return state == CellState.Label; } }
		public bool IsMineDetonated {
			get { return GetMode(CellModes.DetonatedMine); }
			set { SetMode(CellModes.DetonatedMine, value); }
		}
		public bool IsSelected {
			get { return GetMode(CellModes.Selected); }
			set {
				if(value != IsSelected && CanOpened) {
					SetMode(CellModes.Selected, value);
					Update();
				}
			}
		}
		public bool IsSelectedAround {
			get { return GetMode(CellModes.SelectedAround); }
			set {
				IsSelected = value;
				if(value != IsSelectedAround) {
					SetMode(CellModes.SelectedAround, value);// don't need update
					foreach(Cell cell in aroundCells) {
						cell.IsSelected = value;
					}
				}
			}
		}
	}

	#region Help
	/// <summary>
	/// Represents a visual cell in the field of mines.
	/// </summary>    
	#endregion Help
	public abstract class VisualCell<TVertex, TBox> : Cell, IVisualCell<TVertex, TBox>
		where TVertex : struct
		where TBox : struct {
		private const double InternalBoxCoefficient = 0.5d;
		private TBox boxBounds;
		private TVertex centerVertex;
		private IPainter<TVertex, TBox> lastPainter;
		private CellBackfillType GetBackFillType() {
			CellBackfillType fillType;
			if(IsMineDetonated) {
				fillType = CellBackfillType.Detonated;
			} else {
				fillType = (Closed && !IsSelected) ? CellBackfillType.Closed : CellBackfillType.Opened;
			}
			return fillType;
		}
		protected TVertex[] vertices;
		protected TVertex[] verticesEffect3D;
		internal abstract void InitializeVertices(CellOperationInitialize<TVertex, TBox> cellInit);
		protected abstract TBox GetInternalBox(double boxCoefficient);
		protected override sealed void Update() {
			if(lastPainter != null && lastPainter.IsValid) {
				Draw(lastPainter, null);
			}
		}
		protected TBox BoundsBox { get { return boxBounds; } set { boxBounds = value; } }
		protected TVertex Center { get { return centerVertex; } set { centerVertex = value; } }
		protected abstract int VertexCount { get; }
		protected override sealed bool UseEffect3D { get { return verticesEffect3D != null; } }
		protected VisualCell()
			: base() {
		}
		public void Draw(IPainter<TVertex, TBox> painter, ISelector<TVertex, TBox> selector) {
			// Set reference to external painter object (only for cell Update)
			lastPainter = painter;
			// Basic cell visual data
			painter.FillPolygon(vertices, GetBackFillType());
			if(UseEffect3D && Closed && !IsSelected) {
				painter.DrawBorder3D(verticesEffect3D); 
			}//else
			painter.DrawPolygon(vertices);
			// Adding this cell to selector if needed
			if(selector != null) {
				selector.AddCell(this, vertices);
			}
			// Other common cell visual data
			TVertex center = Center;
			TBox internalBox = GetInternalBox(InternalBoxCoefficient);
			if(!Closed) {
				if(HasMine) {
					if(!IsLabelState) {
						painter.DrawMine(center, internalBox, true);
					}
				} else {
					switch(State) {
						case CellState.Label:
							painter.DrawMine(center, internalBox, false);
							break;
						case CellState.Normal:
							if(MinesNumber != 0) {
								painter.DrawInteger(center, internalBox, MinesNumber);
							}
							break;
					}
				}
			} else {// closed
				switch(State) {
					case CellState.Undefined:
						painter.DrawUndefined(center, internalBox);
						break;
					case CellState.Label:
						painter.DrawLabel(center, internalBox);
						break;
				}
			}
		}
		public void Initialize(CellOperationInitialize<TVertex, TBox> cellInit) {
			vertices = new TVertex[VertexCount];
			if(cellInit.UseEffect3D) {
				verticesEffect3D = new TVertex[VertexCount];
			}
			BoundsBox = cellInit.ExtentsBox;
			InitializeVertices(cellInit);
		}
	}
}