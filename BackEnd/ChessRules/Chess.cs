using System.Collections.Generic;

namespace ChessRules
{
    public class Chess
    {
        public string fen
        {
            get
            {
                return Board.fen;
            }
        }

        public Color MoveColor
        {
            get
            {
                return Board.MoveColor;
            }
        }

        public bool IsCheck { get; private set; }
        public bool IsCheckMate { get; private set; }
        public bool IsStaleMate { get; private set; }


        Board Board;
        Moves Moves;

        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq — 0 1")
        {
            Board = new Board(fen);
            Moves = new Moves(Board);
        }

        private Chess(Board board)
        {
            Board = board;
            Moves = new Moves(board);
            SetChessFlags();
        }

        private void SetChessFlags()
        {
            IsCheck = Board.IsCheck();
            IsCheckMate = false;
            IsStaleMate = false;

            foreach (var _ in YieldValidMoves())
            {
                return;
            }

            if (IsCheck)
            {
                IsCheckMate = true;
            }
            else
            {
                IsStaleMate = true;
            }
        }

        public bool IsValidMove(FigureMoving fm)
        {
            return Moves.CanMove(fm) && !Board.IsCheckAfter(fm);
        }

        public Chess Move(string move)
        {
            var fm = new FigureMoving(move);

            if (!IsValidMove(fm))
            {
                return this;
            }

            var nextBoard = Board.Move(fm);

            return new Chess(nextBoard);
        }

        public char GetFigureAt(int x, int y)
        {
            var square = new Square(x, y);
            var figure = Board.GetFigureAt(square);

            return figure == Figure.none ? '.' : (char)figure;
        }

        public char GetFigureAt(string xy)
        {
            var square = new Square(xy);

            var figure = Board.GetFigureAt(square);

            return figure == Figure.none ? '.' : (char)figure;
        }

        public List<string> GetValidMoves()
        {
            var figuresOnSquares = Board.GetFigureOnSquares();
            var squares = Square.GetBoardSquares();
            var validMoves = new List<string>();

            foreach (var figuresOnSquare in figuresOnSquares)
            {
                foreach (var to in squares)
                {
                    foreach (Figure promotion in figuresOnSquare.Figure.YieldPromotions(to))
                    {
                        FigureMoving fm = new FigureMoving(figuresOnSquare, to, promotion);

                        if (IsValidMove(fm))
                        {
                            validMoves.Add(fm.ToString());
                        }
                    }
                }
            }

            return validMoves;
        }


        public List<string> GetValidCaptures()
        {
            var figuresOnSquares = Board.GetFigureOnSquares();
            var squares = Square.GetBoardSquares();
            var validMoves = new List<string>();

            var opponentCollor = MoveColor == Color.black ? Color.white : Color.black;
            foreach (var figuresOnSquare in figuresOnSquares)
            {
                foreach (var to in squares)
                {
                    foreach (Figure promotion in figuresOnSquare.Figure.YieldPromotions(to))
                    {
                        FigureMoving fm = new FigureMoving(figuresOnSquare, to, promotion);

                        if (IsValidMove(fm) 
                            && Board.GetFigureAt(to).GetFigureColor() == opponentCollor)
                        {
                            validMoves.Add(fm.ToString());
                        }
                    }
                }
            }

            return validMoves;
        }


        public IEnumerable<string> YieldValidMoves()
        {
            var figuresOnSquares = Board.GetFigureOnSquares();
            var squares = Square.GetBoardSquares();

            foreach (var figuresOnSquare in figuresOnSquares)
            {
                foreach (var to in squares)
                {
                    foreach (Figure promotion in figuresOnSquare.Figure.YieldPromotions(to))
                    {
                        FigureMoving fm = new FigureMoving(figuresOnSquare, to, promotion);

                        if (IsValidMove(fm))
                        {
                            yield return fm.ToString();
                        }
                    }
                }
            }
        }
    }
}
