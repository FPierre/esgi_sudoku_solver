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
        List<Model> modelList;

        /**
         * Constructeur
         * @param (string) path = Nom du fichier .sud
         * @param (string) delimiter = Chaîne de caractères de séparation des sudokus (optionnel)
         * @param (int) mode = Mode du manager 0 = validation, 1 = résolution (optionnel)
         **/
        public SudokuManager(string path, string delimiter = "//", int mode = 1) {
            this.Path = path;
            this.Delimiter = delimiter;
            this.ModelList = new List<Model>();
            this.Mode = mode;

            if (!File.Exists(this.Path)) {
                Console.WriteLine("Erreur: " + this.Path + " n'existe pas.");
                return;
            }

            if (populateList()) {
               checkAllSudoku(this.Mode);
            }
        }

        public string Path {
            get { return this.path; }
            set { this.path = value; }
        }

        public string Delimiter {
            get { return this.delimiter; }
            set { this.delimiter = value; }
        }

        public List<Model> ModelList {
            get { return this.modelList; }
            set { this.modelList = value; }
        }

        public int Mode {
            get { return this.mode; }
            set { this.mode = value; }
        }

        private bool populateList() {
            string file = "";

            try {
                file = File.ReadAllText(this.Path);
            } catch (IOException e) {
                Console.WriteLine(e.Message);
            }

            if (file.Length == 0) {
                Console.WriteLine("Erreur: " + this.Path + " est vide.");
                return false;
            }

            var allSudoku = file.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (allSudoku.Length == 0) {
                Console.WriteLine("Erreur: " + this.Path + " ne contient pas de valeurs valides.");
                return false;
            }
            int size = 0;
            for (int i=0; i < allSudoku.Length; ++i) {

                // D'après la 1ère délimitation d'un sudoku...
                if (allSudoku[i].StartsWith(this.Delimiter))
                {

                    int currentLine = i + 1;
                    string name = allSudoku[currentLine].Replace(this.Delimiter, string.Empty);
                    currentLine++;
                    string date = allSudoku[currentLine];
                    currentLine++;
                    size = allSudoku[currentLine].Length;
                    string required = allSudoku[currentLine];

                    List<LineValue> valueList = new List<LineValue>();
                    LineValue value = new LineValue(size);  // c = caractère, i = ligne

                    // Récupèrer la grille de sudoku
                    currentLine = currentLine + 1;
                    int limit = currentLine + size;
                    for (; currentLine < limit; currentLine++)
                    {
                        String[] tempLine = this.SplitWithSeparatorEmpty(allSudoku[currentLine]);
                        for (int k = 0; k < size;  ++k)
                        {
                            value.populateLineValue(tempLine[k].ToString(), k);  
                        }
                        
                        valueList.Add(value);
                        value.PosLine++;
                    }
                        

                    this.ModelList.Add(new Model(name, date, valueList, required, size));

                    // Passer au prochain sudoku
                    i += limit;
                }
            }

            return true;
        }

        private string checkAllSudoku(int mode) {
            const String error = "Le sudoku est invalide {0}";

            foreach (Model model in this.ModelList) {
                model.ToString();

                // Si le sudoku n'est pas un format supporté tel que 9x9, 16x16 ou 25x25
                if (model.Size % 
                    Convert.ToInt32(Math.Floor(Math.Sqrt(model.Size))) != 0) {
                        model.isValid = false;
                        model.error += String.Format(error, "(format non supporté, devrait être 9x9, 16x16 ou 25x25).");

                    //return String.Format(error, "(format non supporté, devrait être 9x9, 16x16 ou 25x25).");
                }

                // Compteur de la taille des lignes et colonnes
                int counterLine = 0, counterColumn = 0;
                // Liste pour futur check de caractères
                List<string> allValues = new List<string>();
                string required = model.Required;
                // Ajout du "." au pattern pour la résolution
                if (mode == 1) 
                    required += ".";

                for (int i=0; i < model.Size; ++i) {
                    if (counterLine > model.Size)
                    {
                          model.isValid = false;
                          model.error += String.Format(error, "(une ligne est plus grande que le pattern).");
                    }
                    for (int j=0; j < model.Size; ++j) {
                        if (counterColumn > model.Size)
                        {
                            model.isValid = false;
                            model.error += String.Format(error, "(une colonne est plus grande que le pattern).");
                        } 
               

                        string s = model.Grid[i].Value[i, j];

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
                if (allValues.Count(c => c == ".") == model.Size) {
                    return String.Format(error, "(toutes les valeurs sont vides).");
                }

            }
            return "Le sudoku est valide.";
        }


        private String[] SplitWithSeparatorEmpty(String s)
        {
            int number = s.Length;
            char[] c = s.ToCharArray();
            String[] stringTable = new string[number];
            for (int i = 0; i < number; i++)
            {
                stringTable[i] = "" + c[i];
            }
            return stringTable;
        }
    }

    class Model {
        private string name;
        private string date;
        private List<LineValue> grid;
        private string required;
        private int size;
        public String error;
        public bool isValid;

        public Model(string name, string date, List<LineValue> grid, string required, int size) {
            this.name = name;
            this.date = date;
            this.grid = grid;
            this.required = required;
            this.size = size;
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Date {
            get { return this.date; }
            set { this.date = value; }
        }

        public List<LineValue> Grid {
            get { return this.grid; }
            set { this.grid = value; }
        }

        public string Required {
            get { return this.required; }
            set { this.required = value; }
        }

        public int Size {
            get { return this.size; }
            set { this.size = value; }
        }

        public override string ToString() {
            string s = "Vérification du Sudoku \"" + this.Name + "\"\nDate : " + this.Date + "\n"
                + "Modèle requis : " + this.Required + "\n";
            return s;
        }
    }

    class LineValue {
        private string[,] value;
        private int posLine;
        private int posColumn;

        public LineValue(int size = 9, int posLine = 0) {
            this.value = new string[size, size];
            this.posLine = posLine;
        }

        public string[,] Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public int PosLine {
            get { return this.posLine; }
            set { this.posLine = value; }
        }

        public int PosColumn {
            get { return this.posColumn; }
            set { this.posColumn = value; }
        }

        public void populateLineValue(string value, int posColumn) {
            this.posColumn = posColumn;
            this.value[this.posLine, this.posColumn] = value;
        }
    }
}
