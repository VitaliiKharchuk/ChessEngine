using System.Collections.Generic;

namespace ChessRules
{
    public class Board
    {
        public string fen { get; protected set; }
        protected Figure[,] Figures { get; private set; } = new Figure[8, 8];

        public Color MoveColor { get; protected set; }

        public int DrawNumber { get; protected set; }

        public int MoveNumber { get; protected set; }

        public bool CanCastleA1 { get; set; }

        public bool CanCastleH1 { get; set; }

        public bool CanCastleA8 { get; set; }

        public bool CanCastleH8 { get; set; }


        public Square Enpassant { get; set; }

        public Board(string fen)
        {
            this.fen = fen;
            Figures = new Figure[8, 8];
            Init();
        }

        public Board Move(FigureMoving figureMoving)
        {
            return new NextBoard(fen, figureMoving);
        }

        public Figure GetFigureAt(Square square)
        {
            if (!square.OnBoard())
            {
                return Figure.none;
            }

            return Figures[square.x, square.y];
        }

        private void Init()
        {
            string[] parts = fen.Split();
            InitFigures(parts[0]);
            InitMoveColor(parts[1]);
            InitCastleFlags(parts[2]);
            InitEnpassant(parts[3]);
            InitDrawNumber(parts[4]);
            InitMoveNumber(parts[5]);
        }

        public bool IsCheckAfter(FigureMoving fm)
        {
            Board after = Move(fm);
            return after.CanEatKing();
        }

        private bool CanEatKing()
        {
            var oponentKing = FindOponentKing();
            if (oponentKing == Square.none)
            {
                return false;
            }
            
            var moves = new Moves(this);
            
            foreach (var fs in YieldFiguresOnSquares())
            {
                if (moves.CanMove(new FigureMoving(fs, oponentKing)))
                {
                    return true;
                }
            }

            return false;
        }

        private Square FindOponentKing()
        {
            var king = MoveColor == Color.white ?
                Figure.blackKing : Figure.whiteKing;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var square = new Square(x, y);
                    if (GetFigureAt(square) == king)
                    {
                        return square;
                    }
                }
            }

            return Square.none;
        }

        private void InitMoveNumber(string v)
        {
            MoveNumber = int.Parse(v);
        }

        private void InitDrawNumber(string v)
        {
            DrawNumber = int.Parse(v);
        }

        private void InitEnpassant(string v)
        {
            Enpassant = new Square(v);
        }

        private void InitCastleFlags(string v)
        {
            CanCastleA1 = v.Contains("Q");

            CanCastleH1 = v.Contains("K");

            CanCastleA8 = v.Contains("q");

            CanCastleH8 = v.Contains("k");
        }

        private void InitMoveColor(string v)
        {
            MoveColor = v == "b" ? Color.black : Color.white;
        }

        private void InitFigures(string v)
        {
            for (int j = 8; j >= 2; j--)
            {
                v = v.Replace(j.ToString(), (j - 1).ToString() + "1");
            }

            v = v.Replace('1', (char)Figure.none);

            var lines = v.Split('/');

            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    Figures[x, y] = (Figure)lines[7 - y][x];
                }
            }
        }

        public List<FigureOnSquare> GetFigureOnSquares()
        {
            var figuresOnSquares = new List<FigureOnSquare>();
           
            foreach (Square square in Square.YeildBoardSquares())
            {
                if (GetFigureAt(square).GetFigureColor() == MoveColor)
                {
                    figuresOnSquares.Add(new FigureOnSquare(GetFigureAt(square), square));
                }
            }

            return figuresOnSquares;
        }

        public IEnumerable<FigureOnSquare> YieldFiguresOnSquares()
        {
            foreach (Square square in Square.YeildBoardSquares())
            {
                if (GetFigureAt(square).GetFigureColor() == MoveColor)
                {
                    yield return new FigureOnSquare(GetFigureAt(square), square);
                }
            }
        }

        internal bool IsCheck()
        {
            return IsCheckAfter(FigureMoving.none);
        }
    }
}
