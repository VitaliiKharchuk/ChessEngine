using System;
using ChessRules;
using SolverApi;

namespace ChessPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            //"r2qkb1r/pb1nppp1/2p4p/1p1nP1B1/2pPN3/5N2/PP2BPPP/R2QK2R w - - 0 1"
            var chess = new Chess("rnb1kbnr/ppppqppp/8/8/8/8/PPPP1PPP/RNBQKBNR b - - 0 1");
            var solver = new ChessSolver(2);

            while (true)
            {
                Extentions.Print(Extentions.ChessToAscii(chess));
                Console.WriteLine(solver.EvaluatePosition(chess));

                if (HasMateOrStaleMate(chess))
                {
                    break;
                }

                string bestMove;
                if (chess.MoveColor == Color.white)
                {
                    do
                    {
                        bestMove = Console.ReadLine();

                    } while (bestMove.Trim() == string.Empty);

                }
                else
                {
                    bestMove = solver.FindBestMove(chess, 3);
                }

                Console.WriteLine("BestMove = " + bestMove);

                chess = chess.Move(bestMove);
                Console.WriteLine(chess.fen);
            }

            Console.ReadKey();
        }

        static bool HasMateOrStaleMate(Chess chess)
        {
            if (chess.IsStaleMate)
            {
                Console.WriteLine("StaleMate");
                return true;
            }
            else if (chess.IsCheckMate)
            {
                Console.WriteLine("CheckMate");
                return true;
            }

            return false;
        }
    }
}
