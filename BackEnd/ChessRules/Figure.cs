using System.Collections.Generic;

namespace ChessRules
{
    public enum Figure
    {
        none,

        whiteKing = 'K',
        whiteQueen = 'Q',
        whiteBishop = 'B',
        whiteKnight = 'N',
        whiteRook = 'R',
        whitePawn = 'P',

        blackKing = 'k',
        blackQueen = 'q',
        blackBishop = 'b',
        blackKnight = 'n',
        blackRook = 'r',
        blackPawn = 'p',
    }

    public static class FigureMethods
    {
        public static Color GetFigureColor(this Figure figure)
        {
            return figure == Figure.none ? Color.none :
                'a' <= (char)figure && (char)figure <= 'z'
                ? Color.black : Color.white;
        }

        public static IEnumerable<Figure> YieldPromotions(this Figure figure, Square to)
        {
            if (figure == Figure.whitePawn && to.y == 7)
            {
                yield return Figure.whiteQueen;
                yield return Figure.whiteBishop;
                yield return Figure.whiteRook;
                yield return Figure.whiteKnight;
            }
            else if (figure == Figure.blackPawn && to.y == 0)
            {
                yield return Figure.blackQueen;
                yield return Figure.blackBishop;
                yield return Figure.blackRook;
                yield return Figure.blackKnight;
            }
            else
            {
                yield return Figure.none;
            }
        }
    }
}
