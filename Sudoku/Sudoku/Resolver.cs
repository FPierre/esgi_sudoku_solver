using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{

     

	public class Resolver
	{
		public List<CellsGrid> listHypotheticSudoku;
		public int index = 0;
        public CellsGrid currentgrid;
		public Resolver ()
		{
            listHypotheticSudoku = new List<CellsGrid>();
		}

		public void resolve (CellsGrid grid) {

			listHypotheticSudoku.Add(grid);
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


        public Dictionary<string, List<Cell>>  counBlockCells(CellsGrid grid)
        {
            Dictionary<string, List<Cell>> listOccurenHypothesis = new Dictionary<String, List<Cell>>();

            foreach (var cell in grid.grid)
            {
                if (cell.value.Equals("."))
                {
                    String str = cell.hypothesis.Aggregate((stringa, stringb) => stringa + stringb);

                    if (listOccurenHypothesis.ContainsKey(str))
                    {
                   
                        listOccurenHypothesis[str].Add(cell);
                    }
                    else
                    {
                        List<Cell> temp = new List<Cell>();
                        temp.Add(cell);
                        listOccurenHypothesis.Add(str,temp);

                    }
                }
            }
            return listOccurenHypothesis;
        }


        public List<KeyValuePair<string, List<Cell>>> sortedBlockCells(Dictionary<string, List<Cell>> dict)
        {
           List<KeyValuePair<string, List<Cell>>> myList = dict.ToList();
           myList.Sort(
               delegate(KeyValuePair<string, List<Cell>> firstpair,
               KeyValuePair<string, List<Cell>> nextPair)
               {
                        if(firstpair.Value.Count == nextPair.Value.Count)
                            return 0;
                        if (firstpair.Value.Count < nextPair.Value.Count)
                            return -1;

                            return 1;  
               }
            );

           return myList;
        }

        public List<Cell> getBlockCells(List<KeyValuePair<string, List<Cell>>> list)
        {
            
            bool doSomething = false;
            List<Cell> result = new List<Cell>();
            int i = 0;
            do
            {
                Cell[] cellTable = list[i].Value.ToArray();
                if (cellTable.Count() > 1)
                {
                    for (int j = 0, K = 1; K < cellTable.Count(); j++, K++)
                    {
                        if(cellTable[j].existInEnsembleOf(cellTable[K]))
                        {
                            result.Add(cellTable[j]);
                            result.Add(cellTable[K]);
                            doSomething = true;
                        }

                    }
                    if( i == (list.Count - 1 ) && doSomething == false)
                    {
                        Console.Out.WriteLine("Aucune Solution trouvé ");
                        Console.Out.WriteLine("Retour à la version précédente si existe");
                        doSomething = true;
                    }

                }


            }
            while (doSomething == false);


            return result;
        }

        public  void ResolveBlockCells(CellsGrid grid)
        {
             
            Dictionary<string, List<Cell>> counter = this.counBlockCells(grid);
             List<KeyValuePair<string, List<Cell>>> sortedCells =this.sortedBlockCells(counter);
            List<Cell> blockCells = this.getBlockCells(sortedCells);
            if(blockCells.Count == 0)
            {
                if(this.listHypotheticSudoku.Count > 0)
                    this.listHypotheticSudoku.Remove(this.listHypotheticSudoku.Last());
            }
            else
            {
                this.listHypotheticSudoku.Add(grid);
               int nbrHypothesis = blockCells.First().hypothesis.Count();
               for(int i = 0 ; i < nbrHypothesis ; i++)
               {
                   blockCells.First().value = blockCells.First().hypothesis[i];
                   grid.resolveGrid();
               }
            }
           
        }
      
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

