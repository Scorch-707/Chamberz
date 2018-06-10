﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1.Class
{
    public class Maze
    {
        public Cell[,] Board;
        private readonly Random rng = new Random();
        public Point Start = new Point(0, 0);
        public Point End = new Point(0, 0);
        public List<Tuple<Cell, Direction>> Points = new List<Tuple<Cell, Direction>>();
        public int iterationcount = 0;
    
        public Maze(int width, int height)
        {
            GlobalVariables.Height = height;
            GlobalVariables.Width = width;
            Board = new Cell[height, width];
            Initialise();
        }

        public void Generate()
        {
            Generate(rng.Next(GlobalVariables.Width), rng.Next(GlobalVariables.Height));
        }

        public void Generate(int startX, int startY)
        {
            this.Start = new Point(startX, startY);
            Points = new List<Tuple<Cell, Direction>>();
            CarvePassage(startX, startY);
        }

        /// <summary>
        /// Populates board with default Cells.
        /// </summary>
        private void Initialise()
        {
            for (int row = 0; row < GlobalVariables.Height; row++)
            {
                for (int col = 0; col < GlobalVariables.Width; col++)
                {
                    this.Board[row, col] = new Cell();
                }
            }
        }

        /// <summary>
        /// Recursive backtracking maze generation algorithm.
        /// </summary>
        /// <param name="currentPos"></param>
        private void CarvePassage(Point currentPos)
        {

            this.Board[currentPos.Y, currentPos.X].Point = new Point(currentPos.X, currentPos.Y);
            this.Board[currentPos.Y, currentPos.X].Visited = true;
            this.Board[currentPos.Y, currentPos.X].position_in_iteration = ++iterationcount;

            List<Direction> validDirections = GetAllDirections();
            ValidateDirections(currentPos, validDirections);

            //If there is no valid direction we have found a dead end.
            if (validDirections.Count == 0)
            {
                this.Board[currentPos.Y, currentPos.X].isdeadend = true;
                Points.Add(new Tuple<Cell, Direction>(this.Board[currentPos.Y, currentPos.X], Direction.Invalid));
            }

            while (validDirections.Count > 0)
            {
                Direction rndDirection = Direction.Invalid;

                if (validDirections.Count > 1)
                    rndDirection = validDirections[rng.Next(validDirections.Count)];
                else if (validDirections.Count == 1)
                    rndDirection = validDirections[0];

                this.Board[currentPos.Y, currentPos.X].visited_count = ++this.Board[currentPos.Y, currentPos.X].visited_count;

                RemoveWall(currentPos, rndDirection);
                validDirections.Remove(rndDirection);
                Point newPos = GetAdjPos(currentPos, rndDirection);
                Points.Add(new Tuple<Cell, Direction>(this.Board[currentPos.Y, currentPos.X], rndDirection));

                CarvePassage(newPos);

                ValidateDirections(currentPos, validDirections);
            }
        }

        private void CarvePassage(int currentX, int currentY)
        {
            CarvePassage(new Point(currentX, currentY));
        }

        private List<Direction> GetAllDirections()
        {
            return new List<Direction>() {
                Direction.North,
                Direction.East,
                Direction.South,
                Direction.West
            };
        }

        private void ValidateDirections(Point cellPos, List<Direction> directions)
        {
            List<Direction> invalidDirections = new List<Direction>();

            // Check for invalid moves
            for (int i = 0; i < directions.Count; i++)
            {
                switch (directions[i])
                {
                    case Direction.North:
                        if (cellPos.Y == 0 || CellVisited(cellPos.X, cellPos.Y - 1))
                            invalidDirections.Add(Direction.North);
                        break;
                    case Direction.East:
                        if (cellPos.X == GlobalVariables.Width - 1 || CellVisited(cellPos.X + 1, cellPos.Y))
                            invalidDirections.Add(Direction.East);
                        break;
                    case Direction.South:
                        if (cellPos.Y == GlobalVariables.Height - 1 || CellVisited(cellPos.X, cellPos.Y + 1))
                            invalidDirections.Add(Direction.South);
                        break;
                    case Direction.West:
                        if (cellPos.X == 0 || CellVisited(cellPos.X - 1, cellPos.Y))
                            invalidDirections.Add(Direction.West);
                        break;
                }
            }

            // Eliminating invalid moves
            foreach (var item in invalidDirections)
                directions.Remove(item);
        }

        private void RemoveWall(Point pos, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    this.Board[pos.Y, pos.X].NorthWall = false;
                    this.Board[pos.Y - 1, pos.X].SouthWall = false;
                    break;
                case Direction.East:
                    this.Board[pos.Y, pos.X].EastWall = false;
                    this.Board[pos.Y, pos.X + 1].WestWall = false;
                    break;
                case Direction.South:
                    this.Board[pos.Y, pos.X].SouthWall = false;
                    this.Board[pos.Y + 1, pos.X].NorthWall = false;
                    break;
                case Direction.West:
                    this.Board[pos.Y, pos.X].WestWall = false;
                    this.Board[pos.Y, pos.X - 1].EastWall = false;
                    break;
            }
        }

        private bool CellVisited(int x, int y)
        {
            return this.Board[y, x].Visited;
        }

        private Point GetAdjPos(Point position, Direction direction)
        {
            Point adjPosition = position;

            switch (direction)
            {
                case Direction.North:
                    adjPosition.Y = adjPosition.Y - 1;
                    break;
                case Direction.East:
                    adjPosition.X = adjPosition.X + 1;
                    break;
                case Direction.South:
                    adjPosition.Y = adjPosition.Y + 1;
                    break;
                case Direction.West:
                    adjPosition.X = adjPosition.X - 1;
                    break;
            }

            return adjPosition;
        }


    }
}
