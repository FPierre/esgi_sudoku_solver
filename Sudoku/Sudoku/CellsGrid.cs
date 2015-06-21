using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sudoku
{
	public class CellsGrid
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

        public String required;


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

        Resolver GridRecursiveResolver;
           

        
		public CellsGrid (Cell[,] value , List<String> defaultValues)  
		{
            GridRecursiveResolver = new Resolver();
            this.cantResolve = false;
            size = defaultValues.Count;
            grid = value;
            required = defaultValues.Aggregate( (i , j) => i +j);
       //     verifyAppearance();
		}


        public CellsGrid (CellsGrid cellsgrid)
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
                   MesEnsembleLine.Add(new Ensemble());
               }

                for(int j = 0 ; j < size ; j++)
                {
                    if (MesEnsembleColumn.Count <= j)
                    {
                        MesEnsembleColumn.Add(new Ensemble());
                    }

                    int indexSector = ((int)(Math.Floor(i / sqrtNumber) * sqrtNumber + Math.Floor(j / sqrtNumber)));

                    if (MesEnsembleSector.Count <= indexSector)
                    {
                        MesEnsembleSector.Add(new Ensemble());
                    }
                   this.grid[i,j] = new Cell(MesEnsembleColumn[j], MesEnsembleLine[i], MesEnsembleSector[indexSector], cellsgrid.grid[i, j].value, cellsgrid.grid[i,j].hypothesis,i,j );
                    this.grid[i,j].addItsEnsemble();
                }
            }
        }

        public CellsGrid(Cell[,] value , List<String> defaultValues , String date, String name) : this(value,defaultValues)
        {
            this.date = date;
            this.name = name;
        }

        private CellsGrid()
        {
            // TODO: Complete member initialization
        }

      


        public CellsGrid  resolveGrid()
        {
            CellsGrid TempGrid = new CellsGrid();
            do
            {
                int oldNumberResolution = numberOfDots;
                bool doSomething = false;
                foreach(Cell c in grid)
                {
                    if(c.value.Equals(".") && c.hypothesis.Count == 1)
                    {
                        
                        c.value = c.hypothesis.First();
                        c.diffuseInItsEnsemble();
                        doSomething = true;
                        numberOfDots--;
                        Console.WriteLine(this);
//                        Console.ReadLine();
                    }

                    if(c.value.Equals(".") && c.hypothesis.Count == 0)
                    {

                        this.cantResolve = true;
                        return this;
                    }
              

                }
                if (doSomething == false && oldNumberResolution == numberOfDots && this.cantResolve == false)
                {

                    
                    TempGrid = new CellsGrid(GridRecursiveResolver.ResolveBlockCells(this));
                    if(TempGrid.isDone())           //Save have a different Adresse 
                    {
                        return TempGrid;
                    }
                    if(TempGrid.cantResolve == true)
                    {
                        return TempGrid;
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
                        Console.Out.WriteLine(error);
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
                if(cell.value.Equals("."))
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
                    text.AppendFormat("{0,3}", grid[i, j].value);
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
        

	}
}

