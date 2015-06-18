using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Sudoku
{
	public class Cell
	{

        protected internal Ensemble listColumn
        {
            get;
            set;
        }
        protected internal Ensemble listLine
        {
            get;
            set;
        }
        protected internal Ensemble listSector
        {
            get;
            set;
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

		private Cell (String value,List<String> hypothesis)
		{
            this.value = value;
            this.hypothesis = hypothesis;
		}

        private Cell(String value, String hypothesis)
            : this(value,new List<String>(Utility.SplitWithSeparatorEmpty(hypothesis)))
        {
   
        }

        private Cell(Cell cell, string hypothesis) : this(cell.value,new List<String>(Utility.SplitWithSeparatorEmpty(hypothesis)))
        {

        }

		public bool ExistsInEnsemble(params Ensemble[] t)
		{
            bool exists = false;
            int i = 0;
            while(i < t.Length && exists == false)
            {
                if(t[i].ExistInEnsemble(this))
                    exists = true;
	             i++;
            }
            return exists;	
		}

        public bool ExistsInItsEnsemble()
        {
        
            return this.ExistsInEnsemble(listColumn, listLine, listSector);

        }

        public void add(params Ensemble[]t)
        {
            foreach(Ensemble monEnsemble in t)
            {
                monEnsemble.Add(this);
            }
            //this.calculateNewHipothesis();
        }

        public void addItsEnsemble()
        {
            this.add(listColumn, listLine, listSector);
            //this.calculateNewHipothesis();
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

