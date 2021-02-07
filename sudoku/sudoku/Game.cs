using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace sudoku
{
    public static class Game
    {
        /*
        * FUNCTION STATEMENT: The main function that manage the game
        * INPUT STATEMENT: None
        * OUTPUT STATEMENT: None
        */
        public static void StartGame()
        {
            Console.WriteLine("Hello and Welcome to the world of sudoku!");
            string chosenMethod = " ", BoardString = " ";
            do
            {
                do
                {
                    //Gets the method type the user wants to insert his board 
                    do
                    {
                        Console.WriteLine("\n*For importing Sudoku board from a file type F \n*For a string type S \n*For finishing type FINISH:");
                        Console.WriteLine("Please Enter valid input: ");
                        chosenMethod = Console.ReadLine();

                     //while the input is incorrect, ask again
                    } while (chosenMethod != "F" && chosenMethod != "S" && chosenMethod != "FINISH");
                    if (chosenMethod == "FINISH") break;
                    //Read according to the chosen method
                    switch (chosenMethod)
                    {
                        //In case of F -> read from file
                        case ("F"):
                            BoardString = IO.ReadFromFile();
                            break;
                        //In case of S -> read from console
                        case ("S"):
                            BoardString = IO.ReadFromConsole();
                            break;
                    }
                    //If the board is invalid,Reloop
                    if (!Board.IsBoardValid(BoardString))
                    {
                        IO.ShowMessage("The board is Invalid");
                    }
                    else
                        break;
                }
                while (true);
                if (chosenMethod == "FINISH")
                {
                    IO.ShowMessage("Thanks for using my sudoku solver :)");
                    break;
                }
                //Build the board with a given string represents the board
                Board board;
                try
                {
                    board = new Board(BoardString);
                }
                catch (ArgumentException e)
                {
                    //If the board was incorrect,show the error message
                    IO.ShowMessage(e.Message);
                    continue;
                }
                //Print the initial board
                board.PrintBoard();
                Console.WriteLine("----------------------------------------------------------------------------");
                //Execute the solving function
                if(SudSolver.SolveSud(board))
                {
                    string answer;
                    do
                    {
                        //Save Solution option
                        Console.WriteLine("Would you like to save the solution?");
                        Console.WriteLine("Type F For saving in file:");
                        Console.WriteLine("Type S For printing as string");
                        Console.WriteLine("Else type N");
                        answer = Console.ReadLine();
                        switch (answer)
                        {
                            case ("F"):
                                IO.WriteToFile(SudSolver.SolvedBoard);
                                break;
                            case ("S"):
                                Console.WriteLine("\nSudoku board string:");
                                Console.WriteLine(SudSolver.SolvedBoard.ToString());
                                break;
                            case ("N"):
                                break;
                        }
                    } while (answer != "F" && answer != "S" && answer != "N");
                    //Sets the SolvedBoard to null again,for the next solving 
                    SudSolver.SolvedBoard = null;
                }

            } while (true);

        }


    }
}
