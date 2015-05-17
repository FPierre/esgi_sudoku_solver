
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
			this->cellsList = cellsList;
		
		}
	}
}

