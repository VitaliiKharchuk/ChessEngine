using System;

namespace ChessRules
{
    public class FigureMoving
    {
        public static FigureMoving none = new FigureMoving();
        public Figure Figure { get; private set; }

        public Square From { get; private set; }

        public Square To { get; private set; }

        public Figure Promotion { get; private set; }

        public FigureMoving(FigureOnSquare fs, Square to, Figure prom = Figure.none)
        {
            Figure = fs.Figure;
            From = fs.Square;
            To = to;
            Promotion = prom;
        }

        private FigureMoving()
        {
            Figure = Figure.none;
            From = Square.none;
            To = Square.none;
            Promotion = Figure.none;
        }

        public override string ToString()
        {
            return (char)Figure + From.Name + To.Name +
                (Promotion == Figure.none ? "" : ((char)Promotion).ToString());
        }


        public FigureMoving(string move)
        {
            Figure = (Figure)move[0];
            From = new Square(move.Substring(1, 2));
            To = new Square(move.Substring(3, 2));

            if (move.Length == 6)
            {
                Promotion = (Figure)move[5];
            }
        }

        public int DeltaX
        {
            get
            {
                return To.x - From.x;
            }
        }

        public int DeltaY
        {
            get
            {
                return To.y - From.y;
            }
        }

        public int AbsDeltaX
        {
            get
            {
                return Math.Abs(DeltaX);
            }
        }

        public int AbsDeltaY
        {
            get
            {
                return Math.Abs(DeltaY);
            }
        }

        public int SignX
        {
            get
            {
                return Math.Sign(DeltaX);
            }
        }

        public int SignY
        {
            get
            {
                return Math.Sign(DeltaY);
            }
        }
    }
}
