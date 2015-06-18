
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Sudoku
{
	public class Ensemble
	{
		protected internal List<Cell> cellsList
		{ get; set; }
		public Ensemble (List<Cell> cellsList)
		{
			this.cellsList = cellsList;
		}

        public Ensemble()
        {
            this.cellsList = new List<Cell>();
        }


        public void Add(Cell cell)
        {
            this.cellsList.Add(cell);
            if (!cell.value.Equals("."))
            {
                //cell.hypothesis = null;
                List<Cell> tempCel = this.cellsList.FindAll(c => c.hypothesis.Contains(cell.value));
                if (tempCel.Count != 0)
                    tempCel.ForEach(c => c.hypothesis.Remove(cell.value));
            }
            else
            {

                foreach(Cell c in this.cellsList)
                {
                    if(cell.hypothesis.Contains(c.value))
                    {
                        cell.hypothesis.Remove(c.value);
                    }
                }
            }
            
            
        }

        public bool ExistInEnsemble(Cell c)
        {
            if(!c.value.Equals("."))
                return this.cellsList.Exists(cell => cell.value.Equals(c.value));
            return false;
        }

        public static Ensemble operator +(Ensemble s1, Cell c2)
        {
            s1.Add(c2);
            return s1;
        }

        public static Ensemble operator +(Cell c2 , Ensemble s1)
        {
            return s1 + c2 ;
        }


	}
}

