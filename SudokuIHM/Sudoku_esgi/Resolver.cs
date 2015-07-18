using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku_esgi
{
	public class Resolver :  SudokuObject, IObservable<SudokuObject>
	{
		public List<CellsGrid> listHypotheticSudoku;
		public int index = 0;

		public Resolver () : base()
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
                if (cell.Value.Equals("."))
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
                        Log(ModeText.Error, "Aucune Solution trouvé");

                        Log(ModeText.Error, "Retour à la version précédente si existe");

                        doSomething = true;
                    }

                }
                i++;
            }
            while ( i < list.Count && doSomething == false );


            return result;
        }


        private void deleteInListCells(List<Cell> cells , Cell cell, Cell nextCell)
        {

             cells=cells.FindAll(Mycell => !Mycell.Equals(cell) && !Mycell.Equals(nextCell));
             foreach (String hypothesisString in cell.hypothesis)
            {
                foreach (Cell tempCell in cells)
                {
                    tempCell.hypothesis.Remove(hypothesisString);
                }
            }
        }

        private void deleteAllHypothesisFromEnsembleInCell(List<Cell> blockCells,Cell c)
        {
           for(int i = 0 ; i < blockCells.Count -1 ; i++)
           {
               int j = i +1;
               if(blockCells[i].ExistsInEnsemble(blockCells[j].listColumn))
               {
                   deleteInListCells(blockCells[j].listColumn.cellsList, blockCells[i], blockCells[j]);
               }

               if (blockCells[i].ExistsInEnsemble(blockCells[j].listLine))
               {
                 
                   deleteInListCells(blockCells[j].listColumn.cellsList, blockCells[i], blockCells[j]);
               }

               if (blockCells[i].ExistsInEnsemble(blockCells[j].listSector))
               {
                   deleteInListCells(blockCells[j].listColumn.cellsList, blockCells[i], blockCells[j]);
               }
           }
        }



        public  CellsGrid ResolveBlockCells(CellsGrid grid)
        {
            List<CellsGrid> testList = new List<CellsGrid>();
            testList.Add(grid);
            testList.Add(new CellsGrid(grid));
            Log(ModeText.Verbose, "Save last version");

            Dictionary<string, List<Cell>> counter = this.counBlockCells(testList.Last());
            List<KeyValuePair<string, List<Cell>>> sortedCells =this.sortedBlockCells(counter);

            List<Cell> blockCells = this.getBlockCells(sortedCells);


            Log(ModeText.Verbose, "cellule bloquante");
            foreach(Cell c in blockCells)
            {
                Console.Out.WriteLine(c.hypothesis.Aggregate((stringa,stringb) => stringa + stringb));

                TextLog = String.Format("({0},{1})", c.PosX, c.PosY);
               // Console.ReadLine();
            }
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
                for(int i = 0 ; i < blockCells.Count-1 ; i++ )
                {

                    var cell = blockCells[i];
                    List<String> temp = new List<String>(cell.hypothesis);
                    testList.Last().cantResolve = false;
                   // testList.Add(tempgrid);
                    deleteAllHypothesisFromEnsembleInCell(blockCells, cell);
                    
                    
                    testList.Add(new CellsGrid( testList.Last().resolveGrid(false)));
                    Console.Out.WriteLine(testList.Last());
                    if (testList.Last().isDone())
                    {

                        return testList.Last();
                    }
                    
                    
                    foreach (String Hypothesis in temp)
                   {
                       testList.Add(new CellsGrid(testList.Last()));
                       Console.Out.WriteLine(testList.Last());
                       Cell realCell = testList.Last()[cell.PosX, cell.PosY];
                       testList.Last().cantResolve = false;
                       lastTextLogLevel = ModeText.Verbose;
                       TextLog = String.Format("test value {0} at  ({1},{2})", Hypothesis,realCell.PosX,realCell.PosY);
                       if (realCell == null)
                       {
                           Log(ModeText.Error,"cell is null");
                       }
                           realCell.Value = Hypothesis;
                           realCell.diffuseInItsEnsemble();
                           //Console.Out.WriteLine(testList.Last());
                        testList.Add( testList.Last().resolveGrid());


                        if (testList.Last().isDone())
                        {
                           // Console.WriteLine("Resolve");
                            return testList.Last();

                        }
                            if (testList.Last().cantResolve == true)
                           {
                               
                               testList.Remove(testList.Last());
                               testList.Remove(testList.Last());
                               testList.Last().cantResolve = true;
                               //this.listHypotheticSudoku.Remove(this.listHypotheticSudoku.Last());

 
                               Log(ModeText.Verbose, "RollBack");
                               // Console.Out.WriteLine(testList.Last());
                                
 
                              //  Console.In.ReadLine();
                              //  Console.In.ReadLine();
                               //realCell = tempCell;
                           }  
                        }

                    testList.Last().cantResolve = true;
                    
                    testList.Remove(testList.Last());
              }
           }
            if (!grid.isDone())
            {
                
                grid = testList.First();
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
								myCell.Value = hypothesisTest.ToString();
								if (grid.isDone ()) {
									Console.WriteLine ("solution trouvée!");
                                    grid.ToString();

								} else {
                                    Console.Out.WriteLine("recursive solution");
                                    Console.Out.WriteLine(grid);
                                    Console.ReadLine();
									RecursivebrowseGrid (grid, i, j);
								}
							} else {
								myCell.Value = ".";
							}
						}
					}
				}
			}
		}
	}
}

