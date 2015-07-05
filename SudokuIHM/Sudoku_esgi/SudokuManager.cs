using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_esgi {
    public class SudokuManager {

        public string NomApplication { get; set; }
        public ObservableCollection<GridTest> GridList { get; set; }
        public GridTest GridSelect { get; set; }

        public SudokuManager() {
            NomApplication = "Application Sudokus";

            GridList = new ObservableCollection<GridTest>();
            GridList.Add(new GridTest { Name = "Grille 1 ", Date = "04/07/2015", Required = "123456789" });
            GridList.Add(new GridTest { Name = "Grille 2 ", Date = "16/06/2015", Required = "123456789ABCDEFG" });
            GridList.Add(new GridTest { Name = "Grille 3 ", Date = "01/01/2015", Required = "123456789ABCDEFGHIJKLMNOPQ" });

            foreach (GridTest g in GridList) {
                g.initGrid();
            }
        }
    }
}
