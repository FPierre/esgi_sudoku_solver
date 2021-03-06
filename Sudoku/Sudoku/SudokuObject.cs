﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{

    public class SudokuObject : INotifyPropertyChanged
    {
        protected List<IObserver<SudokuObject>> observers;
        private string textLog_;
   
       public string TextLog
        {
            get
            {
                return textLog_;
            }
           set
            {
                textLog_ = value;
                observers.ForEach(observer => observer.OnNext(this));
                NotifyPropertyChanged("Logs");
            }
        }

       public  ModeText lastTextLogLevel;


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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
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



