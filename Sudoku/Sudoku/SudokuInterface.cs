using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    
    public class SudokuInterface
    {
        protected List<IObserver<SudokuInterface>> observers;
        private String textLog_;
   
       protected internal String TextLog
        {
            get
            {
                return textLog_;
            }
           set
            {
                textLog_ = value;
                observers.ForEach(observer => observer.OnNext(this));
            }
        }

       protected internal ModeText lastTextLogLevel;


        public SudokuInterface()
        {
            observers = new List<IObserver<SudokuInterface>>();
        }
        public IDisposable Subscribe(IObserver<SudokuInterface> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber<ConsoleMenu>(observers, observer);
        }

        public  void clearObservers()
        {
            observers.Clear();
        }


        public void Log(ModeText level,String text ,bool stepByStep )
        {
            bool ReadingMode = ConsoleMenu.StepByStep;
            ConsoleMenu.StepByStep = stepByStep;
            lastTextLogLevel = level;
            TextLog = text;
            ConsoleMenu.StepByStep = ReadingMode;
        }

    }

    internal class Unsubscriber<ConsoleMenu> : IDisposable
    {
        private List<IObserver<SudokuInterface>> _observers;
        private IObserver<SudokuInterface> _observer;

        internal Unsubscriber(List<IObserver<SudokuInterface>> observers, IObserver<SudokuInterface> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}



