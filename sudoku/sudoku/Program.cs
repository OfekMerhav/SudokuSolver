using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sudoku
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Game.StartGame();          
        }
    }
}
