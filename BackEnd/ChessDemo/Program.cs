using System;
using ChessRules;
using System.Text;

namespace ChessDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var chess = new Chess("rnbqkbnr/p2pp1pp/8/1pp2p2/3P1P2/3B4/PP1P1PPP/RNBQK1NR w KQkq - 0 1");
            var nums = Extentions.ValidMoves(3, chess);
            Console.WriteLine(nums);
            // return;

            while (true)
            {
                //Console.WriteLine(chess.fen);
                Extentions.Print(Extentions.ChessToAscii(chess));
                var move = Console.ReadLine();
                if (move == string.Empty)
                {
                    break;
                }

                chess = chess.Move(move);
            }
        }
    }
}
