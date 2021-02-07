using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{

    public static class SudSolver
    {
        //Delegate for a the sudoku solve function
        private delegate bool solver(Board board, int first, int last, bool IsFirstTime, DateTime t1);
        //static variable which will point the first solved board,if there is one
        public static Board SolvedBoard = null;

        /////////////////////////////////////FUNCTIONS/////////////////////////////////////

        /*
        * FUNCTION STATEMENT:The function creates  delegate for the solve function which solves the board
        * and executes them asynchronously making the solving more efficient and fast.
        * INPUT STATEMENT: Board 
        * OUTPUT STATEMENT: Return bool -> wether the board was solved or not 
        */
        public static bool SolveSud(Board board)
        {
            //if the recieved board is full
            if (board.BoardIsFull())
            {
                Console.WriteLine("Board is full");
                return true;
            }
            //build a cell 
            Cell MinCell = new Cell();
            //Gets the cell with the minimum filling options(in the most dense area)
            MinCell = GetMinOptionsIndex(board);
            //If there are no possibilities,then there is a cell with no filling options,return false
            if (MinCell.Possibilities.Count == 0)
            {
                Console.WriteLine("No solution");
                return false;
            }
            //build the solver delegate
            solver solver = new solver(SudSolver.Solve);
            //save the current time
            DateTime t1 = DateTime.Now;

            //The number of threads is set to the number of options the first minimum cell has
            int numberOfThreads = MinCell.Possibilities.Count;                     
            //List of IAsyncResult
            List<IAsyncResult> Threads = new List<IAsyncResult>();
            
            //Execute the solve function with the solver delegate sending each one of them the first running range,
            //BeginInvoke Executes the specified delegate asynchronously with Threads.
            //Builds and Run numbersOfThreads times the solve function asynchronously. 

            for (int i = 0; i < numberOfThreads; i++)
            {
                Board board2 = new Board(board);
                //Eeach function gets the first range for the first running
                IAsyncResult result = solver.BeginInvoke(board2, i * (MinCell.Possibilities.Count / numberOfThreads), (i + 1) * (MinCell.Possibilities.Count / numberOfThreads), true, t1, null, null);
                Threads.Add(result);
            }
            //Waits for Each of the solvers to finish
            for (int i = 0; i < MinCell.Possibilities.Count; i++)
                Threads[i].AsyncWaitHandle.WaitOne();


            //When the solvers are finished,
            //if the Solvedboard is not empty then the board has a solution,return true
            if (SolvedBoard != null)
            {
                SolvedBoard.PrintBoard();
                return true;
            }
            //If the board has no solution return false
            Console.WriteLine("No solution");
            return false;
        }
        /*
        * FUNCTION STATEMENT: Execute the SolveSudoku function and for a solved board it prints
        * the solution with the running time.
        * INPUT STATEMENT: board,first,last,IsFirstTime,DateTime t1 which has the time we started solving
        * OUTPUT STATEMENT: bool true/false for solve/unsolved board
        */
        private static bool Solve(Board board, int first, int last, bool IsFirstTime, DateTime t1)
        {
            //Executes the SolveSudoku function which tries to solve the board
            bool result = SudSolver.SolveSudoku(board, first, last, IsFirstTime);
            
            //Checks if the board was solved
            if (result)
            {
                DateTime t2 = DateTime.Now;
                //Sets the static isSolved variable to true
                board.SetAsSolved();
                //Calculates the run time of the Solve function
                TimeSpan RunTime = t2 - t1;
                Console.WriteLine("Run Time: " + RunTime);
               
                //Sets the Solvedboard to the current board
                SolvedBoard = board;
                return true;
            }
            else
            {
                //In case of no solution
                return false;
            }
            
        }

        /*
         * FUNCTION STATEMENT: Recursive function that tries to solve the given sudoku board with BackTracking algorithm
         * and Filling by density algorithm -> Each time we search for the cell with the minimum options 
         * and try backtracking on it's possibilities.
         * INPUT STATEMENT: Sudoku board,(first,last,IsFirstTime)->because the function 
         * run in a thread then for the first time running ,the function will run only in the given range(first to last).
         * OUTPUT STATEMENT:Returns bool --> returns True/False for Solvable/Unsolvable board. 
        */
        private static bool SolveSudoku(Board board, int first, int last, bool IsFirstTime)
        {
            //If any other thread as already solved the board,stop trying and return false
            if (SolvedBoard != null)
            {
                return false;
            }
            /*
             * Stop Condition:
             * If the board is full then the board is solved -> return true
            */
            if (board.BoardIsFull())
            {
                return true;
            }
            //Gets the length of the board
            int board_length = board.Getlength();
            //Gets the Sudoku board
            int[,] game_board = board.GetBoard();
            //Gets the Rows,Cols and cubes matrixes
            char[,] game_rows = board.GetRows();
            char[,] game_columns = board.GetColumns();
            char[,] game_cubes = board.GetCubes();

            //build a cell 
            Cell MinCell = new Cell();
            //Gets the cell with the minimum filling options(in the most dense area)
            MinCell = GetMinOptionsIndex(board);
            //If there are no possibilities,then there is a cell with no filling options,return false
            if (MinCell.Possibilities.Count == 0)
            {
                return false;
            }
            int row = MinCell.Row;
            int col = MinCell.Col;
            int num;
            //if the current function was called recursivly then change the given range to a full range.
            //A full range means running over all the possibilities.
            if (!IsFirstTime)
            {
                first = 0;
                last = MinCell.Possibilities.Count;
            }
            //Runing over the given range
            for (int i = first; i < last; i++)
            {
                //Gets the current filling option from the possibilities list.
                num = MinCell.Possibilities[i];
                //Do move
                game_board[row, col] = num;
                //Populate the rows,columns and cubes matrixes with '1' in the position we filled
                game_rows[row, num - 1] = '1';
                game_columns[num - 1, col] = '1';
                game_cubes[GetCubeNumber(board.Getlength(), row, col), num - 1] = '1';
                //Executes the recursive function -> if the function returned true the board is solved -> return true also
                if (SolveSudoku(board, first, last, false)) return true;
                //In case the function returned false it means the current number isn't good
                //and we need to un do the move and try the next number
                //UnDo Turn
                game_board[row, col] = 0;
                game_rows[row, num - 1] = '0';
                game_columns[num - 1, col] = '0';
                game_cubes[GetCubeNumber(board.Getlength(), row, col), num - 1] = '0';

            }
            //If none of the filling options was okey,return false
            return false;

        }
        /*
         * FUNCTION STATEMENT:The function Search for the cell with the minimum filling options  
         * INPUT STATEMENT: Board board
         * OUTPUT STATEMENT:Returns Cell -> the cell with the minimum options in the board
        */
        private static Cell GetMinOptionsIndex(Board board)
        {
            Cell min = new Cell();
            //filling the row of the cell ilegal row using it as a flag for the first running
            min.Row = board.Getlength() + 1;
            //Runs over the board
            for (int i = 0; i < board.Getlength(); i++)
                for (int j = 0; j < board.Getlength(); j++)
                    //Checks if the cell is empty
                    if (board.GetBoard()[i, j] == 0)
                    {
                        //Get the list of filling options of the current position
                        List<int> currentPos = Get_Possibilities(board, i, j);
                        //If its the first running
                        if (min.Row == board.Getlength() + 1)
                            min = new Cell(i, j, currentPos);
                        //If the min cell has more possibilities then the current position,update it's values
                        if (min.Possibilities.Count > currentPos.Count)
                        {
                            min.Row = i;
                            min.Col = j;
                            min.Possibilities = currentPos;
                        }
                    }
            //Return the min cell
            return min;

        }
        /*
         * FUNCTION STATEMENT: The Function search for the possible numbers to fill for the given position
         * INPUT STATEMENT: Sudoku board,(row,col)->position
         * OUTPUT STATEMENT:Returns List of the possible moves the given position has
        */
        private static List<int> Get_Possibilities(Board board, int row, int col)
        {
            //Gets the length of the board
            int board_length = board.Getlength();
            List<int> possibleNumbers = new List<int>();    
            //Run over the possibilities
            for (int i = 1; i <= board_length; i++)
            {
                if (IsLegal(board, row, col, i))
                {
                    //If the number is legal add it to the list
                    possibleNumbers.Add(i);
                }
            }
            return possibleNumbers;
        }

        /*
        * FUNCTION STATEMENT: The Function Calculates Cube number for the given location
        * INPUT STATEMENT: Length of the board, row and column that represents a location
        * OUTPUT STATEMENT: Returns the appropriate cube for the location
        */
        public static int GetCubeNumber(int length, int row, int col)
        {
            //Length of a single cube
            double lengthOfCube = Math.Sqrt(length);
            //Number of cube horizontally 
            int col_pos = (int)(col / lengthOfCube);
            //Number of cube transversely
            int row_pos = (int)(row / lengthOfCube);
            //calculates the cube number of cube when the first cube is 0 and the last is 8
            int CubeNumber = col_pos + (row_pos * (int)lengthOfCube);

            return CubeNumber;
        }

        /*
        * FUNCTION STATEMENT: The Function Checks wether its legal to insert the number in the current position
        * INPUT STATEMENT: Sudoku board || position--> row,col ||num -> the wanted number
        * OUTPUT STATEMENT:Returns bool --> Returns True/False for Legal/Illegal number
        */
        private static bool IsLegal(Board board, int row, int col, int num)
        {
            //Gets the cube number of the given position
            int CubeNum = GetCubeNumber(board.Getlength(), row, col);
            char[,] cubes = board.GetCubes();

            //Checks if the number exists in the row,col or cube of the given position
            if (board.GetRows()[row, num - 1] == '1' || board.GetColumns()[num - 1, col] == '1' || cubes[CubeNum, num - 1] == '1')
            {
                //if exist return false
                return false;
            }
            //if not exist return true
            return true;

        }


    }

}
