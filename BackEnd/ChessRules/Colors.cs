namespace ChessRules
{
    public enum Color
    {
        none,
        white,
        black
    }

    public static class ColorMethods
    {
        public static Color FlipColor(this Color color)
        {
            switch (color)
            {
                case Color.white:
                    {
                        return Color.black;
                    }
                case Color.black:
                    {
                        return Color.white;
                    }

                case Color.none:
                default:
                    return Color.none;
            }
        }
    }
}
