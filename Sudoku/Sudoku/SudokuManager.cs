using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.ComponentModel;

namespace Sudoku {
    public class SudokuManager : SudokuObject, IObservable<SudokuObject>, INotifyPropertyChanged {
        private string path;
        private string delimiter;
        private int mode;
        public ObservableCollection<CellsGrid> modelList { get; set; }
        public ObservableCollection<string> logs { get; set; }
        public CellsGrid _gridSelected;
        public CellsGrid GridSelected { 
            get{
               return  this._gridSelected;
                
            }
            set{
                this._gridSelected = value;
                NotifyPropertyChanged("GridSelected");    
            }
        }

        /**
         * Constructeur
         * @param (string) path = Nom du fichier .sud
         * @param (string) delimiter = Chaîne de caractères de séparation des sudokus (optionnel)
         * @param (int) mode = Mode du manager 0 = validation, 1 = résolution (optionnel)
         **/
        public SudokuManager(string path, IObserver<SudokuObject> MainConsole, int mode = 0)
            : base() {
            this.Path = path;
            this.ModelList = new ObservableCollection<CellsGrid>();
            this.Logs = new ObservableCollection<string>();
            this.Mode = mode;

            Subscribe(MainConsole);

            if (!File.Exists(this.Path)) {
                Console.WriteLine("Erreur: " + this.Path + " n'existe pas.");
                return;
            }
            this.verifyIntegrityOfAllSudoku();
            GridSelected = this.modelList.First();
        }

        public string Path {
            get { return this.path; }
            set { this.path = value; }
        }

        public string Delimiter {
            get { return this.delimiter; }
            set { this.delimiter = value; }
        }

        public ObservableCollection<CellsGrid> ModelList {
            get { return this.modelList; }
            set { this.modelList = value; }
        }

        public int Mode {
            get { return this.mode; }
            set { this.mode = value; }
        }

        public ObservableCollection<string> Logs {
            get { return this.logs; }
            set { this.logs = value; }
        }

        private void verifyIntegrityOfAllSudoku() {
            int size = 0;
            using (StreamReader file = new StreamReader(path)) {
                this.delimiter = file.ReadLine();
                do {
                    String name = file.ReadLine();
                    String date = file.ReadLine();
                    String required = file.ReadLine();
                    size = required.Length;
                    Cell[,] tableCell = new Cell[size, size];

                    String error = String.Empty;
                    bool getError = false;
                    bool isFullyOfPoint = true;
                    List<Ensemble> MesEnsembleLine = new List<Ensemble>();
                    List<Ensemble> MesEnsembleColumn = new List<Ensemble>();
                    List<Ensemble> MesEnsembleSector = new List<Ensemble>();

                    Cell myCell;
                    int numberOfDots = 0;
                    for (int i = 0; i < size; i++) {
                        verifyEnsemble(MesEnsembleLine, i);
                        String[] tempLine = Utility.SplitWithSeparatorEmpty(file.ReadLine());
                        for (int j = 0; j < size; j++) {

                            verifyEnsemble(MesEnsembleColumn, j);

                            double sqrtNumber = Math.Sqrt((Convert.ToDouble(size)));
                            int indexSector = ((int) (Math.Floor(i / sqrtNumber) * sqrtNumber + Math.Floor(j / sqrtNumber)));
                            verifyEnsemble(MesEnsembleSector, indexSector);
                            myCell = new Cell(MesEnsembleColumn[j], MesEnsembleLine[i], MesEnsembleSector[indexSector], tempLine[j], new List<String>(Utility.SplitWithSeparatorEmpty(required)), i, j, this.observers); ;

                            if (required.Contains(tempLine[j]) || tempLine[j].Equals(".")) {
                                if (!tempLine[j].Equals(".")) {
                                    isFullyOfPoint = false;
                                } else {
                                    numberOfDots++;
                                }

                                tableCell[i, j] = myCell;
                                if (!myCell.ExistsInItsEnsemble()) {
                                    myCell.addItsEnsemble();
                                } else {
                                    if (getError == false) {
                                        getError = true;
                                        error = String.Format("grille : {2} {3}la cellule à l'index ({0} , {1}) a une valeur semblable dans sa ligne, dans sa colonne ou dans son secteur", i, j, name, Environment.NewLine);
                                    }
                                }
                            } else {
                                if (getError == false) {
                                    error = String.Format("grill : {0} {1} la cellule à l'index ({2}, {3}) n'est pas comprise dans les valeurs requises {4} {5}, Values {6}", name, Environment.NewLine, i, j, required, Environment.NewLine, tempLine[j]);
                                    getError = true;
                                }
                            }


                        }

                    }


                    List<String> maListe = new List<string>(Utility.SplitWithSeparatorEmpty(required));
                    this.ModelList.Add(new CellsGrid(tableCell, maListe, date, name, this.observers));
                    this.ModelList.Last().numberOfDots = numberOfDots;
                    GridSelected = this.modelList.Last();
                    this.Log(ModeText.Warning, GridSelected.name);
                    //  Console.Out.WriteLine( this.modelList.Last().ToString());

                    if (!error.Equals(String.Empty)) {
                        this.modelList.Last().isValid = false;
                        this.Log(ModeText.Warning, error);
                        getError = true;

                    }

                    if ((Convert.ToInt32(Math.Floor(Math.Sqrt(this.modelList.Last().size)))) != Convert.ToInt32(Math.Sqrt(this.modelList.Last().size))) {
                        this.modelList.Last().isValid = false;
                        error = "(format non supporté, devrait être 9x9, 16x16 ou 25x25).";
                        this.Log(ModeText.Warning, error);
                        this.modelList.Last().error += error;
                    } else {

                        if (isFullyOfPoint == true) {
                            this.modelList.Last().isValid = false;
                            error = "c'est une grille de point.";
                            this.Log(ModeText.Warning, error);
                            this.modelList.Last().error += error;
                        } else {

                            this.modelList.Last().isValid = true;

                        }
                    }

                    String line = String.Empty;
                    do {
                        line = file.ReadLine();
                        if (string.IsNullOrEmpty(line)) {
                            line = String.Empty;
                        }
                    }
                    while (!file.EndOfStream && !line[0].Equals(this.delimiter[0]));

                    this.Log(ModeText.Warning, "next Sudoku");
                }
                while (!file.EndOfStream);

            }

        }

        private void verifyEnsemble(List<Ensemble> ensemble, int pos) {
            try {
                if (ensemble.Count <= pos)
                    ensemble.Add(new Ensemble(new List<Cell>(), this.observers));
            } catch (Exception e) {
                Console.Out.WriteLine(e.Message);
                ensemble.Add(new Ensemble(new List<Cell>(), this.observers));
            }
        }

        public void resolveAll() {
            for (int i = 0; i < ModelList.Count; i++) {
                GridSelected = ModelList[i];
                resolveSelected();
            }
        }

        public int displayNames() {
            int filter = 50;
            int i = 1;

            foreach (CellsGrid grid in ModelList) {
                Console.Out.WriteLine("-{0}       {1}", i, grid.name);
                i++;
                if (i % filter == 0) {
                    int intChoice = -1;
                    while (intChoice < 1 || intChoice >= this.ModelList.Count) {

                        String text = Console.ReadLine();
                        if (!String.IsNullOrEmpty(text) && !text.Equals(Environment.NewLine)) {
                            try {
                                intChoice = Convert.ToInt32(text);
                                intChoice--;
                                return intChoice;
                            } catch (Exception e) {
                                Console.WriteLine("Rentrer un nombre");
                                intChoice = -1;
                            }

                        } else {
                            break;
                        }
                    }
                }

            }
            return -1;
        }

        public void resolveSelected() {
            if (GridSelected.isValid) {
                if (!GridSelected.isDone()) {

                    GridSelected.resolveGrid();

                    if (GridSelected.isDone()) {

                        this.Log(ModeText.Verbose, "Le sudoku est résolu.");
                        this.Log(ModeText.Verbose, GridSelected.ToString());

                    } else {
                        this.Log(ModeText.Verbose, "Le sudoku est non résolu.");
                        this.Log(ModeText.Verbose, GridSelected.ToString());
                    }
                }
            } else {

                String text = String.Format("Le sudoku {0} est invalide.", GridSelected.name);
                this.Log(ModeText.Verbose, text);
                this.Log(ModeText.Verbose, GridSelected.error);

            }
        }

        internal void resolve(int choiceSudokuu) {
            if (modelList[choiceSudokuu].isValid) {


                modelList[choiceSudokuu] = modelList[choiceSudokuu].resolveGrid();
                if (modelList[choiceSudokuu].isDone()) {

                    this.Log(ModeText.Verbose, "Le sudoku est résolu");
                    this.Log(ModeText.Verbose, modelList[choiceSudokuu].ToString());

                } else {
                    this.Log(ModeText.Verbose, "Le sudoku est non résolu");
                    this.Log(ModeText.Verbose, modelList[choiceSudokuu].ToString());
                }
            } else {

                String text = String.Format("Le sudoku {0} est valide.", modelList[choiceSudokuu].name);
                this.Log(ModeText.Verbose, text);
                this.Log(ModeText.Verbose, modelList[choiceSudokuu].error);

            }
        }

        


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }
}
