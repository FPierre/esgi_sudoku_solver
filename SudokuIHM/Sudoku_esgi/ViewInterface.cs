using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_esgi
{
    public class ViewObject
    {

        public static bool StepByStep = false;
        public static ModeText mode = ModeText.Warning;
        protected StringBuilder result;

        public ViewObject()
        {
            result = new StringBuilder();
        }

    }
}
