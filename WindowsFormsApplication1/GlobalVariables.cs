using WindowsFormsApplication1.Class;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;
using System.Text;


namespace WindowsFormsApplication1
{
    class GlobalVariables
    {
        static string _PlayerName;
        public static string PlayerName
        {
            get { return _PlayerName; }
            set { _PlayerName = value; }
        }

        static int _PlayerNameLength;
        public static int PlayerNameLength
        {
            get { return _PlayerNameLength; }
            set { _PlayerNameLength = value; }
        }

        static bool _PlayerWithName;
        public static bool PlayerWithName
        {
            get { return _PlayerWithName; }
            set { _PlayerWithName = value; }
        }

        #region Maze Variables
        
        
        
        static int _Height;
        public static int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        
        static int _Width;
        public static int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        

        #endregion
        #region Dialogue

        public static string IntroText = "Hello...";

       static bool _IsIntroString;
       public static bool IsIntroString
       {
           get { return _IsIntroString; }
           set { _IsIntroString = value; }
       }

      
       public static string[,] IntroDialogue = new string[5,4]{
           {"Hello there...",
            "0",
            "0",
            "0"},
           
           {"You're probably wondering why you're here",
           "0",
           "0",
           "0"},  
        
           {"You've been sent on a...special trip",
            "300",
            "21",
            "24"},
            
            {"I know what you're thinking...",
            "0",
            "0",
            "0"},

            {"Before you can ask questions...i'd like to know your name",
            "150",
            "28",
            "31"}
       };



       public static string[,] DialogueNoName = new string[5, 4]{
           {"Oh...so you dont have a name then?",
           "300",
           "2",
           "5"},

           {"Or is it because you just dont want to tell me?",
           "0",
           "0",
           "0"},

           {"Alright, i cant force you to do anything. Not in here at least",
           "300",
           "41",
           "43"},

           {"I'll just call you Contestant...Player?", 
           "200",
           "29",
           "32"
           },
           
           {"Whatever floats my boat",
           "0",
           "0",
           "0"}
        };

       public static string[,] DialogueWithName = new string[5, 4]
        {
           {""+PlayerName+"...i like that",
           "300",
           "2",
           ""+PlayerNameLength},

           {"Since you've obliged so easily. You deserve a reward",
           "0",
           "0",
           "0"},

           {"The freedom to ask questions...Though dont confuse liberty and freedom in this context",
           "300",
           "28",
           "32"},

           {"You are free to ask questions, though the liberaty of asking any question is limited.",
           "0",
           "0",
           "0",},

           {"I will only answer questions i am programmed to",
           "0",
           "0",
           "0"}

        };

        #endregion
    }
}
