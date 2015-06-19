using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sudoku
{
	public class CellsGrid
	{
        private static string gridDelimiter = @"//---------------------------";

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

        internal int numberOfDots
        {

            get;
            set;
        }

        
		public CellsGrid (Cell[,] value , List<String> defaultValues)  
		{
     
            size = defaultValues.Count;
            grid = value;
            required = defaultValues.Aggregate( (i , j) => i +j);
       //     verifyAppearance();
		}


        public CellsGrid(Cell[,] value , List<String> defaultValues , String date, String name) : this(value,defaultValues)
        {
            this.date = date;
            this.name = name;
        }


        public void resolveGrid()
        {
            
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
                        Console.ReadLine();
                    }
                }
                if (doSomething == false && oldNumberResolution == numberOfDots)
                {
                    new Resolver().resolve(this);
                }


            }
            while (!this.isDone());
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

	}
}

