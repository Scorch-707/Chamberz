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
    public partial class Main : Form
    {

        private Maze maze;
        private DateTime _start;
        private List<Tuple<Cell, Direction>> item = new List<Tuple<Cell, Direction>>();
        Point currentPos = new Point();
        Point[] GameBoard = new Point[49];
        int charctr = 1;
        int dialoguectr = 0;
        int Stage = 3;


        public Main()
        {

            InitializeComponent();
            MainPanel.Visible = false;

            _start = DateTime.Now;
            timer1.Interval = 50;
            timer1.Start();
           
        }     

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (Stage == 0)
            {
                GlobalVariables.IsIntroString = true;
                if (dialoguectr < 5)
                {
                    Announcer(GlobalVariables.IntroDialogue, GlobalVariables.IntroDialogue[dialoguectr, 0], charctr);
                    charctr++;
                }
               
            }
            else if (Stage == 2)
            {
                if (dialoguectr < 5)
                {
                    if (GlobalVariables.PlayerWithName)
                    {
                        Announcer(GlobalVariables.DialogueWithName, GlobalVariables.DialogueWithName[dialoguectr, 0], charctr);
                        charctr++;

                    }
                    else
                    {
                        Announcer(GlobalVariables.DialogueNoName, GlobalVariables.DialogueNoName[dialoguectr, 0], charctr);
                        charctr++;
                    }
                }
               
            }
          
        }


        #region Text Manipulator

        
        public void Announcer(string[,] TextArray, string Text, int sec)
        {
            if (Text.Length >= sec)
            {
                if (Int32.Parse(TextArray[dialoguectr, 1]) > 0)
                {

                    if (sec >= Int32.Parse(TextArray[dialoguectr, 2]))
                    {
                        Stall(Text, sec, Int32.Parse(TextArray[dialoguectr, 1]), Int32.Parse(TextArray[dialoguectr, 2]), Int32.Parse(TextArray[dialoguectr, 3]));
                    }
                    else
                    {
                        TextEater(Text, sec, GlobalVariables.IsIntroString);
                    }
                }
                else
                {
                    TextEater(Text, sec, GlobalVariables.IsIntroString);
                }
            }
            else
            {
                //Skip ME
            }
        }

        private void Stall(String Text, int sec, int value, int start, int end)
        {
            if (sec < end)
            {
                timer1.Interval = value;
            }
            else if(sec > end)
            {
                timer1.Interval = 50;
            }
            TextEater(Text, sec, GlobalVariables.IsIntroString);
        }

        public void TextEater(string Text, int sec, bool isIntroString)
        {
            char[] szArr = Text.ToCharArray();
            StringBuilder sb = new StringBuilder();
            String Dialogue;
            for (int x = 0; x < sec; x++)
            {
                sb.Append(szArr[x]);
            }
            Dialogue = sb.ToString();
            if (isIntroString)
            {
                IntroString.Text = Dialogue;
            }
            else
            {

            }
        }

        #endregion

        #region Intro Codes

        private void IntroString_MouseClick(object sender, MouseEventArgs e)
        {
            timer1.Stop();
            timer1.Interval = 50;
            if (Stage == 0)
            {
                if (dialoguectr < 5)
                {
                    charctr = 1;
                    dialoguectr++;
                    timer1.Start();
                }
                else if (dialoguectr >= 5)
                {
                    Stage++;
                }
            }       
            if(Stage == 1)
            {             
                    IntroString.Visible = false;
                    NameField.Visible = true;
                    NameButton.Visible = true;
            }
            if (Stage == 2)
            {
                if (dialoguectr < 5)
                {
                    charctr = 1;
                    dialoguectr++;
                    timer1.Start();
                }
                else if (dialoguectr >= 5)
                {
                    Stage++;
                }
            }

            if (Stage == 3)
            {
                StartMaze();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            IntroString.Visible = true;
            NameField.Visible = false;
            NameButton.Visible = false;
            timer1.Start();
            dialoguectr = 0;
            GlobalVariables.PlayerNameLength = NameField.Text.Length;

            if(GlobalFunctions.IsNullOrWhiteSpace(NameField.Text))
            {
                GlobalVariables.PlayerName = "No name";
                GlobalVariables.PlayerWithName = false;
                Announcer(GlobalVariables.DialogueNoName, GlobalVariables.DialogueNoName[dialoguectr, 0], charctr);
                charctr++;
            }
            else if(!(GlobalFunctions.IsNullOrWhiteSpace(NameField.Text)))
            {
                GlobalVariables.PlayerName = NameField.Text;
                GlobalVariables.PlayerWithName = true;
                GlobalVariables.DialogueWithName[0, 0] = "" + GlobalVariables.PlayerName + "...I like that";
                GlobalVariables.DialogueWithName[0, 3] = "" + (GlobalVariables.PlayerNameLength + 3); 
                Announcer(GlobalVariables.DialogueWithName, GlobalVariables.DialogueWithName[dialoguectr, 0], charctr);
                charctr++;
            }
            Stage++;
        }

        public void StartMaze()
        {
            GlobalVariables.IsIntroString = false;
            GlobalVariables.Height = 10;
            GlobalVariables.Width = 10;
            GlobalVariables.DetectRange = 3;
            maze = new Maze(GlobalVariables.Height, GlobalVariables.Width);
            maze.Generate();

            currentPos.X = maze.Start.X;
            currentPos.Y = maze.Start.Y;
            
            refreshGrid();
            setGameBoard();
            updatePosition();
            
            IntroString.Visible = false;
            MainPanel.Visible = true;
        }

        #endregion


        private void setGameBoard()
        {
            int totalctr = 0;
            int XMinRange = currentPos.X - 3;
            int XMaxRange = currentPos.X + 4;
            int YMinRange = currentPos.Y - 3;
            int YMaxRange = currentPos.Y + 4;

            for (int y = YMinRange; y < YMaxRange; y++)
            {
                for (int x = XMinRange; x < XMaxRange; x++)            
                {
                    GameBoard[totalctr].X = x;
                    GameBoard[totalctr].Y = y;
                    totalctr++;
                }
            }
        }

        private void updatePosition()
        {
            setGameBoard();
              
            for (int ctr = 0; ctr < 49; ctr++)
            {
                    string gridName = "Grid" + (ctr + 1);
                    Panel grid = this.Controls.Find(gridName, true).FirstOrDefault() as Panel;
           
                    string disableCellName = "Cell" + (ctr + 1);

             
                    if (GameBoard[ctr].X >= 0 && GameBoard[ctr].Y >= 0 && GameBoard[ctr].X < GlobalVariables.Width && GameBoard[ctr].Y < GlobalVariables.Height)
                    {
                        Panel disableCell = this.Controls.Find(disableCellName, true).FirstOrDefault() as Panel;
                        disableCell.Visible = true;
                        disableCell.BackColor = System.Drawing.SystemColors.AppWorkspace;
                        disableCell.Enabled = false;
                        DrawWalls(GameBoard[ctr].X, GameBoard[ctr].Y, grid);
                    }
                    if (GameBoard[ctr].X < 0 || GameBoard[ctr].Y < 0 || GameBoard[ctr].X >= GlobalVariables.Width || GameBoard[ctr].Y >= GlobalVariables.Height)
                    {
                        Panel disableCell = this.Controls.Find(disableCellName, true).FirstOrDefault() as Panel;
                        disableCell.Visible = true;
                        disableCell.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                        disableCell.Enabled = false;
                        grid.Padding = new Padding(0, 0, 0, 0);
                    }
            }
            

            Cell25.Visible = true;
            Cell25.BackColor = System.Drawing.SystemColors.MenuHighlight;
            Cell25.Enabled = true;

            DetectPath();
        }

        public void DetectPath()
        {
           
                if (!(maze.Points[getPosition(currentPos.X, currentPos.Y)].Item1.NorthWall))
                {
                    for (int x = 0; x < GlobalVariables.DetectRange; x++)
                    {

                    }
                }
                if (!(maze.Points[getPosition(currentPos.X, currentPos.Y)].Item1.EastWall))
                {

                }
                if (!(maze.Points[getPosition(currentPos.X, currentPos.Y)].Item1.SouthWall))
                {

                }
                if (!(maze.Points[getPosition(currentPos.X, currentPos.Y)].Item1.WestWall))
                {

                }
            
        }

        public void DrawWalls(int x, int y, Panel grid) 
        {
            int North = 0, East = 0, South = 0, West = 0; 
              
            if (maze.Points[getPosition(x,y)].Item1.NorthWall)
            {  
                North = 2;          
            }
            if (maze.Points[getPosition(x,y)].Item1.EastWall)
            {
                East = 2;
            }
            if (maze.Points[getPosition(x,y)].Item1.SouthWall)
            {
                South = 2;
            }
            if (maze.Points[getPosition(x,y)].Item1.WestWall)
            {
                West = 2;
            }

            grid.Padding = new Padding(West,North,East,South);
        }
        private void refreshGrid()
        {
            for (int x = 1; x <= 49; x++)
            {
                string disableCellName = "Cell" + x;
                Panel disableCell = this.Controls.Find(disableCellName, true).FirstOrDefault() as Panel;
                disableCell.Visible = true;
                disableCell.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                disableCell.Enabled = false;
            }
        }
        
       
        #region Key bindings
        
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    if (e.Shift)
                    {

                    }
                    else
                    {
                    }
                    break;
            }
        }

        private int getPosition(int x, int y)
        {
            int cellnum = 0;
         
            foreach (Tuple<Cell, Direction> item in maze.Points)
            {
                if (item.Item1.Point.X == x && item.Item1.Point.Y == y)
                {
                    return cellnum;
                }
                else
                    cellnum++;
            }
            return cellnum;
        }
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            int up, down, left, right;
            

            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                up = currentPos.Y - 1; //Going up

                if (up >= 0)
                {
                    if (maze.Points[getPosition(currentPos.X, currentPos.Y)].Item1.NorthWall)
                    {
                        MessageBox.Show("You've bumped to a wall");
                    }
                    else
                    {
                        currentPos.Y = up;
                        refreshGrid();
                        updatePosition();
                    }
                }
                else
                {
                    MessageBox.Show("You've bumped to a wall");
                }
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                left = currentPos.X - 1; //Going up

                if (left >= 0)
                {
                    if (maze.Points[getPosition(currentPos.X, currentPos.Y)].Item1.WestWall)
                    {
                        MessageBox.Show("You've bumped to a wall");
                    }
                    else
                    {
                        currentPos.X = left;
                        refreshGrid();
                        updatePosition();
                    }
                }
                else
                {
                    MessageBox.Show("You've bumped to a wall");
                }
            }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                down = currentPos.Y + 1;

                if (down < GlobalVariables.Height)
                {
                    if (maze.Points[getPosition(currentPos.X, currentPos.Y)].Item1.SouthWall)
                    {
                        MessageBox.Show("You've bumped to a wall");
                    }
                    else
                    {
                        currentPos.Y = down;
                        refreshGrid();
                        updatePosition();
                    }
                }
                else
                {
                    MessageBox.Show("You've bumped to a wall");
                }
                
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                right = currentPos.X + 1;

                if (right < GlobalVariables.Width)
                {
                    if (maze.Points[getPosition(currentPos.X, currentPos.Y)].Item1.EastWall)
                    {
                        MessageBox.Show("You've bumped to a wall");
                    }
                    else
                    {
                        currentPos.X = right;
                        refreshGrid();
                        updatePosition();
                    }
                }
                else
                {
                    MessageBox.Show("You've bumped to a wall");
                }
            }
        }

        #endregion


        #region Debug Options 

        private void debug_Click(object sender, EventArgs e)
        {
            Debug obj = new Debug(maze);
            obj.Show();
        }

        private void PingPosition_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Current Position X: " + currentPos.X + "\n Current Position Y: " + currentPos.Y);
        }

        private void lightTest_Click(object sender, EventArgs e)
        {
            LightTest obj = new LightTest();
            obj.Show();
        }

        
        private void gameBoardArray_Click(object sender, EventArgs e)
        {
            string message = "";
            int ctr = 0;
            int subctr = 1;

            setGameBoard();
            
            foreach(Point point in GameBoard)
            {
                
                message += "["+ctr+"]: "+point.X+","+point.Y+"  ";
                if (subctr == 7)
                {
                    message += "\n";
                    subctr = 0;
                }
                subctr++;
                ctr++;

            }
            MessageBox.Show(message);

        }
        #endregion
    }
}
