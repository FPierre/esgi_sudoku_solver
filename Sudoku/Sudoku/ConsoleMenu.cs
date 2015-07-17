using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public enum ModeText { Verbose, Warning, Error };
    public class ConsoleMenu : ViewObject,IObserver<SudokuObject> 
    {

        private  String choice;
        public  SudokuManager manager;
        private  bool managerIsOn = false;
        public ConsoleMenu() : base()
        {
            result = new StringBuilder();  
        }

        public void show()
        {
            string menu = " ****** Menu ****** " + Environment.NewLine +
                " 1 – Sudoku file validation "   + Environment.NewLine +
                " 2 – Sudoku file resolution "   + Environment.NewLine +
                " 3 – Quit "                     + Environment.NewLine +
                " Choice: ";

            do
            {
                Console.Write(menu);

                choice = Console.ReadLine();

                 compute();

                //Console.WriteLine(result);

                //redirect(choice);
            }
            while (!choice.Equals("3"));

            Console.ReadLine();
        }

        private  void compute()
        {
      
            switch (choice)
            {
                case "1":
                    result.Append( "–> Sudoku file validation ");
                    manager = new SudokuManager(Properties.Resources.testSudoku,this);
                    this.managerIsOn = true;
                    break;
                case "2":
                    result.Append("–> Sudoku file resolution ");
                    if (this.managerIsOn == false)
                    {
                        defineStepByStep();
                        manager = new SudokuManager(Properties.Resources.testSudoku, this);
                        this.managerIsOn = true;
                        
                    }
                    ConsoleMenu.mode = ModeText.Verbose;
                    int choiceSudokuu = this.chooseSudoku();

                    if (choiceSudokuu < 0)
                    {
                        Console.WriteLine("Vous n'avez riens choisis");
                        return;
                    }
                    else
                    {
                        defineStepByStep();
                        manager.resolve(choiceSudokuu);
                    }
                    
                    break;
                case "3":
                    result.Append("–> Bye bye ! ");
                    // Sort du programme
                    Environment.Exit(0);
                    break;
                default:
                    result.Append( "Bad choice, try again ");
                    break;
            }

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


        // The observable invokes this method to pass the Subject object to the observer
        public void OnNext(SudokuObject currentObject)
        {
            if (ConsoleMenu.mode <= currentObject.lastTextLogLevel)
            {
                Console.WriteLine(currentObject.TextLog);
                result.Append(currentObject.TextLog);
                if (ConsoleMenu.StepByStep == true)
                {
                    Console.ReadLine();
                }
            }
        }



        // Usually called when a transmission is complete. Not implemented.
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        // Usually called when there was an error. Didn't implement.
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }


        public int chooseSudoku()
        {

            Console.WriteLine("Choissisez quel sudoku vous voullez resoudre");
            int choice = this.manager.displayNames();
            
            return choice;    
        }


        public void defineStepByStep()
        {
            bool response = false;
            do
            {
                Console.WriteLine("voullez vous faire du pas à pas ? oui/non");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "oui": ConsoleMenu.StepByStep = true;
                        response = true;
                        break;

                    case "non": ConsoleMenu.StepByStep = false;
                        response = true;
                        break;

                    default: Console.WriteLine("mauvaise réponse");
                        break;
                }
            } while (response == false);
        }


    }
}
