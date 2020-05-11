using System.Collections.Generic;

namespace ChessRules
{
    public struct Square
    {
        public static Square none = new Square(-1, -1);

        public int x { get; private set; }
        public int y { get; private set; }

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Square(string str)
        {
            if (str.Length == 2 &&
               'a' <= str[0] && str[0] <= 'h' &&
               '1' <= str[1] && str[1] <= '8')
            {
                x = str[0] - 'a';
                y = str[1] - '1';
            }
            else
            {
                this = none;
            }
        }

        public static bool operator ==(Square a, Square b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Square a, Square b)
        {
            return !(a == b);
        }

        public string Name
        {
            get
            {
                if (OnBoard())
                {
                    char xName = (char)('a' + x);
                    char yName = (char)('1' + y);
                    return $"{xName}{yName}";
                }

                else
                {
                    return "-";
                }
            }
        }

        public bool OnBoard()
        {
            return 0 <= x && x < 8 && 0 <= y && y < 8;
        }

        public static List<Square> GetBoardSquares()
        {
            var squares = new List<Square>();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    squares.Add(new Square(x, y));
                }
            }

            return squares;
        }
        public static IEnumerable<Square> YeildBoardSquares()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    yield return new Square(x, y);
                }
            }
            /*
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    yield return new Square(x, y);
                }
            }
            */
        }
    }
}
