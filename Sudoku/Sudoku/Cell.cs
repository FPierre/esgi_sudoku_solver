﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Sudoku
{
	public class Cell : SudokuInterface, IObservable<SudokuInterface>
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
        private String _value;
        protected internal String Value
        {

            get
            {
                return this._value;

            }
            set
            {
                this._value = value;
                if( this.hypothesis != null &&this.hypothesis.Count > 0 && !value.Equals("."))
                    this.hypothesis.RemoveRange(0,this.hypothesis.Count);

                    this.Log(ModeText.Verbose, this.ToString(), false);
            }
        }

        internal int PosX;
        internal int PosY;

        protected Cell(List<IObserver<SudokuInterface>> MainConsole)
            : base()
        {
            foreach (IObserver<SudokuInterface> observer in MainConsole)
            {
                this.observers.Add(observer);
            }
        }

        public Cell(Ensemble listColumn, Ensemble listLine, Ensemble listSector, String value, List<String> hypothesis, int posx, int posy, List<IObserver<SudokuInterface>> MainConsole)
            : this( MainConsole)
		{
  
			this.listColumn = listColumn;
			this.listLine = listLine;
			this.listSector = listSector;
			
			this.hypothesis = new List<String>(hypothesis);
            this.PosX = posx;
            this.PosY = posy;
            this.Value = value;
            
            
		}

        private Cell(String value, List<String> hypothesis,List<IObserver<SudokuInterface>> MainConsole)
            : base()
		{
            foreach(IObserver<SudokuInterface> observer in MainConsole)
            {
                base.Subscribe(observer);
            }
            
            this.Value = value;
            this.hypothesis = hypothesis;
		}

        private Cell(String value, String hypothesis, List<IObserver<SudokuInterface>> MainConsole)
            : this(value,new List<String>(Utility.SplitWithSeparatorEmpty(hypothesis)),MainConsole)
        {
   
        }

    

        private Cell(Cell cell, string hypothesis, IObserver<SudokuInterface> MainConsole) : this(cell.Value,new List<String>(Utility.SplitWithSeparatorEmpty(hypothesis)),cell.observers)
        {

        }


		public bool ExistsInEnsemble(params Ensemble[] t)
		{
            bool exists = false;
            int i = 0;
            while(i < t.Length && exists == false)
            {
                if (t[i].ExistInEnsemble(this))
                {
                    exists = true;
                    break;
                    
                }
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

        public void diffuseInItsEnsemble()
        {
            this.diffuse(listColumn, listLine, listSector);
        }

        private void diffuse(params Ensemble[] t)
        {
            foreach (Ensemble monEnsemble in t)
            {
                monEnsemble.diffuse(this);
            }
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
            return this.Value.Equals(".");
        }

        public bool ExistsInEnsemble( int value)
        {

            Cell tempCell = new Cell(Convert.ToString(value),new List<string>(),this.observers);
            List<Ensemble> TempListEnsemble = new List<Ensemble>();
            TempListEnsemble.Add(this.listColumn);
            TempListEnsemble.Add(this.listLine);
            TempListEnsemble.Add(this.listSector);

           return TempListEnsemble.Any(e => e.ExistInEnsemble(tempCell));
        }




        internal bool existInEnsembleOf(Cell cell)
        {


            if(cell.listColumn == this.listColumn)
                return true;

            if(cell.listLine == this.listLine)
                return true;

            if(cell.listSector == this.listSector)
                return true;

            return false;
        }


        public override bool Equals(object obj)
        {
          
            return base.Equals(obj);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat(" Value : {0} {1}",this.Value, Environment.NewLine);
            if(hypothesis.Count > 0)
                result.AppendFormat(" Hypothesis : {0} {1}", this.hypothesis.Aggregate((stringa, stringb) => stringa + stringb),Environment.NewLine);
            result.AppendFormat(" Pos : [{0},{1}] {2}", this.PosX, this.PosY, Environment.NewLine);

            return result.ToString();
        }


    }
}

