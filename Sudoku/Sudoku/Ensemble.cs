
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


        public void Add(Cell cell)
        {
            List<Cell> tempCel = this.cellsList.FindAll(c => c.hypothesis.Contains(cell.values));
            tempCel.ForEach(c => c.hypothesis.Remove(cell.values));
            this.cellsList.Add(cell);
        }

        public static Ensemble operator +(Ensemble c1, Cell c2)
        {
            c1.Add(c2);
            return c1;
        }

        public static Ensemble operator +(Cell c2 , Ensemble c1)
        {
            c1.Add(c2);
            return c1;
        }


	}
}

