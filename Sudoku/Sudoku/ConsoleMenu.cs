using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class ConsoleMenu
    {
        private static int choice;

        public static void show()
        {
            string menu = " ****** Menu ****** " + Environment.NewLine +
                " 1 – Sudoku file validation "   + Environment.NewLine +
                " 2 – Sudoku file resolution "   + Environment.NewLine +
                " 3 – Quit "                     + Environment.NewLine +
                " Choice: ";

            do
            {
                Console.Write(menu);

                choice = Convert.ToInt32(Console.ReadLine());

                string result = compute(choice);

                Console.WriteLine(result);

                redirect(choice);
            }
            while (choice != 4);

            Console.ReadLine();
        }

        private static string compute(int choice)
        {
            string result = " ";

            switch (choice)
            {
                case 1:
                    result += "–> Sudoku file validation ";
                    // SudokuManager manager = new SudokuManager(Properties.Resources.testSudoku);
                    break;
                case 2:
                    result += "–> Sudoku file resolution ";
                    break;
                case 3:
                    result += "–> Bye bye ! ";
                    // Sort du programme
                    Environment.Exit(0);
                    break;
                default:
                    result += "Bad choice, try again ";
                    break;
            }

            return result;
        }

        private static int redirect(int choice)
        {
            switch (choice)
            {
                case 1:
                    // SudokuManager (path, delimiter, choice);
                    break;
                case 2:
                    // SudokuManager (path, delimiter, choice);
                    break;
            }

            return 0;
        }
    }
}
