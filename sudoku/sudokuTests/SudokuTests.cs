using Microsoft.VisualStudio.TestTools.UnitTesting;
using sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        /*
        * FUNCTION STATEMENT: Main function alike for the test functions
        * INPUT STATEMENT: string BoardString representing Sudoku board
        * OUTPUT STATEMENT: bool , true/false for solved/unsolved board
        */
        public bool SolveBoard(string BoardString)
        {
            //Checks if the board is invalid
            if (!Board.IsBoardValid(BoardString))
            {
                return false;
            }
            Board board;
            try
            {
                board = new Board(BoardString);
            }
            catch (ArgumentException e)
            {
                return false;
            }
            //Solve
            if (SudSolver.SolveSud(board))
            {
                //If the board was solved
                SudSolver.SolvedBoard = null;
                return true;
            }
            return false;
        }


        [TestMethod()]
        public void TestEmptyBoard_4x4()
        {
            //Empty board
            string board = "0000000000000000";

            bool test = SolveBoard(board);

            Assert.AreEqual(true, test);

        }
        [TestMethod()]
        public void TestUnsolvableBoard_4x4()
        {
            //Unsolvable board
            string board = "1200002000010002";

            bool test = SolveBoard(board);

            Assert.AreEqual(false, test);

        }
        [TestMethod()]
        public void TestInvalidFullBoard_4x4()
        {
            //Invalid Full board
            string board = "1111222233334444";

            bool test = SolveBoard(board);

            Assert.AreEqual(false, test);

        }
        [TestMethod()]
        public void TestInvalidBoard_4x4()
        {
            //Invalid board
            string board = "000000000 000000";

            bool test = SolveBoard(board);

            Assert.AreEqual(false, test);

        }
        [TestMethod()]
        public void TestInvalidBoard2_4x4()
        {
            //Invalid board -> S in middle
            string board = "0000000000S00000";

            bool test = SolveBoard(board);

            Assert.AreEqual(false, test);

        }
        [TestMethod()]
        public void TestSolvavleBoard_4x4()
        {
            //Solvable board
            string board = "0000100000000000";

            bool test = SolveBoard(board);

            Assert.AreEqual(true, test);

        }
        [TestMethod()]
        public void TestEmptyBoard_9x9()
        {
            //EmptyBoard
            string board = "00000000000000000000000000000" +
                           "00000000000000000000000000000" +
                           "00000000000000000000000";
 
            bool test = SolveBoard(board);

            Assert.AreEqual(true, test);

        }
        [TestMethod()]
        public void TestSolvableBoard_9x9()
        {
            //Solvable board
            string board = "7000004004090000003150080000000" +
                           "0601010207304000004070005000000" +
                           "0000201005030000681";

            bool test = SolveBoard(board);

            Assert.AreEqual(true, test);

        }
        [TestMethod()]
        public void TestInvalidBoard_9x9()
        {
            //Invalid board
            string board = "933018000000000020000030000000000" +
                           "108200500000600000000000205040010" +
                           "000900000600000";

            bool test = SolveBoard(board);

            Assert.AreEqual(false, test);

        }
        [TestMethod()]
        public void TestUnsolvableBoard_9x9()
        {
            //Unsolvable board
            string board = "93705000024617398585102000016" +
                           "85374923248901007950600305190" +
                           "80073002010000003040009";

            bool test = SolveBoard(board);

            Assert.AreEqual(false, test);

        }
        [TestMethod()]
        public void TestValidFullboard_9x9()
        {
            //Valid Full board
            string board = "726319458489752136315468297" +
                           "574926813162873549893145762" +
                           "951687324648231975237594681";

            bool test = SolveBoard(board);

            Assert.AreEqual(true, test);

        }
        [TestMethod()]
        public void TestEmptyBoard_16x16()
        {
            //Empty board
            string board = "00000000000000000000000000000000000000" +
                           "00000000000000000000000000000000000000" +
                           "00000000000000000000000000000000000000" +
                           "00000000000000000000000000000000000000" +
                           "00000000000000000000000000000000000000" +
                           "00000000000000000000000000000000000000" +
                           "0000000000000000000000000000";
            

            bool test = SolveBoard(board);

            Assert.AreEqual(true, test);

        }
        [TestMethod()]
        public void TestSolvableBoard_16x16()
        {
            //Solvable board
            string board = "6007:1004>2?800=00@>0760<900004?0;=0008<07000069<00400>0036" +
                           ":072096001000>:0503008<0;9002?@00010004:0>005201;000<10000:" +
                           "00009<?2;0?020007=0109;008300008007=4000000071@<000;0050020" +
                           "@083510000064<00?05000000020=002180;0=70?0000>00060<000900=" +
                           "0000@04:?290;5300080";

            bool test = SolveBoard(board);

            Assert.AreEqual(true, test);

        }
        [TestMethod()]
        public void TestInvalidBoard_16x16()
        {
            //Invalid board
            string board = "60]7:1004>2?800=00@>0760<900004?0;=0008<0pp000069<00400>0036" +
                           ":072096001000>:0503008<0;9002?@00010004:0>005201;000<10000:0" +
                           "0009<?2;0?020007=0109;008300008007=4000000071@<000;0050020@0" +
                           "83510000064<00?05000000020=002180;0=70?0000>00060<000900=000" +
                           "0@04:?290;5300080";

            bool test = SolveBoard(board);
            
            Assert.AreEqual(false, test);

        }
        [TestMethod()]
        public void TestUnSolvableBoard_16x16()
        {
            //UnSolvable board
            string board = "6397:1@;4>2?8<5=52@>=763<9;81:4?:;=?248<07003>69<81459>?=36" +
                           ":@72;96?01000>:0503008<0;9002?@00010074:0>005201;000<10000:" +
                           "00009<?2;0?0204>7=0109;@083000680:7=4000000071@<000;805002=" +
                           "@;83519:2?>64<70?05000000020=002180;0=70?0000>00063<000900=" +
                           "0000@=4:?29>;537<681";

            bool test = SolveBoard(board);

            Assert.AreEqual(false, test);

        }
        [TestMethod()]
        public void TestInvalidBoard2_16x16()
        {
            //Invalid board -> too long
            string board = "6397:1@;4>2?8<5=52@>=763<9;81:4?:;=?248<07003>69<81459>?=36" +
                           ":@72;96?01000>:0503008<0;9002?@00010074:0>005201;000<10000:" +
                           "00009<?2;0?0204>7=0109;@083000680:7=4000000071@<000;805002=" +
                           "@;83519:2?>64<70?05000000020=002180;0=70?0000>00063<000900=" +
                           "0000@=4:?29>;537<68100";

            bool test = SolveBoard(board);

            Assert.AreEqual(false, test);

        }
    }
}