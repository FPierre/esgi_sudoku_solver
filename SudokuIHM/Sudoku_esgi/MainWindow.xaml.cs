using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku_esgi {

    public partial class MainWindow : Window {

        private string ButtonName { get; set; }

        public MainWindow() {
            InitializeComponent();
            DataContext = App.SudokuManager;

            // Initialise l'IHM
            initIHM();
        }

        private void initIHM() {
            ActionSudoku.Content = "Validation";
        }

        private void StepByStepUnchecked(object sender, RoutedEventArgs e) {
            ActionStep.Visibility = Visibility.Hidden;
        }

        private void StepByStepChecked(object sender, RoutedEventArgs e) {
            ActionStep.Visibility = Visibility.Visible;
        }

        private void SelectModeChanged(object sender, SelectionChangedEventArgs e) {
            ComboBoxItem selectedMode = (ComboBoxItem) SelectMode.SelectedItem;
            ChangeButtonContent( selectedMode.Content.ToString() );
        }

        private void ChangeButtonContent(String s) {
            if ( ActionSudoku != null )
                ActionSudoku.Content = s;
        }

        private void SelectSudokuChanged(object sender, SelectionChangedEventArgs e) {
            ChangeSudokuGrid();
        }

        private void ChangeSudokuGrid() {
            GridSudoku.Children.Clear();
            GridSudoku.ColumnDefinitions.Clear();
            GridSudoku.RowDefinitions.Clear();
            CellsGrid g = App.SudokuManager.GridSelected;

            for (int i = 0; i < g.size; ++i) {
                GridSudoku.ColumnDefinitions.Add(new ColumnDefinition());
                GridSudoku.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < g.size; ++i) {
                for (int j = 0; j < g.size; j++) {
                    FrameworkElement b = CreateGridCase(g, i, j);
                    GridSudoku.Children.Add(b);
                }
            }
        }

        private FrameworkElement CreateGridCase(CellsGrid g, int i, int j) {
            FrameworkElement b;
            string s = g[i, j].Value;
            if (s == ".") {
                b = new Rectangle();
                ((Rectangle) b).Fill = new SolidColorBrush(Colors.Blue);
            } else {
                b = new Button();
                ((Button) b).Click += (c, e) => {
                    // 
                };
                ((Button) b).Content = s;
            }
            Grid.SetRow(b, i);
            Grid.SetColumn(b, j);
            return b;
        }

        private void TreatSudoku(object sender, RoutedEventArgs e) {
            // Traitement sur la grille
        }

        private void GoNextStep(object sender, RoutedEventArgs e) {
            // Traitement step-by-step
        }
    }
}
