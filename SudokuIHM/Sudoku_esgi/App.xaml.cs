using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku_esgi {
    public partial class App : Application {
        public static SudokuManager SudokuManager { get; set; }
        static App() {
            SudokuManager = new SudokuManager();
        }
    }
}
