using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.Class;

namespace WindowsFormsApplication1
{
    public partial class Debug : Form
    {
        bool showPositionIter = false;
        bool showLightTesting = false;

        private Maze maze;

        //wall points
        List<PointF[,]> segment = new List<PointF[,]>();

        //ray data
        List<PointF> points = new List<PointF>();
        List<PointF> uniquePoints = new List<PointF>();
        List<double> uniqueAngles = new List<double>();
        List<PointF[]> rays = new List<PointF[]>();
        List<Tuple<PointF, float>> intersects = new List<Tuple<PointF, float>>();

        public Debug()
        {
            InitializeComponent();
            this.GenerateMazeLetsGo();
            this.DoubleBuffered = true;
        }

        public Debug(Maze _maze)
        {
            maze = _maze;
            InitializeComponent();
            panel1.Invalidate();
            draw_output();
        }
        /// <summary>
        /// Draws the grid and walls of the field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen grid = new Pen(Color.LightCyan, 3);
            Pen wall = new Pen(Color.Black, 5);
            Brush brush_start = new SolidBrush(Color.LightGoldenrodYellow);
            Brush brush_end = new SolidBrush(Color.LightSeaGreen);
            int cellHeight = panel1.Height / GlobalVariables.Height;
            int cellWidth = panel1.Width / GlobalVariables.Width;


            DrawGrid(g, grid, cellHeight, cellWidth);
            DrawStart(g, brush_start, maze.Start, cellHeight, cellWidth);
            DrawDeadEnd(g, brush_end, cellHeight, cellWidth);

            DrawWalls(g, wall, cellHeight, cellWidth);
            DrawRays(drawOnPanel2());
            if (showPositionIter)
            {
                DrawPositionIteration(g, cellHeight, cellWidth);
            }
        }

        public Graphics drawOnPanel2()
        {
            Graphics g = panel2.CreateGraphics();
            Pen grid = new Pen(Color.LightCyan, 3);
            Pen wall = new Pen(Color.Black, 5);
            Brush brush_start = new SolidBrush(Color.LightGoldenrodYellow);
            Brush brush_end = new SolidBrush(Color.LightSeaGreen);
            int cellHeight = panel1.Height / GlobalVariables.Height;
            int cellWidth = panel1.Width / GlobalVariables.Width;


            DrawGrid(g, grid, cellHeight, cellWidth);
            DrawStart(g, brush_start, maze.Start, cellHeight, cellWidth);
            DrawDeadEnd(g, brush_end, cellHeight, cellWidth);

            DrawWalls(g, wall, cellHeight, cellWidth);

            return g;
        }
        private void DrawRays(Graphics g)
        {
            float radius = 10;
            float x = GlobalVariables.MousePos.X - radius;
            float y = GlobalVariables.MousePos.Y - radius;
            float height = 2 * radius;
            float width = 2 * radius;
            PointF[,] point;
            List<PointF> seg = new List<PointF>();
            double angle;
            Tuple<PointF, float> intersect = new Tuple<PointF, float>(new PointF(), new float());
            Tuple<PointF, float> closestIntersect = new Tuple<PointF, float>(new PointF(), new float());

            //draw circle
            g.FillEllipse(Brushes.Aqua, x, y, height, width);

            //reset lists
            points.Clear();
            uniquePoints.Clear();
            uniqueAngles.Clear();
            intersects.Clear();
            rays.Clear();

            for (int ctr = 0; ctr < segment.Count; ctr++)
            {
                point = segment[ctr];
                for (int ctrX = 0; ctrX < point.GetLength(0); ctrX++)
                {
                    //draw segments/polygons 
                    g.DrawLine(Pens.Aqua, point[ctrX, 0], point[ctrX, 1]);

                    for (int ctrY = 0; ctrY < point.GetLength(1); ctrY++)
                    {
                        //get all points
                        points.Add(new PointF(point[ctrX, ctrY].X, point[ctrX, ctrY].Y));

                        if (!(uniquePoints.Contains(point[ctrX, ctrY])))
                        {

                            //get unique points
                            uniquePoints.Add(new PointF(point[ctrX, ctrY].X, point[ctrX, ctrY].Y));

                            //get unique angle
                            angle = Math.Atan2(point[ctrX, ctrY].Y - GlobalVariables.MousePos.Y, point[ctrX, ctrY].X - GlobalVariables.MousePos.X);

                            uniqueAngles.Add(angle - 0.00001);
                            uniqueAngles.Add(angle);
                            uniqueAngles.Add(angle + 0.00001);
                        }
                    }
                }
            }

            //Rays in all direction
            int c = 0;
            for (int anglectr = 0; anglectr < uniqueAngles.Count; anglectr++)
            {
                angle = uniqueAngles[anglectr];

                double dx = Math.Cos(angle);
                double dy = Math.Sin(angle);

                PointF[] ray = new PointF[2]{
                        new PointF {X = GlobalVariables.MousePos.X, Y = GlobalVariables.MousePos.Y},  
                        new PointF(GlobalVariables.MousePos.X+(float)dx, GlobalVariables.MousePos.Y+(float)dy), 
                };

                rays.Add(ray);

                closestIntersect = null;
                for (int ctr = 0; ctr < segment.Count; ctr++)
                {
                    point = segment[ctr];
                    for (int ctrX = 0; ctrX < point.GetLength(0); ctrX++)
                    {
                        seg.Clear();
                        seg.Add(new PointF(point[ctrX, 0].X, point[ctrX, 0].Y));
                        seg.Add(new PointF(point[ctrX, 1].X, point[ctrX, 1].Y));

                        intersect = getIntersection(ray, seg.ToArray());
                        if (intersect == null) continue;

                        if (closestIntersect == null || intersect.Item2 < closestIntersect.Item2)
                        {
                            closestIntersect = intersect;
                        }
                    }
                }
                //add to list of intersects
                intersects.Add(closestIntersect);
            }

            //draw rays 
            for (int ctr = 0; ctr < intersects.Count; ctr++)
            {
                intersect = intersects[ctr];
                SizeF size = new SizeF(5, 5);
                var solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                RectangleF rec = new RectangleF(intersect.Item1, size);
                g.DrawLine(Pens.Black, new PointF(GlobalVariables.MousePos.X, GlobalVariables.MousePos.Y), intersect.Item1);
            }

            //debug intersects
        }

        
        // Find intersection of RAY & SEGMENT
        public Tuple<PointF, float> getIntersection(PointF[] ray, PointF[] segment)
        {
            Tuple<PointF, float> returnset = new Tuple<PointF, float>(new PointF(), new float());
            // RAY in parametric: PointF + Delta*T1
            var r_px = ray[0].X;
            var r_py = ray[0].Y;
            var r_dx = ray[1].X - ray[0].X;
            var r_dy = ray[1].Y - ray[0].Y;

            // SEGMENT in parametric: PointF + Delta*T2
            var s_px = segment[0].X;
            var s_py = segment[0].Y;
            var s_dx = segment[1].X - segment[0].X;
            var s_dy = segment[1].Y - segment[0].Y;

            //Are they parallel? If so, no intersect
            var r_mag = Math.Sqrt(r_dx * r_dx) + (r_dy * r_dy);
            var s_mag = Math.Sqrt(s_dx * s_dx) + (s_dy * s_dy);

            if (r_dx / r_mag == s_dx / s_mag && r_dy / r_mag == s_dy / s_mag)
            {
                return null;
            }

            var T2 = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx);
            var T1 = (s_px + s_dx * T2 - r_px) / r_dx;

            if (T1 < 0) return null;
            if (T2 < 0 || T2 > 1) return null;

            var X = r_px + r_dx * T1;
            var Y = r_py + r_dy * T1;

            PointF a = new PointF((float)X, (float)Y);

            //Tuple<PointF, double> returnset = new Tuple<PointF, double>(new PointF((int)Math.Ceiling(X), (int)Math.Ceiling(Y)), T1);
            returnset = new Tuple<PointF, float>(a, T1);

            return returnset;
        }

        private void DrawPositionIteration(Graphics g, int cellHeight, int cellWidth)
        {
            Font drawFont = new Font("Arial", cellHeight / 7);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            foreach (Cell item in maze.Board)
            {
                RectangleF drawRect = new RectangleF((cellWidth * item.Point.X) + (cellWidth / 3), (cellHeight * item.Point.Y) + (cellHeight / 3), cellWidth / 2, cellHeight / 2);
                g.DrawString(item.position_in_iteration.ToString(), drawFont, drawBrush, drawRect);
            }

        }

        private void DrawDeadEnd(Graphics g, Brush brush, int cellHeight, int cellWidth)
        {
            foreach (Cell item in maze.Board)
            {
                if (item.isdeadend)
                {
                    g.FillRectangle(brush, cellWidth * item.Point.X, cellHeight * item.Point.Y, cellWidth, cellHeight);
                }
            }
        }

        private void DrawStart(Graphics g, Brush brush_start, Point start, int cellHeight, int cellWidth)
        {
            //g.FillEllipse(brush_start, (cellWidth * start.X ) + (cellWidth / 4), (cellHeight * start.Y ) + (cellHeight / 4), cellWidth / 2, cellHeight / 2);
            g.FillRectangle(brush_start, cellWidth * start.X, cellHeight * start.Y, cellWidth, cellHeight);
        }

        private void DrawGrid(Graphics g, Pen grid, int cellHeight, int cellWidth)
        {
            for (int row = 1; row < GlobalVariables.Height; row++)
            {
                g.DrawLine(grid, 0, cellHeight * row, panel1.Width, cellHeight * row);
            }

            for (int col = 1; col < GlobalVariables.Width; col++)
            {
                g.DrawLine(grid, cellWidth * col, 0, cellWidth * col, panel1.Height);
            }
        }

        private void DrawWalls(Graphics g, Pen wall, int cellHeight, int cellWidth)
        {
            for (int row = 0; row < GlobalVariables.Height; row++)
            {
                for (int col = 0; col < GlobalVariables.Width; col++)
                {
                    //Calculate current positions
                    int xCurrent = cellWidth * col;
                    int yCurrent = cellHeight * row;

                    int rowctr = 0;
                    List<PointF> pt1 = new List<PointF>();
                    List<PointF> pt2 = new List<PointF>();
                    PointF p1;
                    PointF p2;

                    if (maze.Board[row, col].NorthWall)
                    {
                        p1 = new PointF(xCurrent, yCurrent);
                        p2 = new PointF(xCurrent + cellWidth, yCurrent);
                        pt1.Add(p1);
                        pt2.Add(p2);
                        rowctr++;
                        g.DrawLine(wall, p1, p2);
                        
                    }
                    if (maze.Board[row, col].EastWall)
                    {
                        p1 = new PointF(xCurrent + cellWidth, yCurrent);
                        p2 = new PointF(xCurrent + cellWidth, yCurrent + cellHeight);
                        pt1.Add(p1);
                        pt2.Add(p2);
                        g.DrawLine(wall, p1, p2);
                        rowctr++;
                    }
                    if (maze.Board[row, col].SouthWall)
                    {
                        p1 = new PointF(xCurrent, yCurrent + cellHeight);
                        p2 = new PointF(xCurrent + cellWidth, yCurrent + cellHeight);
                        pt1.Add(p1);
                        pt2.Add(p2);
                        g.DrawLine(wall, p1, p2);
                        rowctr++;
                    }
                    if (maze.Board[row, col].WestWall)
                    {
                        p1 = new PointF(xCurrent, yCurrent);
                        p2 = new PointF(xCurrent, yCurrent + cellHeight);
                        pt1.Add(p1);
                        pt2.Add(p2);
                        g.DrawLine(wall, p1, p2);
                        rowctr++;
                    }

                    PointF[,] wallgrid = new PointF[rowctr+1, 2];
                    for (int x = 0; x < rowctr; x++)
                    {
                        wallgrid[x, 0] = new PointF(pt1[x].X, pt1[x].Y);
                        wallgrid[x, 1] = new PointF(pt2[x].X, pt2[x].Y);
                    }

                    segment.Add(wallgrid);
                }
            }
        }

        /// <summary>
        /// Opens a prompt, where the user can change the field dimensions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeDimensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ChangeSizeForm f2 = new ChangeSizeForm();
            //Show.f2.FormClosed += ChangeSizeForm_FormClosed;
            //f2.Show();
        }

        private void ChangeSizeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GenerateMazeLetsGo();
        }

        private void draw_output()
        {
            this.txt_output.Text = "";
            foreach (Tuple<Cell, Direction> item in maze.Points)
            {
                this.txt_output.Text += item.Item1.position_in_iteration.ToString() + ": " + item.Item1.Point.X + " / " + item.Item1.Point.Y + " - " + item.Item2.ToString() + " - visited: " + item.Item1.visited_count + Environment.NewLine;
            }
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateMazeLetsGo();
        }

        /// <summary>
        /// This method starts the generation and paint process of the new maze.
        /// </summary>
        public void GenerateMazeLetsGo()
        {

            maze = new Maze(GlobalVariables.Height, GlobalVariables.Width);
            maze.Generate();
            panel1.Invalidate();
            draw_output();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Invalidate();
            draw_output();
        }

        private void showPositionIterationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showPositionIter)
            {
                showPositionIter = false;
                showPositionIterationToolStripMenuItem.Text = "Show Position Iteration";
            }
            else if (!(showPositionIter))
            {
                showPositionIter = true;
                showPositionIterationToolStripMenuItem.Text = "Disable Position Iteration";
            }
      
            this.panel1.Invalidate();
            panel1.Visible = true;
            panel2.Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawRays(g);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
        
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            PointF mouse = PointToClient(MousePosition);
            GlobalVariables.MousePos = new PointF(mouse.X, mouse.Y);
            this.panel2.Invalidate();
        }

        private void lightTestingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
        }
    }
}
