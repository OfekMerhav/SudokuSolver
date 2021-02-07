using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    //The cell class represent a single cell in the Sudoku board
    public class Cell
    {
        //properties 
        public int Row { get; set; }
        public int Col { get; set; }
        //List of the possible filling numbers in the cell
        public List<int> Possibilities { get; set; }
        //Constructors
        public Cell()
        {
            Possibilities = new List<int>();
        }
        public Cell(int row, int col, List<int> possibilities)
        {
            Row = row;
            Col = col;
            Possibilities = possibilities;
        }
    }
}
