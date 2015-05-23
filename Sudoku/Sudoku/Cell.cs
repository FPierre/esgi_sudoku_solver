using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Sudoku
{
	public class Cell
	{
		
		private Ensemble listColumn {
			get {
				return this.listColumn;
			}
			set{
				this.listColumn = value;
			}
		}
		private Ensemble listLine{
			get {
					return this.listLine;
			}
			set{
					this.listLine = value;
			}
		}
		private Ensemble listSector {
			get {
				return this.listLine;
			}
			set {
				this.listLine = value;
			}
		}

		protected internal List<String> hypothesis;
        protected internal String value;

		public Cell (Ensemble listColumn , Ensemble listLine , Ensemble listSector,String value,List<String> hypothesis)
		{
			this.listColumn = listColumn;
			this.listLine = listLine;
			this.listSector = listSector;
			this.value = value;
			this.hypothesis = hypothesis;
		}

		public Cell (String value,List<String> hypothesis)
		{
            this.value = value;
            this.hypothesis = hypothesis;
		}

		public bool ExistsInEnsemble(params Ensemble[] t)
		{
            bool exists = true;
            int i = 0;
            while(i < t.Length && exists == true)
            {
                exists = t[i].ExistInEnsemble(this);
	             i++;
            }
            return exists;	
		}

        public void add(params Ensemble[]t)
        {
            foreach(Ensemble monEnsemble in t)
            {
                monEnsemble.Add(this);
            }
            this.calculateNewHipothesis();
        }

/*
        public void calculateNewHipothesis()
        {
            List<Ensemble> tempList = new List<Ensemble>();
            tempList.Add(this.listColumn);
            tempList.Add(this.listLine);
            tempList.Add(this.listSector);
            tempList.Select(monEnsemble => monEnsemble.cellsList.FindAll(
                                            myCell =>  this.hypothesis.Exists(
                                                oldHypothesis => myCell.value.Equals(oldHypothesis))));
       }
 * 
 */

        public bool ValuesIsNull()
        {
            return this.value.Equals(".");
        }

        public bool ExistsInEnsemble( int value)
        {

            Cell tempCell = new Cell(Convert.ToString(value),new List<string>());
            List<Ensemble> TempListEnsemble = new List<Ensemble>();
            TempListEnsemble.Add(this.listColumn);
            TempListEnsemble.Add(this.listLine);
            TempListEnsemble.Add(this.listSector);

           return TempListEnsemble.Any(e => e.ExistInEnsemble(tempCell));
        }



    }
}

