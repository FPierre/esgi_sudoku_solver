using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
			int choice;
			string path, delimiter;
			path = selectFile ();
			delimiter = selectDelimiter ();
			do {
				Console.Write(getMainMenu());
				choice = Convert.ToInt32(Console.ReadLine());
				string result = computeMainMenuChoice(choice);
				Console.WriteLine(result);
				redirectToFunction (choice, path, delimiter);
			} while (choice != 4);

			Console.ReadLine();

        }

		public static string selectDelimiter () {
			return @"//---------------------------";
		}

		public static string selectFile() {
			return @"c:\Fichier_de_sudoku_résolution2.sud";
		}

		public static string getMainMenu(){

			return  " ****** Menu ****** "+Environment.NewLine
				+ " 1 – Sudoku file validation "+Environment.NewLine
				+ " 2 – Sudoku file resolution " +Environment.NewLine
				+ " 3 – Sudoku grid Generation " +Environment.NewLine
				+ " 4 – Quit " + Environment.NewLine
				+ " Choice ? : " ;
		}

		public static string computeMainMenuChoice(int choice)
		{
			string result =  " " ;

			switch (choice)
			{
			case 1:
				result += " –> Sudoku file validation ";
                 SudokuManager manager = new SudokuManager(Properties.Resources.testSudoku);

                 
				break;
			case 2:
				result += "–> Sudoku file resolution ";
				break;
			case 3:
				result += "–> Sudoku grid Generation ";
				break;
			case 4:
				result += "–> Bye bye ! ";
				break;
			default:
				result += "Bad choice, try again ";
				break;
			}
			return result;
		}

		public static int redirectToFunction(int choice, string path, string delimiter)
		{
			switch (choice)
			{
			case 1:
				//SudokuManager (path, delimiter, choice);
				break;
			case 2:
				//SudokuManager (path, delimiter, choice);
				break;
			case 3:
				Console.WriteLine("/!\\ Sudoku grid Generation Coming soon!");
				break;
			}
			return 0;
		}
    }
}
