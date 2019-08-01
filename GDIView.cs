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
using Sapper.Kernel;
using Sapper.Types;

namespace Sapper.WinSapper {
	using Point2D = System.Drawing.PointF;
	using Size2D = System.Drawing.SizeF;
	using Box2D = System.Drawing.RectangleF;
	using Float2D = System.Single;

	public static class Geometric {
		public const Float2D CellOffset = 1.2f;
		public const Float2D FieldOffset = 2;
		public static double Degree(double radian) {
			return (radian * 180.0 / Math.PI);
		}
		public static double Radian(double angle) {
			return (angle * Math.PI / 180.0);
		}
		#region Help
		/// <summary>
		/// Set points of the elliptic curve use clockwise direction of
		/// starting angle from the x-axis.		
		/// </summary>
		/// <returns>
		/// Returns center point of current elliptic segment.
		/// </returns>
		#endregion
		public static Point2D CalculateEllipticPoints(Point2D[] points, int pointsCount,
			int startIndex, Box2D bounds, double startAngle, double sweepAngle, bool isReverseOrder) {
			double angle, majorRadius, minorRadius, sinus, cosinus;
			double sweep = Geometric.Radian(sweepAngle / (pointsCount - 1));
			if(bounds.Width > bounds.Height) {
				majorRadius = bounds.Width / 2;
				minorRadius = bounds.Height / 2;
				sinus = 0;
				cosinus = 1;
				angle = Geometric.Radian(startAngle);
			} else {
				majorRadius = bounds.Height / 2;
				minorRadius = bounds.Width / 2;
				sinus = 1;
				cosinus = 0;
				angle = Geometric.Radian(startAngle - 90);
			}
			int beginIndex = startIndex;
			int sign = 1;
			if(isReverseOrder) {
				sign = -sign;
				beginIndex += pointsCount - 1;
			}
			int index;
			double x, y;
			double cX = (bounds.Left + bounds.Right) / 2;
			double cY = (bounds.Top + bounds.Bottom) / 2;
			for(int i = 0; i < pointsCount; i++) {
				index = beginIndex + sign * i;
				x = majorRadius * Math.Cos(angle);
				y = minorRadius * Math.Sin(angle);
				points[index].X = (Float2D)(cX + x * cosinus - y * sinus);
				points[index].Y = (Float2D)(cY + x * sinus + y * cosinus);
				angle += sweep;
			}
			index = startIndex + pointsCount / 2;
			if(pointsCount % 2 == 0) {
				return Geometric.CenterPoint(points[index - 1], points[index]);
			} else {
				return points[index];
			}
		}
		public static Point2D CenterPoint(Point2D pointStart, Point2D pointEnd) {
			Point2D center = new Point2D();
			center.X = (pointStart.X + pointEnd.X) / 2;
			center.Y = (pointStart.Y + pointEnd.Y) / 2;
			return center;
		}
		public static bool CheckBox(Box2D box) {
			return (box.Width > 0 && box.Height > 0);
		}
		public static bool CheckSize(Size2D size) {
			return (size.Width > 0 && size.Height > 0);
		}
		public static Box2D GetOptimalFieldSize(Size2D size) {
			return new Box2D(FieldOffset, FieldOffset, size.Width - (4 * FieldOffset), size.Height - (4 * FieldOffset));
		}
		#region Help
		/// <summary>
		/// Calculates elliptic angle by S.Romanujan,1914 numeric formula
		/// </summary>
		/// <param name="size">Width and height of elliptic curve box. </param>
		/// <param name="length">Length of the elliptic curve segment. </param>
		/// <returns>
		/// Elliptic angle in radians
		/// </returns>
		#endregion Help
		public static double EllipticAngle(double length, Size2D size) {
			double q = Math.Pow((size.Width - size.Height) / (size.Width + size.Height), 2);
			return 4 * length / ((size.Width + size.Height) * (1 + 3 * q / (10 + Math.Sqrt(4 - 3 * q))));
		}
	}
	public class TabularFieldSettings : VisualFieldSettings<Box2D> {
		protected override Box2D CorrectBoundsBox(Box2D box) {
			if(box.Height < DefaultHeight) {
				box.Height = DefaultHeight;
			}
			if(box.Width < DefaultWidth) {
				box.Width = DefaultWidth;
			}
			return base.CorrectBoundsBox(box);
		}
		public TabularFieldSettings()
			: base() {
		}
		public TabularFieldSettings(int rows, int columns)
			: base() {
			RowCount = rows;
			ColumnCount = columns;
		}
	}

	#region VisualCell implementations
	public class EllipticCell : VisualCell<Point2D, Box2D> {
		private const byte EllipticVerticesCount = 2 * 9;
		protected override Box2D GetInternalBox(double boxCoefficient) {
			Box2D internalBox = new Box2D();
			internalBox.Location = Center;
			internalBox.Width = (Float2D)(boxCoefficient * BoundsBox.Width);
			internalBox.Height = (Float2D)(boxCoefficient * BoundsBox.Height);
			internalBox.Location = new Point2D(internalBox.Location.X - internalBox.Width / 2,
				internalBox.Location.Y - internalBox.Height / 2);
			return internalBox;
		}
		protected override int VertexCount { get { return EllipticVerticesCount; } }
		internal override void InitializeVertices(CellOperationInitialize<Point2D, Box2D> cellInit) {
			Point2D centerElliptic = Geometric.CalculateEllipticPoints(vertices, VertexCount, 0, cellInit.ExtentsBox, 0, 360, true);
			Center = Geometric.CenterPoint(vertices[0], centerElliptic);
			if(cellInit.UseEffect3D) {
				cellInit.ExtentsBox = Box2D.Inflate(cellInit.ExtentsBox, -Geometric.CellOffset, -Geometric.CellOffset);
				Geometric.CalculateEllipticPoints(verticesEffect3D, VertexCount, 0, cellInit.ExtentsBox, 0, 360, true);
			}
		}
	}
	public class RectangularCell : EllipticCell {
		private const byte RectangleVerticesCount = 4;
		private static void FillVertices(Point2D[] arrVertices, Box2D box) {
			arrVertices[0] = new Point2D(box.Left, box.Top);
			arrVertices[1] = new Point2D(box.Right, box.Top);
			arrVertices[2] = new Point2D(box.Right, box.Bottom);
			arrVertices[3] = new Point2D(box.Left, box.Bottom);
		}
		protected override int VertexCount { get { return RectangleVerticesCount; } }
		internal override void InitializeVertices(CellOperationInitialize<Point2D, Box2D> cellInit) {
			FillVertices(vertices, BoundsBox);
			Center = Geometric.CenterPoint(vertices[0], vertices[VertexCount / 2]);
			if(cellInit.UseEffect3D) {
				FillVertices(verticesEffect3D, Box2D.Inflate(BoundsBox, -Geometric.CellOffset, -Geometric.CellOffset));
			}
		}
	}

	public class PolarCell : EllipticCell {
		private const byte PolarVerticesCount = 2 * 5;
		protected override int VertexCount { get { return PolarVerticesCount; } }
		internal override void InitializeVertices(CellOperationInitialize<Point2D, Box2D> cellInit) {
			int count = VertexCount / 2;
			Point2D center1 = Geometric.CalculateEllipticPoints(vertices, count, 0, cellInit.BigBox, cellInit.StartAngle, cellInit.SweepAngle, true);
			Point2D center2 = Geometric.CalculateEllipticPoints(vertices, count, count, cellInit.SmallBox, cellInit.StartAngle, cellInit.SweepAngle, false);
			Center = Geometric.CenterPoint(center1, center2);
			if(cellInit.UseEffect3D) {
				Box2D bigBox = Box2D.Inflate(cellInit.BigBox, -Geometric.CellOffset, -Geometric.CellOffset);
				Box2D smallBox = Box2D.Inflate(cellInit.SmallBox, Geometric.CellOffset, Geometric.CellOffset);
				double bigOffset = Geometric.Degree(Geometric.EllipticAngle(Geometric.CellOffset, bigBox.Size));
				double smallOffset = Geometric.Degree(Geometric.EllipticAngle(Geometric.CellOffset, smallBox.Size));
				Geometric.CalculateEllipticPoints(verticesEffect3D, count, 0, bigBox, cellInit.StartAngle + bigOffset, cellInit.SweepAngle - 2 * bigOffset, true);
				Geometric.CalculateEllipticPoints(verticesEffect3D, count, count, smallBox, cellInit.StartAngle + smallOffset, cellInit.SweepAngle - 2 * smallOffset, false);
			}
		}
	}

	public class TrapezoidCell : PolarCell {
		private const byte TrapezoidVerticesCount = 2 * 2;
		protected override int VertexCount { get { return TrapezoidVerticesCount; } }
	}
	#endregion VisualCell implementations

	#region VisualField implementations
	public class RectangularField<TCell> : TabularField<TCell, Point2D, Box2D>
		where TCell : EllipticCell, new() {
		public RectangularField()
			: base() {
		}
		protected override void SetVisualParameters() {
			base.SetVisualParameters();
			CellOperationInitialize<Point2D, Box2D> initCell = new CellOperationInitialize<Point2D, Box2D>(Settings.UseEffect3D);
			double width = Settings.BoundsBox.Width / ColumnCount;
			double height = Settings.BoundsBox.Height / RowCount;
			for(int i = 0; i < Cells.Count; i++) {
				initCell.ExtentsBox = new Box2D((Float2D)(Settings.BoundsBox.Location.X + width * (i % ColumnCount)),
					(Float2D)(Settings.BoundsBox.Location.Y + height * (i / ColumnCount)), (Float2D)width, (Float2D)height);
				Cells[i].Initialize(initCell);
			}
		}
	}

	public class RectangularRectanglesField : RectangularField<RectangularCell> {
	}

	public class RectangularEllipsesField : RectangularField<EllipticCell> {
	}

	public class EllipticField<TCell> : TabularField<EllipticCell, Point2D, Box2D>
		where TCell : EllipticCell, new() {
		protected int CenterCellCount { get { return Convert.ToInt32(HasCenterCell); } }
		protected override void InitializeCells(int count) {
			count = base.CellCount;
			for(int i = 0; i < count; i++) {
				Cells.Add(new TCell());
			}
			if(HasCenterCell) {
				Cells.Add(new EllipticCell());
			}
		}
		protected override void SetLinks(int count) {
			count = base.CellCount;
			base.SetLinks(count);
			int i, j, k, n2, n3, index;
			for(n2 = 1, j = 0; n2 <= 2; n2++, j += ColumnCount - 1) {
				for(i = j; i < count; i += ColumnCount) {
					index = (i - 1) - 2 * j;// on second n2-iteration: index = i - 2*ColumnCount + 1;
					for(n3 = 1, k = 0; n3 <= 3; n3++, k += ColumnCount) {
						AddLinkToCell(i, index + k, count, -1, false);
					}
				}
			}
			if(HasCenterCell) {
				count = CellCount;
				int lastCellIndex = count - 1;
				for(i = 1; i <= ColumnCount; i++) {
					AddLinkToCell(lastCellIndex, lastCellIndex - i, count, -1, false);
					AddLinkToCell(lastCellIndex - i, lastCellIndex, count, -1, false);
				}
			}
		}
		protected override void SetVisualParameters() {
			base.SetVisualParameters();
			int count = base.CellCount;
			CellOperationInitialize<Point2D, Box2D> initCell = new CellOperationInitialize<Point2D, Box2D>(Settings.UseEffect3D);
			initCell.BigBox = Settings.BoundsBox;
			initCell.ExtentsBox = new Box2D(0, 0, Settings.BoundsBox.Width / (2 * (RowCount + 1)), Settings.BoundsBox.Height / (2 * (RowCount + 1)));
			initCell.SweepAngle = 360.0f / ColumnCount;
			initCell.StartAngle = 0;
			for(int i = 0, row; i < count; i++) {
				row = (i / ColumnCount) + 1;
				if((row > 1) && (i % ColumnCount) == 0) {
					initCell.BigBox = initCell.SmallBox;
				}
				initCell.StartAngle = initCell.SweepAngle * (i % ColumnCount);
				initCell.SmallBox = new Box2D(Settings.BoundsBox.Location.X + initCell.ExtentsBox.Width * row,
					Settings.BoundsBox.Location.Y + initCell.ExtentsBox.Height * row,
					Settings.BoundsBox.Width - 2 * initCell.ExtentsBox.Width * row,
					Settings.BoundsBox.Height - 2 * initCell.ExtentsBox.Height * row);
				Cells[i].Initialize(initCell);
			}
			if(HasCenterCell) {
				initCell.ExtentsBox = initCell.SmallBox;
				Cells[count].Initialize(initCell);
			}
		}
		public override int CellCount { get { return base.CellCount + CenterCellCount; } }
		#region Help
		/// <summary>
		/// Gets or sets a value indicating whether field is has a center cell.
		/// </summary>
		#endregion Help
		public bool HasCenterCell {
			get { return Settings.HasCenter; }
			set { Settings.HasCenter = value; }
		}
	}

	public class EllipticTrapezoidsField : EllipticField<TrapezoidCell> {
		protected override void SetFieldSettings(FieldSettings settings) {
			base.SetFieldSettings(settings);
			HasCenterCell = false;
		}
	}

	public class EllipticPolarField : EllipticField<PolarCell> {
	}
	#endregion VisualField implementations
}
