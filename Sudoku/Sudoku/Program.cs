﻿using System;
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
            ConsoleMenu.show();
        }

		public static string selectFile() {
			return @"c:\Fichier_de_sudoku_résolution2.sud";
		}
    }
}
