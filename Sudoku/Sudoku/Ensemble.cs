
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
            List<Cell> tempCel = this.cellsList.FindAll(c => c.hypothesis.Contains(cell.value));
            if(tempCel.Count != 0)
                tempCel.ForEach(c => c.hypothesis.Remove(cell.value));
            this.cellsList.Add(cell);
        }

        public bool ExistInEnsemble(Cell c)
        {
            return this.cellsList.Exists(cell => cell.value.Equals(c.value));
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

