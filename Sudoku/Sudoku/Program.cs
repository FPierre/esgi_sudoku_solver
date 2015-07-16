using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Program
    {
        string path = selectFile();
 
        // Main principal du programme
        static void Main(string[] args)
        {
            // Affiche le menu console pour choisir son mode
            ConsoleMenu MainConsole = new ConsoleMenu();
            MainConsole.show();
        }

		public static string selectFile() {
            return @"../../../Fichier_sudokus_a_resoudre.sud";
		}
    }
}
