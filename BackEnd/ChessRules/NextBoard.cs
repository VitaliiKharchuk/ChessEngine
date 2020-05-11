using System.Text;

namespace ChessRules
{
    public class NextBoard : Board
    {
        FigureMoving fm;
        public NextBoard(string fen, FigureMoving fm) : base(fen)
        {
            this.fm = fm;

            MoveFigures();
            DropEnpassant();
            SetEnpassant();
            MoveCastleRook();
            UpdateCastleFlags();

            UpdateMoveNumber();
            UpdateMoveColor();
            GenerateFen();
        }

        private void MoveCastleRook()
        {
            if (CanCastleA1 && fm.Figure == Figure.whiteKing && fm.From == new Square("e1") && fm.To == new Square("c1"))
            {
                SetFigureAt(new Square("a1"), Figure.none);
                SetFigureAt(new Square("d1"), Figure.whiteRook);
                return;
            }

            if (CanCastleH1 && fm.Figure == Figure.whiteKing && fm.From == new Square("e1") && fm.To == new Square("g1"))
            {
                SetFigureAt(new Square("h1"), Figure.none);
                SetFigureAt(new Square("f1"), Figure.whiteRook);
                return;
            }

            if (CanCastleA8 && fm.Figure == Figure.blackKing && fm.From == new Square("e8") && fm.To == new Square("c8"))
            {
                SetFigureAt(new Square("a8"), Figure.none);
                SetFigureAt(new Square("d8"), Figure.blackRook);
                return;
            }

            if (CanCastleH8 && fm.Figure == Figure.blackKing && fm.From == new Square("e8") && fm.To == new Square("g8"))
            {
                SetFigureAt(new Square("h8"), Figure.none);
                SetFigureAt(new Square("f8"), Figure.blackRook);
                return;
            }
        }

        private void UpdateCastleFlags()
        {
            switch (fm.Figure)
            {
                case Figure.whiteKing:
                    {
                        CanCastleA1 = CanCastleH1 = false;
                        return;
                    }

                case Figure.blackKing:
                    {
                        CanCastleA8 = CanCastleH8 = false;
                        return;
                    }
                case Figure.whiteRook:
                    {
                        if (fm.From == new Square("a1"))
                        {
                            CanCastleA1 = false;
                        }
                        else if (fm.From == new Square("h1"))
                        {
                            CanCastleH1 = false;
                        }
                        return;
                    }

                case Figure.blackRook:
                    {
                        if (fm.From == new Square("a8"))
                        {
                            CanCastleA8 = false;
                        }
                        else if (fm.From == new Square("h8"))
                        {
                            CanCastleH8 = false;
                        }
                        return;
                    }
                default:
                    return;
            }
        }

        private void DropEnpassant()
        {
            if (fm.To == Enpassant && (fm.Figure == Figure.whitePawn || fm.Figure == Figure.blackPawn))
            {
                SetFigureAt(new Square(fm.To.x, fm.From.y), Figure.none);
            }
        }

        private void SetEnpassant()
        {
            Enpassant = Square.none;

            if (fm.Figure == Figure.whitePawn && fm.DeltaY == 2)
            {
                Enpassant = new Square(fm.From.x, 2);
            }

            if (fm.Figure == Figure.blackPawn && fm.DeltaY == -2)
            {
                Enpassant = new Square(fm.From.x, 5);
            }
        }

        private void UpdateMoveColor()
        {
            MoveColor = MoveColor.FlipColor();
        }

        private void UpdateMoveNumber()
        {
            if (MoveColor == Color.black)
            {
                MoveNumber++;
            }
        }

        private void MoveFigures()
        {
            SetFigureAt(fm.From, Figure.none);
            SetFigureAt(fm.To,
                fm.Promotion == Figure.none ? fm.Figure : fm.Promotion);
        }

        private void SetFigureAt(Square square, Figure figure)
        {
            if (square.OnBoard())
            {
                Figures[square.x, square.y] = figure;
            }
        }

        public void GenerateFen()
        {
            fen = FenFigures() + " " +
                  FenMoveColor() + " " +
                  FenCastleFlags() + " " +
                  FenEmpassant() + " " +
                  FenDrawNumber() + " " +
                  FenMoveNumber();
        }

        private string FenFigures()
        {
            var sb = new StringBuilder();

            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    sb.Append(Figures[x, y] == Figure.none ?
                        '1' : (char)Figures[x, y]);
                }

                if (y > 0)
                {
                    sb.Append('/');
                }
            }

            string eqight = "11111111";
            for (var j = 8; j >= 2; j--)
            {
                sb = sb.Replace(eqight.Substring(0, j), j.ToString());
            }

            return sb.ToString();
        }

        private string FenMoveColor()
        {
            return MoveColor == Color.white ? "w" : "b";
        }

        private string FenCastleFlags()
        {
            var flags = (CanCastleA1 ? "Q" : "") +
                    (CanCastleH1 ? "K" : "") +
                    (CanCastleA8 ? "q" : "") +
                    (CanCastleH8 ? "k" : "");

            return flags == "" ? "-" : flags;
        }

        private string FenEmpassant()
        {
            return Enpassant.Name;
        }

        private string FenDrawNumber()
        {
            return DrawNumber.ToString();
        }

        private string FenMoveNumber()
        {
            return MoveNumber.ToString();
        }
    }
}
