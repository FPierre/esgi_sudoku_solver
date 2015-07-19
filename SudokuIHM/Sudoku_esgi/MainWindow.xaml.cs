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

    public partial class MainWindow : Window, IObserver<SudokuObject> {

        private int mode = 0;
        private bool stepByStep = false;
        public String file;
        public  ModeText modeLog = ModeText.Warning;

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

        private void StepByStepUnchecked(object sender, RoutedEventArgs e) {
            ButtonActionStep.Visibility = Visibility.Hidden;
        }

        private void StepByStepChecked(object sender, RoutedEventArgs e) {
            ButtonActionStep.Visibility = Visibility.Visible;
        }

        private void SelectModeChanged(object sender, SelectionChangedEventArgs e) {
            ComboBoxItem selectedMode = (ComboBoxItem) SelectMode.SelectedItem;
            mode = SelectMode.SelectedIndex;
            //App.SudokuManager = new SudokuManager(App.selectFile(), App.Console, SelectMode.SelectedIndex);

            if (ActionSudoku != null) {
                ActionSudoku.Visibility = Visibility.Visible;
                ButtonActionMode.Content = selectedMode.Content.ToString();
            }
        }

        private void TreatSudoku(object sender, RoutedEventArgs e) {
            if (mode == 0) {

            } else if (App.sudokuManager.GridSelected != null && mode == 1) {
               this.modeLog = ModeText.Verbose;
               Task task = new Task(new Action(App.sudokuManager.resolveSelected));
               task.Start();
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
    }
}
