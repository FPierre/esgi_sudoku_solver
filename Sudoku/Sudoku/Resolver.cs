﻿using System;
using System.Collections.Generic;

namespace Sudoku
{
	public class Resolver
	{
		public Dictionary<int, CellsGrid> listHypotheticSudoku;
		public int index = 0;

		public Resolver ()
		{
            listHypotheticSudoku = new Dictionary<int, CellsGrid>();
		}

		public void resolve (CellsGrid grid) {

			listHypotheticSudoku.Add(index, grid);
			this.index++;
			RecursivebrowseGrid (grid, 0, 0);

		}

//		public Cell RecursivebrowseGrid (CellsGrid grid, int line, int column) {
//			int hypothesisTest = 0;
//			for (int line = 0; line < grid.size; line++) {
//				for (int column = 0; line < grid.size; column++) {
//					if (grid [line] [column].valueIsNull) {
//						grid [line] [column].hypothesis [hypothesisTest];
//					}
//				}
//			}
//		}
		public void RecursivebrowseGrid (CellsGrid grid, int line, int column) {

			for (int i = line; i < grid.size; i++) {
				for (int j = column; j < grid.size; j++) {
					if (grid[i,j].ValuesIsNull()) {
						Cell myCell = grid [i,j];
						for (int h = 0; h < myCell.hypothesis.Count; h++) {
                           
							int hypothesisTest = Convert.ToInt32( myCell.hypothesis[h]);
                            if (myCell.ExistsInEnsemble(hypothesisTest) == false)
                            {
								myCell.value = hypothesisTest.ToString();
								if (grid.isDone ()) {
									Console.WriteLine ("solution trouvée!");
									grid.ToString ();

								} else {
                                    Console.Out.WriteLine("recursive solution");
                                    Console.Out.WriteLine(grid);
                                    Console.ReadLine();
									RecursivebrowseGrid (grid, i, j);
								}
							} else {
								myCell.value = ".";
							}
						}
					}
				}
			}
		}
	}
}

