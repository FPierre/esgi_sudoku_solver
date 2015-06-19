using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Sudoku {
    class SudokuManager {
        private string path;
        private string delimiter;
        private int mode;
        List<CellsGrid> modelList;
        

        /**
         * Constructeur
         * @param (string) path = Nom du fichier .sud
         * @param (string) delimiter = Chaîne de caractères de séparation des sudokus (optionnel)
         * @param (int) mode = Mode du manager 0 = validation, 1 = résolution (optionnel)
         **/
        public SudokuManager(string path, int mode = 1) {
            this.Path = path;
     
            this.ModelList = new List<CellsGrid>();
            this.Mode = mode;

            if (!File.Exists(this.Path)) {
                Console.WriteLine("Erreur: " + this.Path + " n'existe pas.");
                return;
            }

            this.verifyIntegrityOfAllSudoku();
        }

        public string Path {
            get { return this.path; }
            set { this.path = value; }
        }

        public string Delimiter {
            get { return this.delimiter; }
            set { this.delimiter = value; }
        }

        public List<CellsGrid> ModelList {
            get { return this.modelList; }
            set { this.modelList = value; }
        }

        public int Mode {
            get { return this.mode; }
            set { this.mode = value; }
        }

        private void verifyIntegrityOfAllSudoku() {
            int size = 0;
            bool readFirstLine = false;
            using (StreamReader file = new StreamReader(path))
            {
                do
                {
                    if (readFirstLine == false)
                    {
                        this.delimiter = file.ReadLine();
                        readFirstLine = true;
                    }
                    do
                    {
                        String name = file.ReadLine();
                        String date = file.ReadLine();
                        String required = file.ReadLine();
                        size = required.Length;
                        Cell[,] tableCell = new Cell[size, size];

                        String error = String.Empty;
                        bool isFullyOfPoint = true;
                        List<Ensemble> MesEnsembleLine = new List<Ensemble>();
                        List<Ensemble> MesEnsembleColumn = new List<Ensemble>();
                        List<Ensemble> MesEnsembleSector = new List<Ensemble>();
                        Cell myCell;
                        for ( int i = 0; i < size; i++)
                        {
                            verifyEnsemble(MesEnsembleLine, i);
                            String[] tempLine = Utility.SplitWithSeparatorEmpty(file.ReadLine());
                            for ( int j = 0; j < size; j++)
                            {

                                verifyEnsemble(MesEnsembleColumn, j);
              
                                double sqrtNumber = Math.Sqrt((Convert.ToDouble(size)));
                                int indexSector = ((int)(Math.Floor(i / sqrtNumber) * sqrtNumber + Math.Floor(j / sqrtNumber)));
                                verifyEnsemble(MesEnsembleSector, indexSector);
                                myCell = new Cell(MesEnsembleColumn[j], MesEnsembleLine[i], MesEnsembleSector[indexSector], tempLine[j], new List<String>(Utility.SplitWithSeparatorEmpty(required))); ;


                                if(i == 8 && j ==7)
                                {
                                    Console.Out.WriteLine("test");
                                }

                                if (required.Contains(tempLine[j]) || tempLine[j].Equals("."))
                                {
                                    if(!tempLine[j].Equals("."))
                                    {
                                        isFullyOfPoint = false;
                                    }

                                    tableCell[i, j] = myCell;
                                    if (!myCell.ExistsInItsEnsemble())
                                    {
                                        myCell.addItsEnsemble();
                                    }
                                    else
                                    {
                                        error = String.Format("grille : {2} {3}la cellule à l'index ({0} , {1}) a une valeur semblable dans sa ligne, dans sa colonne ou dans son secteur", i, j, name, Environment.NewLine);
                                        Console.Out.WriteLine(error);
                                    }
                                }
                                else
                                {
                                    error = String.Format("grill : {0} {1} la cellule à l'index ({2}, {3}) n'est pas comprise dans les valeurs requises {4} {5}, Values {6}", name, Environment.NewLine, i, j, required, Environment.NewLine, tempLine[j]);
                                }
                            }
              
                        }
                        List<String> maListe = new List<string>(Utility.SplitWithSeparatorEmpty(required));
                        if (error.Equals(String.Empty))
                        {
                            this.ModelList.Add(new CellsGrid(tableCell, maListe, date, name));
                           Console.Out.WriteLine( this.modelList.Last().ToString());
                           Console.ReadLine();

                        }

                        if((Convert.ToInt32(Math.Floor(Math.Sqrt(this.modelList.Last().size) ))) != 0)
                        {
                           this.modelList.Last().isValid = false;
                           this.modelList.Last().error += String.Format(error, "(format non supporté, devrait être 9x9, 16x16 ou 25x25).");
                        }

                        if(isFullyOfPoint == true)
                        {
                            this.modelList.Last().isValid = false;
                            this.modelList.Last().error += String.Format(error, "grille {0} n'a aucun chiffre", name);
                        }

                       
                       
                    }
                    while (!file.ReadLine().Equals(this.delimiter));
                }
                while (!file.EndOfStream );
            }
       
        }

        private void verifyEnsemble(List<Ensemble> ensemble, int pos)
        {
            try
            {
                if(ensemble.Count <= pos )
                    ensemble.Add(new Ensemble(new List<Cell>()));
            }
            catch (Exception e)
            {
                // Console.Out.WriteLine(e.Message);
                ensemble.Add(new Ensemble(new List<Cell>()));
            }

        }
/*
        private string checkAllSudoku(int mode) {
            const String error = "Le sudoku est invalide {0}";

            foreach (CellsGrid model in this.ModelList) {
                model.ToString();

                // Si le sudoku n'est pas un format supporté tel que 9x9, 16x16 ou 25x25
                if (model.size % 
                    Convert.ToInt32(Math.Floor(Math.Sqrt(model.size))) != 0) {
                        model.isValid = false;
                        model.error += String.Format(error, "(format non supporté, devrait être 9x9, 16x16 ou 25x25).");

                    //return String.Format(error, "(format non supporté, devrait être 9x9, 16x16 ou 25x25).");
                }

                // Compteur de la taille des lignes et colonnes
                int counterLine = 0, counterColumn = 0;
                // Liste pour futur check de caractères
                List<string> allValues = new List<string>();
                string required = model.required;
                // Ajout du "." au pattern pour la résolution
                if (mode == 1) 
                    required += ".";

                for (int i=0; i < model.size; ++i) {
                    if (counterLine > model.size)
                    {
                          model.isValid = false;
                          model.error += String.Format(error, "(une ligne est plus grande que le pattern).");
                    }
                    for (int j=0; j < model.size; ++j) {
                        if (counterColumn > model.size)
                        {
                            model.isValid = false;
                            model.error += String.Format(error, "(une colonne est plus grande que le pattern).");
                        }


                        string s = model[i, j].value;

                        // Comparaison des valeurs au pattern
                        if (!required.Contains(s))
                        {
                            model.isValid = false;
                            model.error += String.Format(error, "(des valeurs sont manquantes).");
                        }

                        // Ajoute la valeur dans la liste pour les check
                        allValues.Add(s);
                        counterColumn++;
                    }
                    counterColumn = 0;
                    counterLine++;
                }
                counterLine = 0;
                
                // Vérifier qu'il n'y ait pas que des "."
                if (allValues.Count(c => c == ".") == model.size) {
                    return String.Format(error, "(toutes les valeurs sont vides).");
                }

            }
            return "Le sudoku est valide.";
        } 
 */
    }
}
