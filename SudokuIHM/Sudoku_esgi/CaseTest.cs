using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_esgi {
    public class CaseTest {
        public char Value { get; set; }
        public string Hypothesis { get; set; }
        public CaseTest(char newValue, string newHypothesis) {
            Value = newValue;
            Hypothesis = newHypothesis;
        }
    }
}
