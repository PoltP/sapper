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
using System.Collections.ObjectModel;

namespace Sapper.Types {
	public class FieldMessageEventArgs : EventArgs {
		private bool hasCellSelected;
		private int countLabels;
		private FieldMessage messageField;
		public FieldMessageEventArgs(FieldMessage fieldMessage) {
			this.messageField = fieldMessage;
		}
		public FieldMessageEventArgs(FieldMessage fieldMessage, int labelsCount, bool hasSelectedCell)
			: this(fieldMessage) {
			this.countLabels = labelsCount;
			this.hasCellSelected = hasSelectedCell;
		}
		public bool HasSelectedCell { get { return hasCellSelected; } }
		public int LabelsCount { get { return countLabels; } }
		public FieldMessage Message { get { return messageField; } }
	}

	public class FieldSettings : ICloneable {
		[FlagsAttribute]
		private enum FieldOptions {
			None = 0,
			UseEffect3D = 0x1,
			HasCenter = 0x2,
			Unused01 = 0x4,
			Unused02 = 0x8,
			Unused03 = 0x10
		}
		public static readonly float MinesMaxPercent = 0.8f;
		public static readonly int MaximumColumnCount = 50;
		public static readonly int MaximumRowCount = MaximumColumnCount;
		public static readonly int MaximumMineCount = (int)(MaximumColumnCount * MaximumRowCount * MinesMaxPercent);
		public static readonly int MaximumErrorCount = MaximumMineCount - 1;
		public static readonly int MinimumMineCount = 1;
		public static readonly int MinimumColumnCount = 3;
		public static readonly int MinimumRowCount = MinimumColumnCount;
		public static readonly int DefaultMineCount = 8;
		public static readonly int DefaultColumnCount = 9;
		public static readonly int DefaultHeight = 16 * MinimumRowCount;
		public static readonly int DefaultRowCount = DefaultColumnCount;
		public static readonly int DefaultWidth = 16 * MinimumColumnCount;
		private int countMines = DefaultMineCount;
		private int countError;
		private int countColumns = DefaultColumnCount;
		private FieldOptions options;
		private int countRows = DefaultRowCount;
		private bool GetOption(FieldOptions option) {
			return (options & option) != 0;
		}
		private void SetOption(FieldOptions option, bool value) {
			options = value ? (options | option) : (options & ~option);
		}
		public FieldSettings() {
			options = FieldOptions.HasCenter | FieldOptions.UseEffect3D;
		}
		public virtual Object Clone() {
			FieldSettings fieldSettings = Activator.CreateInstance(this.GetType()) as FieldSettings;
			fieldSettings.countColumns = countColumns;
			fieldSettings.countError = countError;
			fieldSettings.countMines = countMines;
			fieldSettings.countRows = countRows;
			fieldSettings.options = options;
			return fieldSettings;
		}
		public int ColumnCount {
			get { return countColumns; }
			set {
				if(value < MinimumColumnCount)
					value = MinimumColumnCount;
				countColumns = value;
			}
		}
		public int ErrorCount {
			get { return countError; }
			set {
				if(value > 0 && value < MineCount) {
					countError = value;
				}
			}
		}
		public bool HasCenter {
			get { return GetOption(FieldOptions.HasCenter); }
			set { SetOption(FieldOptions.HasCenter, value); }
		}
		public int MineCount {
			get { return countMines; }
			set {
				int count = (int)(countColumns * countRows * MinesMaxPercent);
				if(count < value)
					value = count;
				countMines = value;
			}
		}
		public int RowCount {
			get { return countRows; }
			set {
				if(value < MinimumRowCount)
					value = MinimumRowCount;
				countRows = value;
			}
		}
		public bool UseEffect3D {
			get { return GetOption(FieldOptions.UseEffect3D); }
			set { SetOption(FieldOptions.UseEffect3D, value); }
		}
	}

	public class VisualFieldSettings<TBox> : FieldSettings where TBox : struct {
		private TBox boxBounds;
		protected virtual TBox CorrectBoundsBox(TBox box) {
			return box;
		}
		public TBox BoundsBox { get { return boxBounds; } set { boxBounds = CorrectBoundsBox(value); } }
		public override Object Clone() {
			VisualFieldSettings<TBox> fieldSettings = base.Clone() as VisualFieldSettings<TBox>;
			fieldSettings.boxBounds = boxBounds;
			return fieldSettings;
		}
	}

	public enum CellBackfillType : int { Closed, Opened, Detonated }

	public enum FieldMessage : int { Initialized, SelectCell, ChangeLabelsCount, MinesDetonated, RegenerationNeeded, AllMinesDetonated, AllMinesLabeled, AllEmptyCellsOpened }

	public interface IPainter<TVertex, TBox> {
		void BeginPaint(TBox box);
		void DrawBorder3D(TVertex[] vertices);
		void DrawInteger(TVertex centerPosition, TBox box, int value);
		void DrawLabel(TVertex centerPosition, TBox box);
		void DrawMine(TVertex centerPosition, TBox box, bool isRealMine);
		void DrawPolygon(TVertex[] vertices);
		void DrawUndefined(TVertex centerPosition, TBox box);
		void EndPaint();
		void FillPolygon(TVertex[] vertices, CellBackfillType fillType);
		bool IsValid { get; }
	}

	public interface ISelector<TVertex, TBox> {
		void AddCell(ICell cell, TVertex[] cellContour);
		void Initialize(TBox box);
		ICell GetCell(TVertex position);
	}

	public interface IDraw<TVertex, TBox> {
		void Draw(IPainter<TVertex, TBox> painter, ISelector<TVertex, TBox> selector);
	}

	public interface IField {
		event EventHandler<FieldMessageEventArgs> Message;
		void ClearMessage();
		void Initialize(FieldSettings settings);
		void NextStateCell(ICell cell);
		void OpenCell(ICell cell, bool isWithAround);
		void SelectCell(ICell cell, bool isWithAround);
		int MineCount { get; }
		int CellCount { get; }
		int LabelCount { get; }
	}

	public interface IVisualField<TVertex, TBox> : IField, IDraw<TVertex, TBox> {
		void ChangeBox(TBox box);
	}

	public interface ICell {
		void AddAroundCell(ICell cell);
		bool ChangeState();
		bool MarkLabel();
		CellOperationOpening Open(bool isWithAround);
		int SetNextState();
		void ShowMine();
		bool CanOpened { get; }
		CellsCollection<ICell> CellsAround { get; }
		bool Closed { get; set; }
		bool HasMine { get; set; }
		bool HasVisited { get; set; }
		bool IsLabelState { get; }
		bool IsMineDetonated { get; set; }
		bool IsSelected { get; set; }
		bool IsSelectedAround { get; set; }
		int MinesNumber { get; }
	}

	public interface IVisualCell<TVertex, TBox> : ICell, IDraw<TVertex, TBox> {
		void Initialize(CellOperationInitialize<TVertex, TBox> cellInit);
	}

	public class CellsCollection<TCell> : Collection<TCell> where TCell : ICell {
	}
	
	public struct CellOperationOpening : IEquatable<CellOperationOpening> {
		private int countMines;
		private int countOpened;
		public CellOperationOpening(int mines, int opened) {
			countMines = mines;
			countOpened = opened;
		}
		public void Add(CellOperationOpening opening) {
			countMines += opening.MineCount;
			countOpened += opening.OpenedCount;
		}
		public override int GetHashCode() {
			return countMines ^ countOpened;
		}
		public override bool Equals(object obj) {
			if(!(obj is CellOperationOpening))
				return false;
			return Equals((CellOperationOpening)obj);
		}
		public bool Equals(CellOperationOpening other) {
			if(countMines != other.countMines)
				return false;
			return countOpened == other.countOpened;
		}
		public static bool operator ==(CellOperationOpening opening1, CellOperationOpening opening2) {
			return opening1.Equals(opening2);
		}
		public static bool operator !=(CellOperationOpening opening1, CellOperationOpening opening2) {
			return !opening1.Equals(opening2);
		}
		public static CellOperationOpening operator +(CellOperationOpening opening1, CellOperationOpening opening2) {
			CellOperationOpening opening = opening1;
			opening.Add(opening2);
			return opening;
		}
		public int MineCount { get { return countMines; } set { countMines = value; } }
		public bool HasMines { get { return countMines > 0; } }
		public int OpenedCount { get { return countOpened; } set { countOpened = value; } }
	}

	public struct CellOperationInitialize<TVertex, TBox> : IEquatable<CellOperationInitialize<TVertex, TBox>> {
		private double angleStart;
		private double angleSweep;
		private TBox boxBig;
		private TBox boxExtents;
		private TBox boxSmall;
		private bool notUseEffect3D;
		public TBox BigBox { get { return boxBig; } set { boxBig = value; } }
		public TBox ExtentsBox { get { return boxExtents; } set { boxExtents = value; } }
		public TBox SmallBox { get { return boxSmall; } set { boxSmall = value; } }
		public double StartAngle { get { return angleStart; } set { angleStart = value; } }
		public double SweepAngle { get { return angleSweep; } set { angleSweep = value; } }
		public bool UseEffect3D { get { return !notUseEffect3D; } set { notUseEffect3D = !value; } }
		public override int GetHashCode() {
			return 0;
		}
		public override bool Equals(object obj) {
			if(!(obj is CellOperationInitialize<TVertex, TBox>))
				return false;
			return Equals((CellOperationInitialize<TVertex, TBox>)obj);
		}
		public bool Equals(CellOperationInitialize<TVertex, TBox> other) {
			if(!boxBig.Equals(other.boxBig) || (notUseEffect3D != other.notUseEffect3D))
				return false;
			return (boxSmall.Equals(other.boxSmall)) && (boxExtents.Equals(other.boxExtents)) && (angleStart == other.angleStart) && (angleSweep == other.angleSweep);
		}
		public static bool operator ==(CellOperationInitialize<TVertex, TBox> cellOperation1, CellOperationInitialize<TVertex, TBox> cellOperation2) {
			return cellOperation1.Equals(cellOperation2);
		}
		public static bool operator !=(CellOperationInitialize<TVertex, TBox> cellOperation1, CellOperationInitialize<TVertex, TBox> cellOperation2) {
			return !cellOperation1.Equals(cellOperation2);
		}
		public CellOperationInitialize(TBox big, TBox small, TBox extents, double start, double sweep, bool useEffect3D) {
			angleStart = start;
			angleSweep = sweep;
			boxBig = big;
			boxSmall = small;
			boxExtents = extents;
			notUseEffect3D = !useEffect3D;
		}
		public CellOperationInitialize(bool useEffect3D)
			: this() {
			angleStart = 0;
			angleSweep = 360;
			notUseEffect3D = !useEffect3D;
		}
		public CellOperationInitialize(TBox box)
			: this(true) {
			boxBig = boxSmall = boxExtents = box;
		}
	}
}
