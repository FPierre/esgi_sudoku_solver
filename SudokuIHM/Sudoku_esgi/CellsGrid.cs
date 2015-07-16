using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sudoku_esgi
{
    public class CellsGrid : SudokuObject, IObservable<SudokuObject> 
	{
       // private static string gridDelimiter = @"//---------------------------";

        protected internal Cell[,] grid
        {
            get;
            set;
        }
         protected internal int size
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


        public string name
        {
            get;
            set;
        }

        public string  date
        {
            get;
            set;
        }

        public string required 
        {
            get;
            set;
        }


        public string error
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

        Resolver GridRecursiveResolver;



        public CellsGrid(Cell[,] value, List<String> defaultValues, List<IObserver<SudokuObject>> MainConsole)
            : this(MainConsole) 
		{
            GridRecursiveResolver = new Resolver();
            this.cantResolve = false;
            size = defaultValues.Count;
            grid = value;
            required = defaultValues.Aggregate( (i , j) => i +j);
       //     verifyAppearance();
		}


        public CellsGrid(CellsGrid cellsgrid) 
            : this(cellsgrid.observers)
        {
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
            // TODO: Complete member initialization
        }

      


        public CellsGrid  resolveGrid(bool normalMode = true)
        {
            CellsGrid TempGrid = new CellsGrid(this.observers);
            do
            {
                int oldNumberResolution = numberOfDots;
                bool doSomething = false;
                foreach(Cell c in grid)
                {

                    if (c.Value.Equals(".") && c.hypothesis.Count == 1)
                    {

                        c.Value = c.hypothesis.First();
                        this.Log(ModeText.Verbose, String.Format("Add value {0} at [{1},{2}]",c.Value, c.PosX ,c.PosY));
                        this.Log(ModeText.Verbose, String.Format("Diffuse in all Ensemble the value",c.Value, c.PosX ,c.PosY));

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
                        TempGrid = new CellsGrid(GridRecursiveResolver.ResolveBlockCells(this));
                        if (TempGrid.isDone())           //Save have a different Adresse 
                        {
                            return TempGrid;
                        }
                        if (TempGrid.cantResolve == true)
                        {
                            return TempGrid;
                        }
                    }
                    else
                    {
                        return this;
                    }
                }
            }
            while (!this.isDone() && TempGrid.cantResolve == false);
           return this;

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
            /**StringBuilder text = new StringBuilder() ;
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
            return text.ToString();**/

            return String.Format("{0} ({1})", name, date);
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



	}
}

