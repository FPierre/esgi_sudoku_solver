using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Sudoku
{
    public class CellsGrid : SudokuObject, IObservable<SudokuObject> , IObserver<SudokuObject>, INotifyPropertyChanged
	{
       // private static string gridDelimiter = @"//---------------------------";

        public  Cell[,] grid
        {
            get;
            set;
        }
         public  int size
        {
            get;
            set;
        }

        public Cell this[int i,int j]
        {
            get
            {
                return grid[i, j];

            }
        }


        public String name
        {
            get;
            set;
        }

        public String  date
        {
            get;
            set;
        }

        public String required 
        {
            get;
            set;
        }


        public String error
        {
            get;
            set;
        }

        public bool isValid
        {

            get;
            set;
        }

        public bool cantResolve
        {

            get;
            set;
        }

        internal int numberOfDots
        {

            get;
            set;
        }

        public ObservableCollection<string> logsCellsGrid {get ; set ;}

        Resolver GridRecursiveResolver;
        public List<CellsGrid> listHypotheticSudoku;


        public CellsGrid(Cell[,] value, List<String> defaultValues, List<IObserver<SudokuObject>> MainConsole)
            : this(MainConsole) 
		{
            GridRecursiveResolver = new Resolver();
            this.cantResolve = false;
            size = defaultValues.Count;
            grid = value;
            required = defaultValues.Aggregate( (i , j) => i +j);
            listHypotheticSudoku = new List<CellsGrid>();
       //     verifyAppearance();
		}


        public CellsGrid(CellsGrid cellsgrid) 
            : this(cellsgrid.observers)
        {
            this.listHypotheticSudoku = new List<CellsGrid>(cellsgrid.listHypotheticSudoku);

            this.cantResolve = cellsgrid.cantResolve;
            this.size = cellsgrid.size;
            this.required = cellsgrid.required;
            this.name = cellsgrid.name;
            this.isValid = cellsgrid.isValid;
            this.date = cellsgrid.date;
            this.error = cellsgrid.error;
            this.numberOfDots = cellsgrid.numberOfDots;
            this.GridRecursiveResolver = cellsgrid.GridRecursiveResolver;
            this.grid = new Cell[size, size];
            List<Ensemble> MesEnsembleLine = new List<Ensemble>();
            List<Ensemble> MesEnsembleColumn = new List<Ensemble>();
            List<Ensemble> MesEnsembleSector = new List<Ensemble>();
            double sqrtNumber = Math.Sqrt((Convert.ToDouble(size)));
            for(int i = 0 ; i < size ; i++)
            {
               if( MesEnsembleLine.Count <= i)
               {
                   MesEnsembleLine.Add(new Ensemble(this.observers));
               }

                for(int j = 0 ; j < size ; j++)
                {
                    if (MesEnsembleColumn.Count <= j)
                    {
                        MesEnsembleColumn.Add(new Ensemble(this.observers));
                    }

                    int indexSector = ((int)(Math.Floor(i / sqrtNumber) * sqrtNumber + Math.Floor(j / sqrtNumber)));

                    if (MesEnsembleSector.Count <= indexSector)
                    {
                        MesEnsembleSector.Add(new Ensemble(this.observers));
                    }
                   this.grid[i,j] = new Cell(MesEnsembleColumn[j], MesEnsembleLine[i], MesEnsembleSector[indexSector], cellsgrid.grid[i, j].Value, cellsgrid.grid[i,j].hypothesis,i,j,this.observers );
                    this.grid[i,j].addItsEnsemble();
                }
            }
        }

        public CellsGrid(Cell[,] value, List<String> defaultValues, String date, String name, List<IObserver<SudokuObject>> MainConsole)
            : this(value, defaultValues, MainConsole)  
        {

            this.date = date;
            this.name = name;
        }

        private CellsGrid(List<IObserver<SudokuObject>> MainConsole)
            : base()
        {
            foreach(IObserver<SudokuObject> observer in MainConsole )
            {
                this.observers.Add(observer);
            }
            this.observers.Add(this);
            // TODO: Complete member initialization
            logsCellsGrid = new ObservableCollection<string>();
        }

        public CellsGrid  resolveGrid(bool normalMode = true)
        {
            do
            {
                int oldNumberResolution = numberOfDots;
                bool doSomething = false;
                foreach(Cell c in grid)
                {
                    if (c.Value.Equals(".") && c.hypothesis.Count == 1)
                    {

                        c.Value = c.hypothesis.First();
                        this.Log(ModeText.Verbose, String.Format("Inject {0} at [{1},{2}]",c.Value, c.PosX ,c.PosY));
                        //this.Log(ModeText.Verbose, String.Format("Diffuse in all Ensemble the value",c.Value, c.PosX ,c.PosY));
                        NotifyPropertyChanged("TextLog");

                        c.diffuseInItsEnsemble();
                        doSomething = true;
                        numberOfDots--;
                        Console.WriteLine(this);

                    }
                    else
                    {
                        if (c.Value.Equals("."))
                        {
                            foreach (String hypothesis in c.hypothesis)
                            {
                                
                            
                                if ( (( !c.listColumn.ExistsInEnsembleHypothesis(hypothesis, c)  || !c.listLine.ExistsInEnsembleHypothesis(hypothesis, c)) ||  !c.listSector.ExistsInEnsembleHypothesis(hypothesis, c)))
                                {
                                    this.Log(ModeText.Verbose, String.Format("exclusions des hypothéses"));
                                    NotifyPropertyChanged("TextLog");
                                    c.Value = hypothesis;
                                    c.diffuseInItsEnsemble();
                                    doSomething = true;
                                    numberOfDots--;
                                    Console.WriteLine(this);
                                    break;
                                }
                            }
                        }
                    }

                    if(c.Value.Equals(".") && c.hypothesis.Count == 0)
                    {
                        this.cantResolve = true;
                        return this;
                    }
              

                }
                if (doSomething == false && oldNumberResolution == numberOfDots && this.cantResolve == false)
                {
                    if (normalMode == true)
                    {
                        return this.ResolveBlockCells();
                    }
                    else
                    {
                        return this;
                    }
                }
            }
            while (!this.isDone() && this.cantResolve == false);

           return this;
        }

        public Dictionary<string, List<Cell>> counBlockCells()
        {
            Dictionary<string, List<Cell>> listOccurenHypothesis = new Dictionary<String, List<Cell>>();

            foreach (var cell in this.grid)
            {
                if (cell.Value.Equals("."))
                {

                    if (cell.hypothesis.Count > 0)
                    {
                        String str = cell.hypothesis.Aggregate((stringa, stringb) => stringa + stringb);

                        if (listOccurenHypothesis.ContainsKey(str))
                        {

                            listOccurenHypothesis[str].Add(cell);
                        }
                        else
                        {
                            List<Cell> temp = new List<Cell>();
                            temp.Add(cell);
                            listOccurenHypothesis.Add(str, temp);

                        }
                    }
                }
            }
            return listOccurenHypothesis;
        }

        public List<KeyValuePair<string, List<Cell>>> sortedBlockCells(Dictionary<string, List<Cell>> dict)
        {
            List<KeyValuePair<string, List<Cell>>> myList = dict.ToList();
            myList.Sort(
                delegate(KeyValuePair<string, List<Cell>> firstpair,
                KeyValuePair<string, List<Cell>> nextPair)
                {

                    if (firstpair.Key.Length == nextPair.Key.Length)
                        return 0;
                    if (firstpair.Key.Length < nextPair.Key.Length)
                        return -1;

                    return 1;
                }
             );

            return myList;
        }

        public List<Cell> getBlockCells(List<KeyValuePair<string, List<Cell>>> list)
        {
            bool doSomething = false;
            List<Cell> result = new List<Cell>();

            int i = 0;
            do
            {
                Cell[] cellTable = list[i].Value.ToArray();
                if (cellTable.Count() > 1)
                {
                    for (int j = 0, K = 1; K < cellTable.Count(); j++, K++)
                    {
                        if (cellTable[j].existInEnsembleOf(cellTable[K]))
                        {
                            result.Add(cellTable[j]);
                            result.Add(cellTable[K]);
                            doSomething = true;
                        }

                    }
                    if (i == (list.Count - 1) && doSomething == false)
                    {
                        Log(ModeText.Error, "Aucune Solution trouvé");

                        Log(ModeText.Error, "Retour à la version précédente si existe");

                        doSomething = true;
                    }

                }
                i++;
            }
            while (i < list.Count && doSomething == false);


            return result;
        }

        private void deleteInListCells(List<Cell> cells, Cell cell, Cell nextCell)
        {
            cells = cells.FindAll(Mycell => !Mycell.EqualsInValueAndHypothesis(cell) && !Mycell.EqualsInValueAndHypothesis(nextCell));
            foreach (String hypothesisString in cell.hypothesis)
            {
                foreach (Cell tempCell in cells)
                {
                    tempCell.hypothesis.Remove(hypothesisString);
                }
            }
        }

        private void deleteAllHypothesisFromEnsembleInCell(List<Cell> blockCells, Cell c)
        {
            for (int i = 0; i < blockCells.Count - 1; i++)
            {
                Log(ModeText.Verbose, String.Format("Delete hypothesis {0}", blockCells[i].hypothesis.Aggregate((stringa, stringb) => stringa + stringb)));
                int j = i + 1;
                if (blockCells[i].ExistsInEnsemble(blockCells[j].listColumn))
                {
                    deleteInListCells(blockCells[j].listColumn.cellsList, blockCells[i], blockCells[j]);

                }

                if (blockCells[i].ExistsInEnsemble(blockCells[j].listLine))
                {

                    deleteInListCells(blockCells[j].listColumn.cellsList, blockCells[i], blockCells[j]);
                }

                if (blockCells[i].ExistsInEnsemble(blockCells[j].listSector))
                {
                    deleteInListCells(blockCells[j].listColumn.cellsList, blockCells[i], blockCells[j]);
                }
            }
        }

        public CellsGrid ResolveBlockCells()
        {
            List<CellsGrid> testList = new List<CellsGrid>();
            testList.Add(new CellsGrid(this));
            
            Log(ModeText.Verbose, "Save last version");

            Dictionary<string, List<Cell>> counter = this.counBlockCells();
            List<KeyValuePair<string, List<Cell>>> sortedCells = this.sortedBlockCells(counter);

            List<Cell> blockCells = this.getBlockCells(sortedCells);

            Log(ModeText.Verbose, "cellule bloquante");
            foreach (Cell c in blockCells)
            {
                Console.Out.WriteLine(c.hypothesis.Aggregate((stringa, stringb) => stringa + stringb));
                Log(ModeText.Verbose, String.Format("({0},{1})", c.PosX, c.PosY));

                // Console.ReadLine();
            }

            if (blockCells.Count == 0)
            {
                if (listHypotheticSudoku.Any(Listgrid => this.EqualsInCell(Listgrid)))
                {
                    this.cantResolve = true;
                    return this;
                }
                listHypotheticSudoku.Add(this);

                Log(ModeText.Verbose, "Pure brute force");
                foreach (KeyValuePair<string, List<Cell>> keyPair in sortedCells)
                {

                    foreach (Cell c in keyPair.Value)
                    {
                        List<String> temp = new List<String>(c.hypothesis);
                        foreach (String hypothesis in temp)
                        {
                            testHypothesis(hypothesis, testList, c);
                            if (this.isDone())
                            {
             
                                return this;

                            }
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < blockCells.Count; i++)
                {
                    var cell = blockCells[i];
                    List<String> temp = new List<String>(cell.hypothesis);
                    testList.Last().cantResolve = false;
                    // testList.Add(tempgrid);
                    deleteAllHypothesisFromEnsembleInCell(blockCells, cell);
                    this.resolveGrid(false);

                    if (testList.Last().isDone())
                    {
                        return this;
                    }

                    foreach (String Hypothesis in temp)
                    {
                        testHypothesis(Hypothesis,testList ,cell);
                        if (this.isDone())
                        {
                            return this;
                        }
                    }

                    this.cantResolve = true;
                    testList.Remove(testList.Last());
                }
            }
            this.cantResolve = true;

            return this;
        }

        private void testHypothesis(string Hypothesis, List<CellsGrid> testList, Cell cell)
        {
            testList.Add(new CellsGrid(this));
            Console.Out.WriteLine(testList.Last());
            Cell realCell = this[cell.PosX, cell.PosY];
            this.cantResolve = false;
            Log(ModeText.Verbose, String.Format("test value {0} at  ({1},{2})", Hypothesis, realCell.PosX, realCell.PosY));
            
            realCell.Value = Hypothesis;
            realCell.diffuseInItsEnsemble();
            //Console.Out.WriteLine(testList.Last());
            this.resolveGrid();

            if (this.cantResolve == true)
            {
                CellsGrid saveGrid = testList.Last();
                testList.Remove(testList.Last());
                foreach(Cell saveCell in saveGrid.grid)
                {
                    this.grid[saveCell.PosX, saveCell.PosY].Value = saveCell.Value;
                    this.grid[saveCell.PosX, saveCell.PosY].hypothesis.Clear();
                    this.grid[saveCell.PosX, saveCell.PosY].hypothesis.AddRange(saveCell.hypothesis);
                }

                testList.Last().cantResolve = true;
                Log(ModeText.Verbose, "RollBack");

            }
        }

        public void verifyAppearance()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    Cell myCell = grid[i, j];
                    if (!myCell.ExistsInItsEnsemble())
                    {
 //                       myCell.add(MesEnsembleColumn[j], MesEnsembleLine[i], MesEnsembleSector[indexSector]);
                        grid[i, j] = myCell;
                    }
                    else
                    {
                        error = String.Format("grille : {3} {4}la cellule à l'index {0},{1} a une valeur semblable dans sa ligne, dans sa colonne ou dans son secteur", i, j,this.name,Environment.NewLine);

                        this.Log(ModeText.Error, error);
                        NotifyPropertyChanged("TextLog");

                        this.isValid = false;
                        break;
                    }
                }
            }
        }

        public bool isDone()
        {
            foreach(Cell cell in grid)
            {
                if(cell.Value.Equals("."))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder() ;
            text.Append(name);

            text.Append(Environment.NewLine);
            text.Append(date);
            text.Append(Environment.NewLine);
            for(int i = 0 ; i < size; i++)
            {
                for(int j = 0 ; j < size ; j++)
                {
                    text.AppendFormat("{0,3}", grid[i, j].Value);
                    if (grid[i, j].hypothesis.Count != 0 )
                        text.AppendFormat("({0})", grid[i, j].hypothesis.Aggregate((stringa,stringb) => stringa+ stringb));
                   // text.AppendFormat("({0})", grid[i, j].hypothesis.Aggregate((stringa, stringb) => stringa + stringb));
                }
                text.Append(Environment.NewLine);
            }
            text.Append(Environment.NewLine);
            return text.ToString();
        }

        public Cell Find(Cell cell , int pos)
        {
            int nbr = pos + 1;
            foreach(Cell c in grid)
            {
                if (c.Equals(cell))
                    nbr--;
                    if (nbr == 0)
                        return c;
            }

            return null;
        }

        public bool Exists(Cell cell , int i)
        {
            int nbr = i + 1;
            foreach (Cell c in grid)
            {
                if (c.Equals(cell))
                {
                    nbr--;
                    if(nbr == 0)
                        return true;
                }
                    
            }

            return false;
        }

        public  bool EqualsInCell(object obj)
        {
            if (obj is CellsGrid)
            {
                CellsGrid compareGrid = obj as CellsGrid;
                if(compareGrid.size != this.size)
                    return false;
                foreach(Cell c in this.grid)
                {
                    if(!(compareGrid[c.PosX,c.PosY].EqualsInValueAndHypothesis(c)))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }


        // The observable invokes this method to pass the Subject object to the observer
        public void OnNext(SudokuObject currentObject)
        {
            logsCellsGrid.Add(currentObject.TextLog);
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

	}
}

