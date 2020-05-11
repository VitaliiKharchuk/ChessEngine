namespace ChessRules
{
    public class Moves
    {
        Board Board { get; set; }
        FigureMoving Fm { get; set; }

        public Moves(Board board)
        {
            this.Board = board;
        }

        public bool CanMove(FigureMoving figureMoving)
        {
            this.Fm = figureMoving;

            return CanMoveFrom() &&
                   CanMoveTo() &&
                   CanFigureMove();
        }

        private bool CanFigureMove()
        {
            switch (Fm.Figure)
            {
                case Figure.whiteKing:
                case Figure.blackKing:
                    return CanKingMove() || CanKingCastle();

                case Figure.whiteQueen:
                case Figure.blackQueen:
                    return CanStraightMove();

                case Figure.whiteBishop:
                case Figure.blackBishop:
                    return Fm.DeltaX != 0 && Fm.DeltaY != 0 &&
                        CanStraightMove();

                case Figure.whiteKnight:
                case Figure.blackKnight:
                    return CanKnightMove();

                case Figure.whiteRook:
                case Figure.blackRook:
                    return (Fm.DeltaX == 0 || Fm.DeltaY == 0) &&
                        CanStraightMove();

                case Figure.whitePawn:
                case Figure.blackPawn:
                    return CanPawnMove();

                case Figure.none:
                default:
                    return false;
            }
        }

        private bool CanKingCastle()
        {
            switch (Fm.Figure)
            {
                case Figure.whiteKing:
                    {
                        return (Board.CanCastleH1 &&
                               Fm.From == new Square("e1") &&
                               Fm.To == new Square("g1") &&
                               Board.GetFigureAt(new Square("h1")) == Figure.whiteRook &&
                               Board.GetFigureAt(new Square("f1")) == Figure.none &&
                               Board.GetFigureAt(new Square("g1")) == Figure.none &&
                               //Check ckeck Kde1f1
                               // Check check ke1g1
                               !Board.IsCheckAfter(new FigureMoving("Ke1f1")) &&
                               !Board.IsCheck()
                               ||
                               Board.CanCastleA1 &&
                               Fm.From == new Square("e1") &&
                               Fm.To == new Square("c1") &&
                               Board.GetFigureAt(new Square("a1")) == Figure.whiteRook &&
                               Board.GetFigureAt(new Square("c1")) == Figure.none &&
                               Board.GetFigureAt(new Square("d1")) == Figure.none &&
                               !Board.IsCheckAfter(new FigureMoving("Ke1d1")) &&
                               !Board.IsCheck());
                    }

                case Figure.blackKing:
                    return (Board.CanCastleH8 &&
                              Fm.From == new Square("e8") &&
                              Fm.To == new Square("g8") &&
                              Board.GetFigureAt(new Square("h8")) == Figure.blackRook &&
                              Board.GetFigureAt(new Square("f8")) == Figure.none &&
                              Board.GetFigureAt(new Square("g8")) == Figure.none &&
                              !Board.IsCheckAfter(new FigureMoving("ke8f8")) &&
                              !Board.IsCheck())
                              ||
                              Board.CanCastleA8 &&
                              Fm.From == new Square("e8") &&
                              Fm.To == new Square("c8") &&
                              Board.GetFigureAt(new Square("a8")) == Figure.blackRook &&
                              Board.GetFigureAt(new Square("c8")) == Figure.none &&
                              Board.GetFigureAt(new Square("d8")) == Figure.none &&
                              !Board.IsCheckAfter(new FigureMoving("ke8d8")) &&
                              !Board.IsCheck();
                default:
                    return false;
            }
        }

        private bool CanPawnMove()
        {
            if (Fm.From.y == 0 || Fm.From.y == 7)
            {
                return false;
            }

            int stepY = Fm.Figure.GetFigureColor() == Color.white ? +1 : -1;
            return CanPawnGo(stepY) ||
                   CanPawnJump(stepY) ||
                   CanPawnEat(stepY) ||
                   CanPawnEnpassant(stepY);
        }

        private bool CanPawnEnpassant(int stepY)
        {
            return Fm.To == Board.Enpassant &&
                Board.GetFigureAt(Fm.To) == Figure.none &&
                Fm.DeltaY == stepY &&
                Fm.AbsDeltaX == 1 &&
                (stepY == 1 && Fm.From.y == 4 || stepY == -1 && Fm.From.y == 3);
        }

        private bool CanPawnEat(int stepY)
        {
            return Board.GetFigureAt(Fm.To) != Figure.none && Fm.AbsDeltaX == 1 && Fm.DeltaY == stepY;
        }

        private bool CanPawnJump(int stepY)
        {
            return Board.GetFigureAt(Fm.To) == Figure.none &&
                   (Fm.From.y == 1 && Fm.Figure.GetFigureColor() == Color.white ||
                   Fm.From.y == 6 && Fm.Figure.GetFigureColor() == Color.black) &&
                   Fm.DeltaX == 0 &&
                   Fm.DeltaY == 2 * stepY &&
                   Board.GetFigureAt(
                       new Square(Fm.From.x, Fm.From.y + stepY)) == Figure.none;
        }

        private bool CanPawnGo(int stepY)
        {
            return (Board.GetFigureAt(Fm.To) == Figure.none) &&
                   Fm.DeltaX == 0 &&
                   Fm.DeltaY == stepY;
        }

        private bool CanStraightMove()
        {
            Square at = Fm.From;

            do
            {
                at = new Square(at.x + Fm.SignX, at.y + Fm.SignY);
                if (at == Fm.To)
                {
                    return true;
                }
            } while (at.OnBoard()
            && Board.GetFigureAt(at) == Figure.none);

            return false;
        }

        bool CanKingMove()
        {
            return Fm.AbsDeltaX <= 1 && Fm.AbsDeltaY <= 1;
        }

        bool CanKnightMove()
        {
            return Fm.AbsDeltaX == 1 && Fm.AbsDeltaY == 2 || Fm.AbsDeltaX == 2 && Fm.AbsDeltaY == 1;
        }

        private bool CanMoveFrom()
        {
            return Fm.From.OnBoard() &&
                Fm.Figure.GetFigureColor() == Board.MoveColor &&
                Board.GetFigureAt(Fm.From) != Figure.none;
        }

        private bool CanMoveTo()
        {
            return Fm.From.OnBoard() &&
                Board.GetFigureAt(Fm.To).GetFigureColor() != Board.MoveColor;
        }
    }
}
