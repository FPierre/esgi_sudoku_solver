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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku_esgi {

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            DataContext = App.SudokuManager;
        }

        private void StepByStepUnchecked(object sender, RoutedEventArgs e) {
            ActionStep.Visibility = Visibility.Hidden;
        }

        private void StepByStepChecked(object sender, RoutedEventArgs e) {
            ActionStep.Visibility = Visibility.Visible;
        }

        private void SelectModeChanged(object sender, SelectionChangedEventArgs e) {
            ComboBoxItem selectedMode = (ComboBoxItem) SelectMode.SelectedItem;
            ChangeButtonContent(selectedMode.Content.ToString());
        }

        private void ChangeButtonContent(String s) {
            if (ActionSudoku != null) {
                ActionSudoku.Visibility = Visibility.Visible;
                ActionSudoku.Content = s;
            }
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
                    FrameworkElement elem = CreateGridCase(g, i, j);
                    GridSudoku.Children.Add(elem);
                }
            }
        }

        private FrameworkElement CreateGridCase(CellsGrid g, int i, int j) {
            FrameworkElement elem;
            string s = g[i, j].Value;
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

        private void TreatSudoku(object sender, RoutedEventArgs e) {
            // Traitement sur la grille
        }

        private void GoNextStep(object sender, RoutedEventArgs e) {
            // Traitement step-by-step
        }
    }
}
