
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Sudoku
{
    public class Ensemble : SudokuObject, IObservable<SudokuObject>
	{
       

		protected internal List<Cell> cellsList
		{ get; set; }
        public Ensemble(List<Cell> cellsList, List<IObserver<SudokuObject>> MainConsole) :base()
		{
            this.cellsList = new List<Cell>();
            foreach (IObserver<SudokuObject> observer in MainConsole)
            {
                this.observers.Add(observer);
            }
		}

        public Ensemble(List<IObserver<SudokuObject>> MainConsole)
            : this(new List<Cell>(),MainConsole)
        { 
        }

        public Ensemble( Ensemble e) : this(e.observers)
        {
            
            foreach(Cell cell in e.cellsList )
            {
                this.Add(cell);
            }

        }

        public void Add(Cell cell)
        {
            this.cellsList.Add(cell);
            this.diffuse(cell);   
        }

        
        
        

        public void diffuse(Cell cell)
        {
            if (!cell.Value.Equals("."))
            {
                //cell.hypothesis = null;
                List<Cell> tempCel = this.cellsList.FindAll(c => c.hypothesis.Contains(cell.Value));
                if (tempCel.Count != 0)
                {
                    this.Log(ModeText.Verbose,  String.Format("Remove Hypothesis {0}",cell.Value));
                    tempCel.ForEach(c =>RemoveValueFromHypothesis(cell,c) );
                    
                }
            }

      
            foreach (Cell c in this.cellsList)
            {
           
                if (cell.hypothesis.Contains(c.Value))
                {
                    cell.hypothesis.Remove(c.Value);
                }
            }
          
        }


        public void RemoveValueFromHypothesis(Cell cellWithValue , Cell c)
        {
            c.hypothesis.Remove(cellWithValue.Value);

            this.Log(ModeText.Verbose,c.ToString());
        }
        public bool ExistInEnsemble(Cell c)
        {
            if(!c.Value.Equals("."))
                return this.cellsList.Exists(cell => cell.Value.Equals(c.Value));
            return false;
        }


        private bool CellExistsInEnsemble(Cell cell, Cell cell2)
        {
            if(cell.Value.Equals(cell2.Value))
            {
                Log(ModeText.Error, "Deux cellules ont la même valeur");
                Log(ModeText.Error, cell.ToString());
                Log(ModeText.Error, cell2.ToString());
                return true;
            }
            return false;
        }

        public bool ExistsInEnsembleHypothesis(String value,Cell c)
        {
          
            if (!value.Equals("."))
            {
               var temp = this.cellsList.FindAll(cell => !cell.Equals(c));
               foreach(Cell tempCel in temp)
               {
                   if(tempCel.hypothesis.Contains(value))
                   {
                       return true;
                   }
               }
               
            }
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


        public override bool Equals(object obj)
        {
           
            return base.Equals(obj);
        }

	}
}

