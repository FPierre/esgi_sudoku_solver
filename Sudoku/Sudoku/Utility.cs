using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Utility
    {

        public static String[] SplitWithSeparatorEmpty(String s)
        {
            int number = s.Length;
            char[] c = s.ToCharArray();
            String[] stringTable = new string[number];
            for (int i = 0; i < number; i++)
            {
                stringTable[i] = "" + c[i];
            }
            return stringTable;
        }
    }
}
