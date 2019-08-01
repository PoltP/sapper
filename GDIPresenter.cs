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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Sapper.Types;

namespace Sapper.WinSapper {
	using Point2D = System.Drawing.PointF;
	using Size2D = System.Drawing.SizeF;
	using Box2D = System.Drawing.RectangleF;
	using Float2D = System.Single;

	public sealed class FieldManager : IDisposable {
		public const int FieldCount = 4;
		private struct MouseState {
			public bool LeftDown;
			public bool RightDown;
		}
		private static readonly Type[] FieldTypes = new Type[FieldCount] { typeof(RectangularRectanglesField), typeof(RectangularEllipsesField), typeof(EllipticTrapezoidsField), typeof(EllipticPolarField) };
		private static readonly string[] FieldNames = new string[FieldCount] { "Rectangular rectangles", "Rectangular ellipses", "Elliptic trapezoids", "Elliptic polar" };
		private static readonly TabularFieldSettings[] FieldTabularSettings = new TabularFieldSettings[FieldCount] { new TabularFieldSettings(), new TabularFieldSettings(), new TabularFieldSettings(6, 12), new TabularFieldSettings(8, 16) };
		private static int UsedFieldTypeIndex;
		private IVisualField<Point2D, Box2D> field;
		private GDIPainter gdiPainter;
		private GDISelector gdiSelector;
		private MouseState stateMouse = new MouseState();
		private void Clear() {
			stateMouse.LeftDown = false;
			stateMouse.RightDown = false;
			if(field != null) {
				field.ClearMessage();
			}
		}
		private void FieldMessageHandler(object sender, FieldMessageEventArgs e) {
			switch(e.Message) {
				case FieldMessage.RegenerationNeeded:
					Paint();
					break;
				case FieldMessage.AllMinesLabeled:
				case FieldMessage.AllEmptyCellsOpened:
				case FieldMessage.AllMinesDetonated:
					gdiSelector.Clear();
					break;
			}
		}
		private void MouseUpDown(MouseEventArgs e, bool isDown) {
			ICell cell = gdiSelector.GetCell(new Point2D(e.X, e.Y));
			switch(e.Button) {
				case MouseButtons.Right:
					if(stateMouse.LeftDown) {
						ProcessCell(cell, true, isDown);
					} else if(isDown) {
						field.NextStateCell(cell);
					}
					stateMouse.RightDown = isDown;
					break;
				case MouseButtons.Left:
					ProcessCell(cell, stateMouse.RightDown, isDown);
					stateMouse.LeftDown = isDown;
					break;
				case MouseButtons.Middle:
					ProcessCell(cell, true, isDown);
					break;
			}
		}
		private void Paint() {
			field.Draw(gdiPainter, gdiSelector);
		}
		private void ProcessCell(ICell cell, bool isWithAround, bool isSelect) {
			if(isSelect) {
				field.SelectCell(cell, isWithAround);
			} else {
				field.OpenCell(cell, isWithAround);
			}
		}
		internal void MouseDownEventHandler(object sender, MouseEventArgs e) {
			MouseUpDown(e, true);
		}
		internal void MouseMoveEventHandler(object sender, MouseEventArgs e) {
			switch(e.Button) {
				case MouseButtons.Left | MouseButtons.Right:
				case MouseButtons.Left:
				case MouseButtons.Middle:
					field.SelectCell(gdiSelector.GetCell(new Point2D(e.X, e.Y)), (e.Button == (MouseButtons.Left | MouseButtons.Right)) || e.Button == (MouseButtons.Middle));
					break;
				default:
					// Correction for the next focus changes: Explorer->"Start", Explorer->"Alt+TAB"
					field.SelectCell(null, true);
					break;
			}
		}
		internal void MouseUpEventHandler(object sender, MouseEventArgs e) {
			MouseUpDown(e, false);
		}
		internal void PaintEventHandler(object sender, PaintEventArgs e) {
			Paint();
		}
		public static string GetFieldName(int index) {
			if(index >= 0 && index < FieldCount) {
				return FieldNames[index];
			} else {
				return null;
			}
		}
		public static TabularFieldSettings GetFieldSettings(int index) {
			if(index >= 0 && index < FieldCount) {
				return FieldTabularSettings[index];
			} else {
				return null;
			}
		}
		public static Type GetFieldType(int index) {
			if(index >= 0 && index < FieldCount) {
				return FieldTypes[index];
			} else {
				return null;
			}
		}
		public FieldManager(Graphics graph) {
			gdiPainter = new GDIPainter(graph);
			gdiSelector = new GDISelector();
		}
		public void ChangeFieldSize(Graphics graphics, Box2D box) {
			gdiPainter.Graph = graphics;
			if(field != null) {
				field.ChangeBox(box);
			}
		}
		public void ChangeFieldType(FieldSettings settingsField, Type fieldType, EventHandler<FieldMessageEventArgs> mainFieldMessageHandler) {
			Clear();
			if(fieldType == null)
				fieldType = FieldTypes[(UsedFieldTypeIndex++) % FieldTypes.Length];
			field = System.Activator.CreateInstance(fieldType) as IVisualField<Point2D, Box2D>;
			field.Message += mainFieldMessageHandler;
			field.Message += new EventHandler<FieldMessageEventArgs>(FieldMessageHandler);
			field.Initialize(settingsField);
		}
		public void Dispose() {
			Clear();
			if(gdiPainter != null) {
				gdiPainter.Dispose();
				gdiPainter = null;
			}
			if(gdiSelector != null) {
				gdiSelector.Dispose();
				gdiSelector = null;
			}
		}
	}

	internal class GDIPainter : IPainter<Point2D, Box2D>, IDisposable {
		public static readonly Color BackColor = Color.DarkGray;//Color.Silver;
		private const int ColorsCount = 16;
		private static Color MineColor = Color.Indigo;
		private static Color CellClosedFillColor = Color.Gainsboro;
		private static Color CellOpenedFillColor = Color.Silver;
		private static Color CellBorderColor = Color.DimGray;
		private static Color[] MineNumberColors = new Color[ColorsCount]{Color.Blue, Color.Blue,
				Color.Green, Color.Red, Color.DarkBlue, Color.Maroon, Color.Teal, Color.SlateGray,
				Color.Black, Color.DeepSkyBlue, Color.LightSeaGreen, Color.IndianRed, Color.DarkViolet,
				Color.RosyBrown, Color.SteelBlue, Color.DarkSlateGray};
		private static Pen BorderPen = new Pen(CellBorderColor);
		private static Pen Border3DPenLight = new Pen(Color.GhostWhite, 1.8f * Geometric.CellOffset) { LineJoin = LineJoin.Miter /*, Alignment = PenAlignment.Right*/};
		private static Pen Border3DPenDark = new Pen(Color.DarkGray, Border3DPenLight.Width) { LineJoin = Border3DPenLight.LineJoin /*, Alignment = Border3DPenLight.Alignment*/};
		private static Pen MinePen = new Pen(MineColor);
		private static Pen BlackPen = new Pen(Color.Black);
		private static SolidBrush FillClosedBrush = new SolidBrush(CellClosedFillColor);
		private static SolidBrush FillOpenedBrush = new SolidBrush(CellOpenedFillColor);
		private static SolidBrush BlackBrush = new SolidBrush(Color.Black);
		private static SolidBrush MineBrush = new SolidBrush(MineColor);
		private static SolidBrush RedBrush = new SolidBrush(Color.Red);
		private static SolidBrush WhiteBrush = new SolidBrush(Color.White);
		private static string FontName = "Comic Sans MS";//"Courier New", "BankGothic Lt BT"
		private static StringFormat TextAlign;
		private Bitmap bitmapBuffer;
		private Graphics graph;
		private Graphics graphBuffer;
		private static StringFormat GetTextAlign() {
			if(TextAlign == null) {
				TextAlign = new StringFormat();
				TextAlign.Alignment = StringAlignment.Center;
				TextAlign.LineAlignment = StringAlignment.Center;
			}
			return TextAlign;
		}
		private void DrawText(Point2D centerPosition, Box2D box, string text, SolidBrush textBrush) {
			if(!Geometric.CheckBox(box)) return;
			Font font = new Font(FontName, box.Height, FontStyle.Bold);
			Graph.DrawString(text, font, textBrush, centerPosition, GetTextAlign());
		}
		protected virtual void Dispose(bool disposing) {
			if(disposing) {
				graph = null;
				if(bitmapBuffer != null) {
					bitmapBuffer.Dispose();
				}
			}
		}
		public Graphics Graph {
			get {
				if(graphBuffer != null) {
					return graphBuffer;
				} else {
					return graph;
				}
			}
			set { graph = value; }
		}
		public GDIPainter(Graphics graphics) {
			graph = graphics;
		}
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#region Help
		/// <summary>
		/// Initializes double buffer
		/// </summary>
		#endregion
		public void BeginPaint(Box2D box) {
			if(Geometric.CheckBox(box)) {
				box = Box2D.Inflate(box, Geometric.FieldOffset, Geometric.FieldOffset);
				bitmapBuffer = new Bitmap((int)(box.Width), (int)(box.Height), graph);
				graphBuffer = Graphics.FromImage(bitmapBuffer);
				//gdiSelector.BitmapSize = boxBounds.Size;// if selector in painter
			}
			Graph.Clear(BackColor);
		}
		public void DrawBorder3D(Point2D[] vertices) {
			int centerIndex = vertices.Length / 2;
			Point2D[] upPoints = new Point2D[centerIndex + 1];
			Array.Copy(vertices, upPoints, upPoints.Length);
			Graph.DrawLines(Border3DPenLight, upPoints);
			Point2D[] downPoints = new Point2D[vertices.Length - centerIndex + 1];
			Array.Copy(vertices, centerIndex, downPoints, 0, downPoints.Length - 1);
			downPoints[downPoints.Length - 1] = vertices[0];
			Graph.DrawLines(Border3DPenDark, downPoints);
		}
		public void DrawInteger(Point2D centerPosition, Box2D box, int value) {
			SolidBrush textBrush;
			System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;//new System.Globalization.CultureInfo("en-us");
			textBrush = new SolidBrush(MineNumberColors[value % MineNumberColors.Length]);
			DrawText(centerPosition, box, Convert.ToString(value, ci), textBrush);
		}
		public void DrawLabel(Point2D centerPosition, Box2D box) {
			Point2D[] points = new Point2D[3];
			Box2D baseRect = new Box2D();
			baseRect.Size = new Size2D(box.Width / 2, box.Height / 4);
			baseRect.Location = new Point2D(centerPosition.X - baseRect.Size.Width / 2, box.Bottom - baseRect.Size.Height / 2);
			points[0] = centerPosition;
			points[1] = new Point2D(box.Right, box.Top + box.Height / 4);
			points[2] = new Point2D(centerPosition.X, box.Top);
			Graph.DrawLine(BlackPen, points[2], new Point2D(centerPosition.X, box.Bottom));
			Graph.FillPolygon(RedBrush, points);
			Graph.FillRectangle(BlackBrush, baseRect);
		}
		public void DrawMine(Point2D centerPosition, Box2D box, bool isRealMine) {
			Box2D mineRect = Box2D.Inflate(box, -box.Width / 8, -box.Height / 8);
			Box2D blickRect = new Box2D();
			blickRect.Size = new Size2D(mineRect.Width / 4, mineRect.Height / 4);
			blickRect.Location = new Point2D(centerPosition.X - blickRect.Size.Width, centerPosition.Y - blickRect.Size.Height);
			Graph.DrawLine(MinePen, mineRect.Location, new Point2D(mineRect.Right, mineRect.Bottom));
			Graph.DrawLine(MinePen, new Point2D(mineRect.Left, mineRect.Bottom), new Point2D(mineRect.Right, mineRect.Top));
			Graph.DrawLine(MinePen, new Point2D(centerPosition.X, box.Top), new Point2D(centerPosition.X, box.Bottom));
			Graph.DrawLine(MinePen, new Point2D(box.Left, centerPosition.Y), new Point2D(box.Right, centerPosition.Y));
			Graph.FillEllipse(MineBrush, mineRect);
			Graph.FillRectangle(WhiteBrush, blickRect);
			if(!isRealMine) {
				Pen redPen = new Pen(Color.Red, 2);
				Graph.DrawLine(redPen, new Point2D(box.Left, box.Top), new Point2D(box.Right, box.Bottom));
				Graph.DrawLine(redPen, new Point2D(box.Right, box.Top), new Point2D(box.Left, box.Bottom));
			}
		}
		public void DrawPolygon(Point2D[] vertices) {
			Graph.DrawPolygon(BorderPen, vertices);
		}
		public void DrawUndefined(Point2D centerPosition, Box2D box) {
			DrawText(centerPosition, box, "?", BlackBrush);
		}
		public void EndPaint() {
			if(bitmapBuffer != null) {
				graph.DrawImage(bitmapBuffer, 0, 0);
				bitmapBuffer.Dispose();
				bitmapBuffer = null;
			}
			if(graphBuffer != null) {
				graphBuffer.Dispose();
				graphBuffer = null;
			}
		}
		public void FillPolygon(Point2D[] vertices, CellBackfillType fillType) {
			SolidBrush brush;
			switch (fillType) {
				case CellBackfillType.Detonated:
					brush = RedBrush;
					break;
				case CellBackfillType.Opened:
					brush = FillOpenedBrush;
					break;
				default:
					brush = FillClosedBrush;
					break;
			}
			Graph.FillPolygon(brush, vertices);
		}
		public bool IsValid { get { return Graph != null; } }
	}

	internal class GDISelector : ISelector<Point2D, Box2D>, IDisposable {
		private static readonly Color defaultColor = Color.White;
		private Bitmap bitMap;
		private Graphics canvas;
		private SolidBrush brush;
		private Color brushColor;
		private CellsCollection<ICell> cells;
		protected virtual void Dispose(bool disposing) {
			if(disposing) {
				if(brush != null) {
					brush.Dispose();
					brush = null;
				}
				if(bitMap != null) {
					bitMap.Dispose();
					bitMap = null;
				}
				if(cells != null) {
					cells.Clear();
					cells = null;
				}
			}
		}
		public GDISelector() {
			cells = new CellsCollection<ICell>();
		}
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void SetColor(Color color) {
			brushColor = color;
			brush = new SolidBrush(brushColor);
		}
		private static Color IntToColor(int value) {
			int r = value >> 16;
			int g = (value >> 8) - (r << 8);
			int b = (value - (r << 16) - (g << 8));
			try {
				return Color.FromArgb(r, g, b);
			} catch(ArgumentException) {
				return Color.Empty;
			}
		}
		private static int ColorToInt(Color color) {
			return (color.B + (color.R << 16) + (color.G << 8));
		}
		public void AddCell(ICell cell, Point2D[] cellContour) {
			if(cell != null) {
				SetColor(IntToColor(cells.Count));
				cells.Add(cell);
				Canvas.FillPolygon(brush, cellContour);
			}
		}
		public void Clear() {
			SetColor(defaultColor);
			canvas.FillRectangle(new SolidBrush(defaultColor), canvas.ClipBounds);
			cells.Clear();
		}
		public ICell GetCell(Point2D position) {
			int x = (int)position.X, y = (int)position.Y;
			int index = -1;
			try {
				if((x < bitMap.Width) && (y < bitMap.Height) && (x > 0) && (y > 0)) {
					index = ColorToInt(bitMap.GetPixel(x, y));
				}
			} catch(ArgumentOutOfRangeException) {
				return null;
			}
			if((index > -1) && (index < cells.Count)) {
				return cells[index];
			} else {
				return null;
			}
		}
		public void Initialize(Box2D box) {
			//boxBounds = Box2D.Inflate(boxBounds, TabularFieldSettings.FieldOffset, TabularFieldSettings.FieldOffset);
			BitmapSize = box.Size;
		}
		public Graphics Canvas { get { return canvas; } }
		#region Help
		/// <summary>
		/// Selection bitmap size 
		/// </summary>
		#endregion
		public Size2D BitmapSize {
			set {
				if(Geometric.CheckSize(value)) {
					if(bitMap != null) {
						bitMap.Dispose();
					}
					bitMap = new Bitmap((int)value.Width, (int)value.Height);
					canvas = Graphics.FromImage(bitMap);
					Clear();
				}
			}
		}
	}
}
