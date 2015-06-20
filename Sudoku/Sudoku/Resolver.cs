using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{

     

	public class Resolver
	{
		public List<CellsGrid> listHypotheticSudoku;
		public int index = 0;

		public Resolver ()
		{
            listHypotheticSudoku = new List<CellsGrid>();
		}

        public Resolver(Resolver resolver) : this()
        {
            // TODO: Complete member initialization
            foreach(CellsGrid c in resolver.listHypotheticSudoku)
            {
                this.listHypotheticSudoku.Add(c);
            }
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

                    if (cell.hypothesis.Count > 0)
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
                            listOccurenHypothesis.Add(str, temp);

                        }
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
                       
                        if(firstpair.Key.Length == nextPair.Key.Length)
                            return 0;
                        if (firstpair.Key.Length < nextPair.Key.Length)
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

                i++;
            }
            while ( i < list.Count && doSomething == false );


            return result;
        }

        public  CellsGrid ResolveBlockCells(CellsGrid grid)
        {
             
            Dictionary<string, List<Cell>> counter = this.counBlockCells(grid);
            List<KeyValuePair<string, List<Cell>>> sortedCells =this.sortedBlockCells(counter);
            
            
            List<Cell> blockCells = this.getBlockCells(sortedCells);
            
            if(blockCells.Count == 0)
            {
                if (this.listHypotheticSudoku.Count > 0)
                {
                   // grid = this.listHypotheticSudoku.Last();
                 //   this.listHypotheticSudoku.Remove(this.listHypotheticSudoku.Last());
                    grid.cantResolve = true;
                }
            }
            else
            {


                CellsGrid tempgrid = new CellsGrid(grid);
                this.listHypotheticSudoku.Add(tempgrid);

                List<CellsGrid> testList = new List<CellsGrid>();
                testList.Add(tempgrid);
               List<String> temp = new List<String>(blockCells.First().hypothesis);
              
               foreach (String Hypothesis in temp)
               {
                   
                   if(grid.Exists( blockCells.First()))
                   {

                       Cell realCell = grid.Find(blockCells.First());
                       Cell tempCell = realCell;
                       Console.WriteLine("test value {0}", Hypothesis);
                       if(realCell == null)
                        Console.WriteLine("cell is null");
                       realCell.value = Hypothesis;
                      // realCell.value = Hypothesis;
                       realCell.diffuseInItsEnsemble();

                       Console.Out.WriteLine(grid);
                        grid = grid.resolveGrid();
                       if (grid.isDone())
                           break;
                       if(grid.cantResolve == true)
                       {
                           grid = testList.Last();
                           //this.listHypotheticSudoku.Remove(this.listHypotheticSudoku.Last());

                          counter = this.counBlockCells(grid);
                          sortedCells = this.sortedBlockCells(counter);


                           blockCells = this.getBlockCells(sortedCells);
                           Console.Out.WriteLine("RollBack");
                            Console.Out.WriteLine(grid);
                           
                           //realCell = tempCell;
                       }
                   }
                  
               }
               if (!grid.isDone())
                   grid.cantResolve = true;
            }

            return grid;
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

