using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sudoku
{
    public static class IO
    {
        //Building a File dialog box 
        private static OpenFileDialog dialog = new OpenFileDialog
        {
            Multiselect = false,
            Title = "Please Choose A File",
            //For only text files
            Filter = "Text|*.txt|All|*.*"

        };
        /*
        * FUNCTION STATEMENT:Reads Sudoku board from a selected file
        * INPUT STATEMENT: None
        * OUTPUT STATEMENT: String containing the board
        */
        public static string ReadFromFile()
        {
            string logfile = "Something went wrong reading the file.. Please try again:";
            
            using (dialog)
            {
                //Opening the file selection dialog box
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //try to read the selected file text into a string
                    try{
                        logfile = File.ReadAllText(dialog.FileName);
                    }
                    catch(Exception e)
                    {
                        ShowMessage(logfile);
                    }
                }
               
            }
            //return the readed text
            return logfile;
        }
        
        /*
        * FUNCTION STATEMENT:Reads Sudoku board from the Console
        * INPUT STATEMENT: None
        * OUTPUT STATEMENT: String containing the board 
        */
        public static string ReadFromConsole()
        {
            Console.WriteLine("Enter your board:");
            //Extends the chars limit in the console
            int bufSize = 4096;
            Stream inStream = Console.OpenStandardInput(bufSize);
            Console.SetIn(new StreamReader(inStream, Console.InputEncoding, false, bufSize));
            return Console.ReadLine();
        }

        /*
        * FUNCTION STATEMENT: Write a sudoku board string formated into the chosen file  
        * INPUT STATEMENT: Sudoku Board 
        * OUTPUT STATEMENT: None
        */
        public static void WriteToFile(Board board)
        {
            using (dialog)
            {
                //Opening the file selection dialog box
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //Write to the chosen file
                        using (var wr = new StreamWriter(dialog.FileName))
                        {
                            wr.Write("\r\nLog Entry : ");
                            wr.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                            wr.WriteLine("Board solution:");
                            //writing a string format of the board
                            wr.WriteLine(board.ToString());
                            wr.WriteLine("-------------------------------");
                            
                            wr.Flush();
                            wr.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        ShowMessage("Failed Writing to file");
                    }
                }

            }
            
        }

        /*
        * FUNCTION STATEMENT: Display a message with the message box
        * INPUT STATEMENT: string message
        * OUTPUT STATEMENT: None
        */
        public static void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }


    }
}
