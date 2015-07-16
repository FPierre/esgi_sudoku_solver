using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_esgi
{
    
    public class SudokuObject
    {
        protected List<IObserver<SudokuObject>> observers;
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


        public SudokuObject()
        {
            observers = new List<IObserver<SudokuObject>>();
        }
        public IDisposable Subscribe(IObserver<SudokuObject> observer)
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


        public void Log(ModeText level,String text )
        {
            lastTextLogLevel = level;
            TextLog = text;
        }

    }

    internal class Unsubscriber<ConsoleMenu> : IDisposable
    {
        private List<IObserver<SudokuObject>> _observers;
        private IObserver<SudokuObject> _observer;

        internal Unsubscriber(List<IObserver<SudokuObject>> observers, IObserver<SudokuObject> observer)
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



