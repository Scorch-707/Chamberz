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
        Point currentPos = new Point();
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
            GlobalVariables.Height = 7;
            GlobalVariables.Width = 7;
            maze = new Maze(GlobalVariables.Height, GlobalVariables.Width);
            maze.Generate();

            currentPos.X = maze.Start.X;
            currentPos.Y = maze.Start.Y;
            
            refreshGrid();
            updatePosition();
            
            IntroString.Visible = false;
            MainPanel.Visible = true;
        }

        #endregion


        private void debug_Click(object sender, EventArgs e)
        {
            Debug obj = new Debug(maze);
            obj.Show();
        }

        private void updatePosition()
        {
            int startCellNumber = currentPos.X + ((currentPos.Y) * GlobalVariables.Width);
            string startCellName = "Cell" + (startCellNumber + 1);
            Button startCell = this.Controls.Find(startCellName, true).FirstOrDefault() as Button;
            startCell.Visible = true;
            startCell.Enabled = true;
            MessageBox.Show("Current Position X: " + currentPos.X + "\n Current Position Y: " + currentPos.Y);
        }

        private void refreshGrid()
        {
            for (int x = 1; x <= 49; x++)
            {
                string disableCellName = "Cell" + x;
                Button disableCell = this.Controls.Find(disableCellName, true).FirstOrDefault() as Button;
                disableCell.Visible = true;
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

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            int up, down, left, right;
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {            
                up = currentPos.Y - 1; //Going up

                if (up >= 0)
                {
                    if (maze.Board[currentPos.X, currentPos.Y].NorthWall)
                    {
                        MessageBox.Show("You've reached a wall going up");
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
                    MessageBox.Show("You've reach a wall going up (end wall)");
                }
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                left = currentPos.X - 1; //Going up
                
                if (left >= 0)
                {
                    if (maze.Board[currentPos.X, currentPos.Y].WestWall)
                    {
                        MessageBox.Show("You've reached a wall going left");
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
                    MessageBox.Show("You've reach a wall going left (end wall)");               
                }
            }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                down = currentPos.Y + 1;

                if (down <= GlobalVariables.Height-1)
                {
                    if (maze.Board[currentPos.X, currentPos.Y].SouthWall)
                    {
                        MessageBox.Show("You've reached a wall going down");
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
                    MessageBox.Show("You've reach a wall going down (end wall)");
                }
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                right = currentPos.X + 1;

                if (right <= (GlobalVariables.Width)
                {
                    if (maze.Board[currentPos.X, currentPos.Y].EastWall)
                    {
                        MessageBox.Show("You've reached a wall going right");
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
                    MessageBox.Show("You've reach a wall going right (end wall)");
                }
            }
        }

        #endregion

        private void PingPosition_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Current Position X: " + currentPos.X + "\n Current Position Y: " + currentPos.Y);
        }

      

    }
}
