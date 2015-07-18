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
        public static ConsoleMenu MainConsole { get; set; }

        static App() {
            MainConsole = new ConsoleMenu();
            SudokuManager = new SudokuManager(App.selectFile(), MainConsole);
        }

        public static string selectFile() {
            return @"../../../Fichier_sudokus_a_resoudre.sud";
        }
    }
}
