using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {


        private DateTime _start;
        int charctr = 1;
        int dialoguectr = 0;
        int Stage = 0;
        public Form1()
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

                GlobalVariables.IsIntroString = false;
                IntroString.Visible = false;
                MainPanel.Visible = true;
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
    }
}
