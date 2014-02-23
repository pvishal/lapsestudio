using System;
using System.Collections.Generic;
using System.Linq;
using Timelapse_API;

namespace Timelapse_UI
{
	public sealed class BrightnessGraph : IDisposable
	{
		#region Variables

		public int Width;
		public int Height;
		public int Pointcount
		{
			get { return ProjectManager.CurrentProject.Frames.Count - 1; }
		}

		public double Xpos;
		public double Ypos;
		public double Range
		{
			get { return (ProjectManager.CurrentProject.Frames.Max(t => t.AlternativeBrightness) - ProjectManager.CurrentProject.Frames.Min(t => t.AlternativeBrightness)) * 1.05; }
		}
		public double Py0
		{
			get { return ProjectManager.CurrentProject.Frames[0].AlternativeBrightness; }
		}

		public const int Left = 30;
		public const int Right = 20;
		public const int Top = 20;
		public const int Bottom = 30;
		public const int Smoothness = 150;

		public int SelectedPoint { get; private set; }
		public List<PointD> Points { get; private set; }
		public bool IsMoving { get; private set; }

		public Rectangle CurveArea;
		public bool IsOnCurveArea
		{
			get { return (Xpos > Left - 2 && Xpos < Width - Right + 2 && Ypos > Top - 2 && Ypos < Height - Bottom + 2) ? true : false; }
		}

		#endregion

		#region Init

		public BrightnessGraph(int Width, int Height)
		{
			CurveArea = new Rectangle();
			Points = new List<PointD>();
			SetNewSize(Width, Height);
		}

		public void Init()
		{
			SelectedPoint = 0;
			IsMoving = false;
			ProjectManager.BrightnessCalculated += CurrentProject_BrightnessCalculated;
		}

		public void InitPoints()
		{
			Points.Clear();
			SelectedPoint = 0;
			Points.Add(new PointD(0, (float)ProjectManager.CurrentProject.Frames[0].AlternativeBrightness));
			Points.Add(new PointD(ProjectManager.CurrentProject.Frames.Count - 1, (float)ProjectManager.CurrentProject.Frames[ProjectManager.CurrentProject.Frames.Count - 1].AlternativeBrightness));
		}

		#endregion

		#region Methods

		public void SetFromLoading(List<PointD> Points, int SelectedPoint)
		{
			this.Points = Points;
			this.SelectedPoint = SelectedPoint;
			RefreshGraph();
		}

		public void YtoEnd()
		{
			Points[SelectedPoint] = new PointD(Points[SelectedPoint].X, Points[Points.Count - 1].Y);
			RefreshGraph();
		}

		public void YtoStart()
		{
			Points[SelectedPoint] = new PointD(Points[SelectedPoint].X, Points[0].Y);
			RefreshGraph();
		}

		public void AlignX()
		{
			Points[SelectedPoint] = new PointD((float)Math.Round(Points[SelectedPoint].X), Points[SelectedPoint].Y);
			RefreshGraph();
		}

		public void Reset()
		{
			InitPoints();
			RefreshGraph();
		}

		public void SetNewSize(int Width, int Height)
		{
			if (this.Width != Width || this.Height != Height)
			{
				this.Width = Width;
				this.Height = Height;
				CurveArea.Width = Width - Left - Right;
				CurveArea.Height = Height - Top - Bottom;
			}
		}

		public void CheckForPointHit()
		{
			double Xvalue = Xpos;
			double Yvalue = Ypos;

			bool HitPoint = false;
			int HitIndex = 1;
			float rX = 5;
			float rY = 5;

			for (int i = 1; i < Points.Count; i++)
			{
				PointD p = RealToGraph(Points[i]);
				if (Xvalue > p.X - rX && Xvalue < p.X + rX && Yvalue > p.Y - rY && Yvalue < p.Y + rY)
				{
					HitPoint = true;
					break;
				}
				else { HitIndex++; }
			}

			if (HitPoint)
			{
				SelectedPoint = HitIndex;
				IsMoving = true;
			}
			else
			{
				int newindex = 0;
				for (int i = 0; i < Points.Count; i++)
				{
					if (Xvalue > RealToGraph(Points[i]).X) { newindex++; }
					else { break; }
				}
				Points.Insert(newindex, GraphToReal(Xpos, Ypos));
				SelectedPoint = newindex;
				IsMoving = true;
			}
		}

		public void RemovePoint(int index)
		{
			if (index < Points.Count - 1 && index > 0)
			{
				Points.RemoveAt(index);
				SelectedPoint--;
			}
		}

		public void Dispose()
		{

		}

		public Action RefreshGraph;

		#endregion

		#region Eventhandling

		private void CurrentProject_BrightnessCalculated(object sender, WorkFinishedEventArgs e)
		{
			InitPoints();
		}

		public void Mouse_Down(double X, double Y)
		{
			Xpos = X;
			Ypos = Y;

			if (IsOnCurveArea && ProjectManager.CurrentProject.IsBrightnessCalculated)
			{
				CheckForPointHit();
				RefreshGraph();
			}
		}

		public void Mouse_Up()
		{
			if (IsOnCurveArea) { IsMoving = false; }
			else { RemovePoint(SelectedPoint); }
			RefreshGraph();
		}

		public void Mouse_Move(double X, double Y)
		{
			Xpos = X;
			Ypos = Y;

			if (IsMoving)
			{
				if (SelectedPoint > 0 && SelectedPoint < Points.Count - 1)
				{
					if (RealToGraph(Points[SelectedPoint - 1]).X + 3 < Xpos && RealToGraph(Points[SelectedPoint + 1]).X - 3 > Xpos)
					{
						Points[SelectedPoint] = GraphToReal(Xpos, Ypos);
						RefreshGraph();
					}
				}
				else if (SelectedPoint == 0 || SelectedPoint == Points.Count - 1)
				{
					if (Ypos >= 0 && Ypos < Height - Bottom)
					{
						Points[SelectedPoint] = new PointD(Points[SelectedPoint].X, GraphToReal(0, Ypos).Y);
						RefreshGraph();
					}
				}
			}
		}

		#endregion

		#region Conversion

		public PointD RealToGraph(PointD p)
		{
			double min = ProjectManager.CurrentProject.Frames.Min(b => b.AlternativeBrightness);
			double max = ProjectManager.CurrentProject.Frames.Max(b => b.AlternativeBrightness);

			double x = Left + ((p.X / (double)Pointcount) * CurveArea.Width);
			double y = Top + (((100 - (((p.Y - min) * 100) / (max - min))) * CurveArea.Height) / 100d);

			return new PointD((float)x, (float)y);
		}

		public PointD[] RealToGraph(List<PointD> plist)
		{
			return RealToGraph(plist.ToArray());
		}

		public PointD[] RealToGraph(PointD[] parr)
		{
			PointD[] output = new PointD[parr.Length];
			for (int i = 0; i < parr.Length; i++) { output[i] = RealToGraph(parr[i]); }
			return output;
		}

		public PointD GraphToReal(PointD p)
		{
			double min = ProjectManager.CurrentProject.Frames.Min(b => b.AlternativeBrightness);
			double max = ProjectManager.CurrentProject.Frames.Max(b => b.AlternativeBrightness);

			float x = (float)((Pointcount) * (p.X - Left)) / (float)CurveArea.Width;
			float y = (float)((CurveArea.Height * max) + ((Top - p.Y) * (max - min))) / (float)CurveArea.Height;

			return new PointD(x, y);
		}

		public PointD GraphToReal(double x, double y)
		{
			return GraphToReal(new PointD((float)x, (float)y));
		}

		#endregion
	}
}