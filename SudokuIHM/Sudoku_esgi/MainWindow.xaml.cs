using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

    public partial class MainWindow : Window ,IObserver<SudokuObject> {

        private int mode = 0;
        private bool stepByStep = false;
        public String file;

        SudokuManager sudokuManager;


        public ObservableCollection<String> Logs { get; set; }
        public ObservableCollection<CellsGrid> ModelList { get; set; }
 

        public MainWindow() {
            InitializeComponent();
            Logs = new ObservableCollection<string>();
            ModelList = new ObservableCollection<CellsGrid>();

            if (OpenFile())
            {
                DataContext = sudokuManager;
                foreach( CellsGrid grid in  sudokuManager.ModelList)
                {
                    ModelList.Add(grid);
                }
            }
            else
            {
                this.Close();
            }
        }


        public bool OpenFile()
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                file = openFileDialog.FileName;
                if(file.EndsWith(".sud"))
                {

                    sudokuManager = new SudokuManager(file, this, 0);
                   return true;
                }
            }

            return false;
        }


        private void StepByStepUnchecked(object sender, RoutedEventArgs e) {
            ActionStep.Visibility = Visibility.Hidden;
        }

        private void StepByStepChecked(object sender, RoutedEventArgs e) {
            ActionStep.Visibility = Visibility.Visible;
        }

        private void SelectModeChanged(object sender, SelectionChangedEventArgs e) {
            ComboBoxItem selectedMode = (ComboBoxItem) SelectMode.SelectedItem;
            mode = SelectMode.SelectedIndex;
            //App.SudokuManager = new SudokuManager(App.selectFile(), App.Console, SelectMode.SelectedIndex);

            if (ActionSudoku != null) {
                ActionSudoku.Visibility = Visibility.Visible;
                ActionSudoku.Content = selectedMode.Content.ToString();
            }
        }

        private void TreatSudoku(object sender, RoutedEventArgs e) {
            if (mode == 0) {

            } else if (sudokuManager.GridSelected != null && mode == 1) {
                sudokuManager.resolveSelected(); 
            } else
                MessageBox.Show("Tu dois d'abord sélectionner un sudoku.");
        }

        private void GoNextStep(object sender, RoutedEventArgs e) {
            // Traitement step-by-step
        }

        private void SelectSudokuChanged(object sender, SelectionChangedEventArgs e) {
            ChangeSudokuGrid();
        }

        private void ChangeSudokuGrid() {
            GridSudoku.Children.Clear();
            GridSudoku.ColumnDefinitions.Clear();
            GridSudoku.RowDefinitions.Clear();
            CellsGrid g = sudokuManager.GridSelected;

            for (int i = 0; i < g.size; ++i) {
                GridSudoku.ColumnDefinitions.Add(new ColumnDefinition());
                GridSudoku.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < g.size; ++i) {
                for (int j = 0; j < g.size; j++) {
                    FrameworkElement elem = CreateGridCase( g[i, j].Value, i, j );
                    GridSudoku.Children.Add(elem);
                }
            }
        }

        private FrameworkElement CreateGridCase(string s, int i, int j) {
            FrameworkElement elem;
            if (s != ".") {
                elem = new TextBox();
                TextBox rect = (TextBox) elem;
                rect.Style = (Style) FindResource("RedBlockOpacity");
                rect.Text = s;
            } else {
                elem = new Button();
                Button btn = (Button) elem;
                btn.Style = (Style) FindResource("RedButton");
                btn.Click += (c, e) => {
                    // 
                };

                btn.Content = s;
            }
            Grid.SetRow(elem, i);
            Grid.SetColumn(elem, j);
            return elem;
        }

        public void OnNext(SudokuObject currentObject)
        {
            Logs.Add(currentObject.TextLog);
        }



        // Usually called when a transmission is complete. Not implemented.
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        // Usually called when there was an error. Didn't implement.
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
        
        public void UpdateGridCase(string s, int i, int j) {
            FrameworkElement elem = GridSudoku.Children.OfType<FrameworkElement>()
                                    .FirstOrDefault(child => Grid.GetRow(child) == i && Grid.GetColumn(child) == j);
            GridSudoku.Children.Remove(elem);
            elem = CreateGridCase(s, i, j);
            GridSudoku.Children.Add(elem);
        }
    }
}
