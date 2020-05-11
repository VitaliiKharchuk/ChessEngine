using System;
using System.Text;

namespace ChessRules
{
    public static class Extentions
    {
        public static string ChessToAscii(Chess chess)
        {
            var sb = new StringBuilder();
            sb.Append("  +----------------+\n");

            for (int y = 7; y >= 0; y--)
            {
                sb.Append(y + 1);
                sb.Append(" |");
                for (int x = 0; x < 8; x++)
                {
                    sb.Append(chess.GetFigureAt(x, y) + " ");
                }
                sb.Append("|\n");
            }

            sb.AppendLine("  +----------------+");
            sb.AppendLine("   a-b-c-d-e-f-g-h ");

            if (chess.IsCheck)
            {
                sb.AppendLine("Check");
            }

            if (chess.IsCheckMate)
            {
                sb.AppendLine("CheckMate");
            }

            if (chess.IsStaleMate)
            {
                sb.AppendLine("StaleMate");
            }

            return sb.ToString();
        }

        public static void Print(string text)
        {
            var old = Console.ForegroundColor;
            foreach (char s in text)
            {
                if ('a' <= s && s <= 'z')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if ('A' <= s && s <= 'Z')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }

                Console.Write(s);
            }

            Console.ForegroundColor = old;
        }

        public static int ValidMoves(int moveNumber, Chess chess)
        {
            if (moveNumber == 0)
            {
                return 1;
            }

            int count = 0;

            foreach (var move in chess.YieldValidMoves())
            {
                //Console.WriteLine(move);
                count += ValidMoves(moveNumber - 1, chess.Move(move));
            }

            return count;
        }
    }
}
