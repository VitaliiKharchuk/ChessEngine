namespace ChessRules
{
    public class FigureOnSquare
    {
        public FigureOnSquare(Figure figure, Square square)
        {
            Figure = figure;
            Square = square;
        }

        public Figure Figure { get; private set; }
        public Square Square { get; private set; }

    }
}
