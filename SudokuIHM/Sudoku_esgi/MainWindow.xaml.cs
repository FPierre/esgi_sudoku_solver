using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Sudoku;

namespace Sudoku_esgi {

    public partial class MainWindow : Window, IObserver<SudokuObject> {

        private bool threadingMode = false;
        public String file;
        public ModeText modeLog = ModeText.Warning;

        public MainWindow() {
            InitializeComponent();
            this.modeLog = ModeText.Error;
            if (OpenFile()) {
                DataContext = App.sudokuManager;
            } else {
                this.Close();
            }
        }

        public bool OpenFile() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                file = openFileDialog.FileName;

                if(file.EndsWith(".sud")) {
                    App.sudokuManager = new SudokuManager(file, this, 0);
                    return true;
                }
            }

            return false;
        }

        private void AsyncUnchecked(object sender, RoutedEventArgs e) {
            threadingMode = false;
        }

        private void AsyncChecked(object sender, RoutedEventArgs e) {
            threadingMode = true;
        }

        private void TreatSudoku(object sender, RoutedEventArgs e) {
            if (App.sudokuManager.GridSelected != null) {
                this.modeLog = ModeText.Verbose;

               if (threadingMode) {
                   ListLogs.DataContext = App.sudokuManager.GridSelected;
                   Task task = new Task(new Action(App.sudokuManager.resolveSelected));
                   task.Start();
               } else {
                   Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                       new Action(() => { App.sudokuManager.resolveSelected(); })
                   );
               }

            } else
                MessageBox.Show("Tu dois d'abord sélectionner un sudoku.");
        }

        private void SelectSudokuChanged(object sender, SelectionChangedEventArgs e) {
            ChangeSudokuGrid();
        }

        private void ChangeSudokuGrid() {
            GridSudoku.Children.Clear();
            GridSudoku.ColumnDefinitions.Clear();
            GridSudoku.RowDefinitions.Clear();

            for (int i = 0; i < App.sudokuManager.GridSelected.size; ++i) {
                GridSudoku.ColumnDefinitions.Add(new ColumnDefinition());
                GridSudoku.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < App.sudokuManager.GridSelected.size; ++i) {
                for (int j = 0; j < App.sudokuManager.GridSelected.size; j++) {
                    FrameworkElement elem = CreateGridCase(App.sudokuManager.GridSelected[i, j], i, j);
                    GridSudoku.Children.Add(elem);
                }
            }
        }

        private FrameworkElement CreateGridCase(Cell c, int i, int j) {
            FrameworkElement elem;
            if (c.Value != ".") {
                elem = new TextBox();
                TextBox rect = (TextBox) elem;
                rect.Style = (Style) FindResource("RedBlockOpacity");
                elem.DataContext = c;
                Binding myBinding = new Binding();
                myBinding.Source = c;
                myBinding.Path = new PropertyPath("Value");
                myBinding.Mode = BindingMode.OneWay;
                myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(rect, TextBox.TextProperty, myBinding);
  
            } else {
                elem = new Button();
                Button btn = (Button) elem;
                btn.Style = (Style) FindResource("RedButton");
                /*btn.Click += (c, e) => {
                    // 
                };
                 * */
                elem.DataContext = c;
                Binding myBinding = new Binding();
                myBinding.Source = c;
                myBinding.Path = new PropertyPath("Value");
                myBinding.Mode = BindingMode.OneWay;
                myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(btn, Button.ContentProperty, myBinding);
                
            }
            Grid.SetRow(elem, i);
            Grid.SetColumn(elem, j);
            return elem;
        }

        public void OnNext(SudokuObject currentObject) {
            if (this.modeLog <= currentObject.lastTextLogLevel) {
                try {
                    App.sudokuManager.logs.Add(currentObject.TextLog);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }
        
        public void OnCompleted() { throw new NotImplementedException(); }

        public void OnError(Exception error) { throw new NotImplementedException(); }
    }
}
