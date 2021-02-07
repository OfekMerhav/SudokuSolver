using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    public class Board
    {
        //int matrix that represents the sudoku board 
        private int[,] board;
        //length of the board |->| the square root of the board area
        private int length;
      
        /*
         * The rows,columns and cubes are char matrixes and were created in order to improve 
         * the efficiency of the solve function by reducing as possible the use of storage(there for char matrixes). 
         * these matrixes make the "isLegal" function - A function 
         * that runs each step in the recursive function and make it from O(n) runnning time complications to O(1)  
         * 
         * For Example:
         *    0 1 2 3 4 5 6 7 8 9
         * 0 |0|0|0|1|0|0|0|0|0|0|
         * 1 |0|0|0|0|0|0|0|0|0|0|
         * 
         * For each matrix:
         * rows matrix: in row 0 number 4 exists
         * columns matrix: in column 3 number 1 exists
         * cubes matrix: in cube 0 number 4 exists
        */
        private char[,] rows;
        private char[,] columns;
        private char[,] cubes;

        //A static variable for determining wether any of the boards that was created is solved 
        private static bool isSolved;

        //////////////////////////////////FUNCTIONS/////////////////////////////////////

        /*
         * FUNCTION STATEMENT: Constructor for Board class
         * INPUT STATEMENT: board_values which represents the board in a string format
         * OUTPUT STATEMENT: None
        */
        public Board(string board_values)
        { 
            isSolved = false;
            //Length -> the length of the side of the board
            this.length = (int)Math.Sqrt(board_values.Length);
            //Initialzing the Board matrix and the row,col,cube matrixes
            this.board = new int[length, length];
            this.rows = new char[length, length];
            this.columns = new char[length, length];
            this.cubes = new char[length, length];

            for (int i = 0; i < length; i++)       
               for (int j = 0; j < length; j++)
                  //Inserts each value in the string board to his place in the matrix board by converting to numbers
                  this.board[i, j] = (board_values[(i * length) + j] - '0');
            
            for (int row = 0; row < length; row++)
               for (int col = 0; col < length; col++)
               { 
                   //Populate the matrixes with '0' in each empty cell
                   rows[row, col] = '0';
                   columns[row, col] = '0';
                   cubes[row, col] = '0';
                 
               }
            
            for (int row = 0; row < length; row++)
               for (int col = 0; col < length; col++)
                  //Checks for filled cells in the board
                  if (board[row, col] != 0)
                  {
                      if (rows[row, board[row, col] - 1] != '1' && columns[board[row, col] - 1, col] != '1' && cubes[SudSolver.GetCubeNumber(this.Getlength(), row, col), board[row, col] - 1] != '1')
                      {
                          //Populate the matrixes with '1' in the required places for each number in the board
                          rows[row, board[row, col] - 1] = '1';
                          columns[board[row, col] - 1, col] = '1';
                          cubes[SudSolver.GetCubeNumber(this.Getlength(), row, col), board[row, col] - 1] = '1';
                      }
                      else
                      {
                          throw new ArgumentException("Board was found invalid due to violating sudoku game rules"); 
                      }
                  }
            

        }
        
        /*
         * FUNCTION STATEMENT: Copy Constructor for Board class
         * INPUT STATEMENT: Board class  
         * OUTPUT STATEMENT: None
        */
        public Board(Board board)
        {
            isSolved = false;
            //Length -> the size of the square
            this.length = board.Getlength();
            //Initialzing the Board matrix
            this.board = new int[board.Getlength(), board.Getlength()];

            this.rows = new char[length, length];
            this.columns = new char[length, length];
            this.cubes = new char[length, length];

            //Get the game board 
            int[,] game_board = board.GetBoard();
            //Get the flag matrixes
            char[,] game_rows = board.GetRows();
            char[,] game_columns = board.GetColumns();
            char[,] game_cubes = board.GetCubes();

            //Copy each cell from the recieved board into the new one
            for (int row = 0; row < length; row++)
               for (int col = 0; col < length; col++)
               {
                   this.board[row, col] = game_board[row, col];
                   this.rows[row, col] = game_rows[row, col];
                   this.columns[row, col] = game_columns[row, col];
                   this.cubes[row, col] = game_cubes[row, col];
               }
            
        }

        /*
       * FUNCTION STATEMENT: The Function Checks wether the Board string is valid or not 
       * INPUT STATEMENT: Sudoku Board represented by a string
       * OUTPUT STATEMENT: bool --> Returns True/False for Valid/Invalid board
       */
        public static bool IsBoardValid(string board)
        {
            if (board.Length == 0) return false;
            //Checks wether the board has a valid size 
            double cube_length = Math.Sqrt(Math.Sqrt(board.Length));
            if (!(cube_length % 1 == 0))
                return false;

            int number = 0;
            double board_length = Math.Sqrt(board.Length);
            //Checks wether the board has valid values
            for (int i = 0; i < board.Length; i++)
            {
                number = board[i] - '0';
                if (number < 0 || number > board_length)
                    return false;
            }
            //if no validation error was found,return true
            return true;
        }
        /*
        * FUNCTION STATEMENT: The Function checks if the board is full or not
        * INPUT STATEMENT: Sudoku board
        * OUTPUT STATEMENT: Bool --> Returns True/Flase for a full/not full board
        */
        public bool BoardIsFull()
        {
            int board_length = this.Getlength();
            int[,] game_board = this.GetBoard();
            //Runs over the board
            for (int row = 0; row < board_length; row++)
               for (int col = 0; col < board_length; col++)
                  //If an empty cell was found return false
                  if (game_board[row, col] == 0)
                  {
                      return false;
                  }
               
            
            //If none of the cells are empty return true 
            return true;
        }
        /*
        * FUNCTION STATEMENT: prints the Sudoku Board
        * INPUT STATEMENT: None
        * OUTPUT STATEMENT: None
        */
        //public void PrintBoard()
        //{
        //    int[,] board = this.board;
        //    for (int i = 1; i <= length / 3; ++i)
        //        Console.Write("+--------------");
        //    Console.WriteLine("+");
        //    for (int i = 1; i <= length; ++i)
        //    {
        //        for (int j = 1; j <= length; ++j)
        //            if (board[i - 1, j - 1] > 9)
        //                Console.Write("| {0} ", board[i - 1, j - 1]);
        //            else
        //                Console.Write("| {0}  ", board[i - 1, j - 1]);

        //        Console.WriteLine("|");
        //        if (i % Math.Sqrt(length) == 0)
        //        {
        //            for (int h = 1; h <= length / 3; h++)
        //                Console.Write("+--------------");
        //            Console.WriteLine("+");
        //        }
        //    }
        //}
       /*
        * FUNCTION STATEMENT: prints the Sudoku Board
        * INPUT STATEMENT: None
        * OUTPUT STATEMENT: None
        */
        public void PrintBoard()
        {

            int i = 0, j = 0;
            Console.WriteLine();
            Console.Write("      ");
            for (j = 0; j < length; j++)
            {
                if (j / 10 != 0)
                    Console.Write("  " + j + "  ");
                else
                    Console.Write("  " + j + "   ");

            }
            Console.Write("\n      ");
            for (j = 0; j < length; j++)
            {
                for (j = 1; j <= length; j++)
                    Console.Write("_____ ");

                for (i = 0; i < length; i++)
                {
                    Console.Write("\n     |");
                    for (j = 0; j < length; j++)
                    {
                        Console.Write("     |");

                    }
                    if (i / 10 != 0)
                        Console.Write("\n  " + i + " |");
                    else
                        Console.Write("\n  " + i + "  |");
                    for (j = 0; j < length; j++)
                    {
                        if (board[i, j] / 10 != 0)
                            Console.Write("  " + board[i, j] + " ");
                        else
                            Console.Write("  " + board[i, j] + "  ");
                        Console.Write("|");

                    }
                    Console.Write("\n     |");
                    for (j = 0; j < length; j++)
                    {
                        Console.Write("_____|");
                    }
                }
                Console.WriteLine();

            }

        }
        

        /*
        * FUNCTION STATEMENT: Converts the board into a string
        * INPUT STATEMENT: None
        * OUTPUT STATEMENT: Return a string representing the board 
        */
        override
        public string ToString()
        {
            string SudBoard = "";
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    SudBoard += (char)(board[i, j]+'0');
                }
            }
            return SudBoard;
        }

       /*
        * FUNCTION STATEMENT: Checks wether one of the existed board is solved
        * INPUT STATEMENT: None
        * OUTPUT STATEMENT: Returns true/false for solved/unsolved board
       */
        public static bool IsSolved()
        {
            return isSolved;
        }


        //Gets and Sets methods
        public int[,] GetBoard()
        {
            return this.board;
        }
        public int Getlength()
        {
            return this.length;
        }
        public char[,] GetRows()
        {
            return this.rows;
        }
        public char[,] GetColumns()
        {
            return this.columns;
        }
        public char[,] GetCubes()
        {
            return this.cubes;
        }
        public void SetAsSolved()
        {
            isSolved = true;
        }

        
        
    }
}
