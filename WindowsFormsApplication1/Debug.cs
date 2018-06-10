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

        private Maze maze;

        public Debug()
        {
            InitializeComponent();
            this.GenerateMazeLetsGo();
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

            DrawPositionIteration(g, cellHeight, cellWidth);
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

                    PointF p1;
                    PointF p2;

                    if (maze.Board[row, col].NorthWall)
                    {
                        p1 = new PointF(xCurrent, yCurrent);
                        p2 = new PointF(xCurrent + cellWidth, yCurrent);
                        g.DrawLine(wall, p1, p2);
                    }
                    if (maze.Board[row, col].EastWall)
                    {
                        p1 = new PointF(xCurrent + cellWidth, yCurrent);
                        p2 = new PointF(xCurrent + cellWidth, yCurrent + cellHeight);
                        g.DrawLine(wall, p1, p2);
                    }
                    if (maze.Board[row, col].SouthWall)
                    {
                        p1 = new PointF(xCurrent, yCurrent + cellHeight);
                        p2 = new PointF(xCurrent + cellWidth, yCurrent + cellHeight);
                        g.DrawLine(wall, p1, p2);
                    }
                    if (maze.Board[row, col].WestWall)
                    {
                        p1 = new PointF(xCurrent, yCurrent);
                        p2 = new PointF(xCurrent, yCurrent + cellHeight);
                        g.DrawLine(wall, p1, p2);
                    }
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
    }
}
