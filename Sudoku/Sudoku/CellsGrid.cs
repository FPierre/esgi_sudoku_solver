using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
	public class CellsGrid
	{
        Cell[,] grid
        {
            get;
            set;
        }
        int size
        {
            get;
            set;
        }
       
        
		public CellsGrid (String[,] value , List<String> defaultValues) 
		{
            List<Ensemble> MesEnsembleLine = new List<Ensemble>();
            List<Ensemble> MesEnsembleColumn = new List<Ensemble>();
            List<Ensemble> MesEnsembleSector = new List<Ensemble>();
            size = defaultValues.Count;
            grid = new Cell[size,size];

            for(int i = 0 ; i < value.Length ; i ++)
            {
                if (MesEnsembleColumn[i] == null)
                    MesEnsembleColumn[i] = new Ensemble(new List<Cell>());
                   
                for(int j = 0 ; j < value.Length ; j++)
                {
                    if (MesEnsembleLine[j] == null)
                        MesEnsembleLine[j] = new Ensemble(new List<Cell>());

                    double sqrtNumber = Math.Sqrt((Convert.ToDouble( value.Length)));
                    int indexSector = (int) (Math.Floor(i/sqrtNumber)  + Math.Floor(j/sqrtNumber) * sqrtNumber);
                   

                    if(MesEnsembleSector[indexSector] == null)
                        MesEnsembleSector[indexSector] = new Ensemble(new List<Cell>());
                    
                    Cell myCell = new Cell(value[i, j],defaultValues);
                    if(myCell.ExistsInEnsemble(MesEnsembleColumn[i],MesEnsembleLine[j],MesEnsembleSector[indexSector]))
                    {
                        myCell.add(MesEnsembleColumn[i], MesEnsembleLine[j], MesEnsembleSector[indexSector]);
                        grid[i, j] = myCell; 
                    }
                    else
                    {
                        String text = String.Format("la cellule à l'index {0},{1} a une valeur semblable dans sa ligne, dans sa colonne ou dans son secteur",i, j);
                        throw new Exception(text);
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
                    Console.Write(grid[i, j].value);
                    text.AppendFormat("{0,3}", grid[i, j].value);
                }
                text.Append(Environment.NewLine);
            }
            return text.ToString();
        }



	}
}

