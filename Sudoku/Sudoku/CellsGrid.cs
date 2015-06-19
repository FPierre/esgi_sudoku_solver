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


        public string ToString()
        {
            StringBuilder text = new StringBuilder() ;
    
            for(int i = 0 ; i < grid.Length ; i++)
            {
                for(int j = 0 ; j < grid.Length ; j++)
                {
                    text.AppendFormat("{0,3}", grid[i, j].value);
                }
                text.Append(Environment.NewLine);
            }
            return text.ToString();
        }

	}
}

