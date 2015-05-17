using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Sudoku
{
	public class Cell
	{
		enum ErrorCell{ExistsInLine,ExistsInColumn,ExistsInSector,notExistsInValues};
		private Ensemble listColumn {
			get {
				return this->listColumn;
			}
			set{
				this->listColumn = value;
			}
		}
		private Ensemble listLine{
			get {
					return this->listLine;
			}
			set{
					this->listLine = value;
			}
		}
		private Ensemble listSector {
			get {
				return this->listLine;
			}
			set {
				this->listLine = value;
			}
		}

		private List<String> hypothesis;
		private String values;

		public Cell (Ensemble listColumn , Ensemble listLine , Ensemble listSector,String values,List<String> hypothesis)
		{
			this->listColumn = listColumn;
			this->listLine = listLine;
			this->listSector = listSector;
			this->values = values;
			this->hypothesis = hypothesis;
		}

		public Cell (String values,List<String> hypothesis)
		{
			Cell (null, null, null, values, hypothesis);
		}


		public bool ExistsInEnsemble( Ensemble t)
		{
			return true;
				
		}


	}
}

