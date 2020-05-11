using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using ChessRules;
using System;

namespace Assets.Scripts.Common
{
    class BoardRenderer
    {
        public static void ShowFigures(Chess chess, Dictionary<string, GameObject> Squares, Dictionary<string, GameObject> Figures)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    string key = "" + x + y;

                    string figure = chess.GetFigureAt(x, y).ToString();
                    Figures[key].transform.position = Squares[key].transform.position;

                    if (Figures[key].name == figure)
                    {
                        continue;
                    }

                    SetSprite(Figures[key], figure);
                    Figures[key].name = figure;
                }
            }
        }

        static void ShowSquare(Dictionary<string, GameObject> Squares, int x, int y, bool marked = false)
        {
            string square = ((x + y) % 2 == 0) ? "blackSquare" : "whiteSquare";

            if (marked)
            {
                square += "Marked";
            }

            SetSprite(Squares[$"{x}{y}"], square);
        }

        public static void MarkSquaresFrom(Chess chess, Dictionary<string, GameObject> Squares)
        {
            UnmarkSquares(Squares);
            foreach (var move in chess.YieldValidMoves())
            {
                ShowSquare(Squares, move[1] - 'a', move[2] - '1', true);
            }
        }

        public static void MarkSquaresTo(Chess chess, Dictionary<string, GameObject> Squares, Vector2 from)
        {
            UnmarkSquares(Squares);
            var f = VectorToSquare(from);

            foreach (var move in chess.YieldValidMoves())
            {
                if (move.Substring(1, 2) == f)
                {
                    ShowSquare(Squares, move[3] - 'a', move[4] - '1', true);
                }
            }
        }

        public static void UnmarkSquares(Dictionary<string, GameObject> Squares)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    ShowSquare(Squares, x, y);
                }
            }
        }

        public static void ShowPromotionsFigures(Dictionary<string, GameObject> Promotions, char pawn = default)
        {
            switch (pawn)
            {
                case 'P':
                    {
                        SetSprite(Promotions["Q"], "Q");
                        SetSprite(Promotions["R"], "R");
                        SetSprite(Promotions["B"], "B");
                        SetSprite(Promotions["N"], "N");

                        return;
                    }
                case 'p':
                    {
                        SetSprite(Promotions["q"], "q");
                        SetSprite(Promotions["r"], "r");
                        SetSprite(Promotions["b"], "b");
                        SetSprite(Promotions["n"], "n");
                        return;
                    }
                default:
                    {
                        foreach (var promotion in Promotions.Values)
                        {
                            SetSprite(promotion, ".");
                        }
                        return;
                    }
            }
        }

        public static string VectorToSquare(Vector2 vector)
        {
            int x = Convert.ToInt32(vector.x);
            int y = Convert.ToInt32(vector.y);

            if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
            {
                return $"{(char)('a' + x)}{y + 1}";
            }

            return string.Empty;
        }

        public static void SetSprite(GameObject gameObject, string pattern)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite =
                GameObject.Find(pattern).GetComponent<SpriteRenderer>().sprite;
        }
    }
}
