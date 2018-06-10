using WindowsFormsApplication1.Class;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;
using System.Text;


namespace WindowsFormsApplication1
{
    class GlobalFunctions
    {      
        public static bool IsNullOrWhiteSpace(string value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    
     


    }
}
