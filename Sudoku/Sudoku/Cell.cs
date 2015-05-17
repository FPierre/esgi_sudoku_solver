using System;
using System.Linq;
using System.Collections;


namespace Sudoku
{
	public class Cell
	{
		private Ensemble listColumn;
		private Ensemble listLine;
		private Ensemble listSector;

		private ArrayList<String> hypothesis;
		private String values;

		public Cell ()
		{
		}
	}
}

