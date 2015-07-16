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
            ConsoleMenu console = new ConsoleMenu();
            SudokuManager = new SudokuManager(selectFile(), console);
        }

        public static string selectFile() {
            return @"../../../Fichier_sudokus_a_resoudre.sud";
        }
    }
}
