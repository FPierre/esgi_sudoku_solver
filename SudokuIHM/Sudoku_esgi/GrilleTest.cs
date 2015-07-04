using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_esgi {
    public class GridTest {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Required { get; set; }
        public int Size { get { return Required.Length; } }
        public CaseTest[,] Tab { get; set; }

        public void initGrid() {
            Tab = new CaseTest[Size, Size];
            Random rnd = new Random();
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    int rndResult = rnd.Next(Size + 1);
                    char v;
                    if (rndResult == Size) {
                        v = '.';
                    } else {
                        v = Required[rndResult];
                    }
                    Tab[i, j] = new CaseTest(v, Required);
                }
            }
        }

        public override string ToString() {
            return String.Format("{0} ({1})", Name, Date);
        }
    }
}
