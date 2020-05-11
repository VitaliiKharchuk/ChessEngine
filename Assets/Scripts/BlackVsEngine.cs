using UnityEngine;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using ChessRules;
using System;
using Assets.Scripts.Common;
using SolverApi;

public class BlackVsEngine : MonoBehaviour
{
    DragAndDrop Dad = new DragAndDrop(PickObject, RenderMove);
    ChessSolver Solver = new ChessSolver(Constants.QuiesceDeath);
    static Chess Chess = new Chess();
    static bool IsBlackMove = false;
    static string OnPromotionMove = string.Empty;

    static Dictionary<string, GameObject> Squares = new Dictionary<string, GameObject>();
    static Dictionary<string, GameObject> Figures = new Dictionary<string, GameObject>();
    static Dictionary<string, GameObject> Promotions = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitGameObjects();
        ShowFigures();
        MarkSquaresFrom();
        ShowPromotionsFigures();
    }

    // Update is called once per frame
    void Update()
    {
        if (Chess.IsCheckMate || Chess.IsStaleMate)
        {
            return;
        }

        if (!IsBlackMove)
        {
            var bestMove = Solver.FindBestMove(Chess, Constants.DefaultSeachDeath);
            Debug.Log(bestMove);
            Chess = Chess.Move(bestMove);

            ShowFigures();
            MarkSquaresFrom();
            ChangeMoveColor();
        }
        else
        {
            Dad.Action();
        }
    }

    static public void ShowPromotionsFigures(char pawn = default)
    {
        switch (pawn)
        {
            case 'p':
                {
                    BoardRenderer.SetSprite(Promotions["q"], "q");
                    BoardRenderer.SetSprite(Promotions["r"], "r");
                    BoardRenderer.SetSprite(Promotions["b"], "b");
                    BoardRenderer.SetSprite(Promotions["n"], "n");

                    return;
                }
            default:
                {
                    foreach (var promotion in Promotions.Values)
                    {
                        BoardRenderer.SetSprite(promotion, ".");
                    }
                    return;
                }
        }
    }

    static void ChangeMoveColor()
    {
        IsBlackMove = !IsBlackMove;
    }

    static public void ShowFigures()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                int transY = 7 - y;
                string key = $"{x}{transY}";

                string figure = Chess.GetFigureAt(x, transY).ToString();
                Figures[key].transform.position = Squares[$"{x}{y}"].transform.position;

                if (Figures[key].name == figure)
                {
                    continue;
                }

                BoardRenderer.SetSprite(Figures[key], figure);
                Figures[key].name = figure;
            }
        }
    }

    public static void InitGameObjects()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                string key = $"{x}{y}";
                string name = (x + y) % 2 == 0 ? "whiteSquare" : "blackSquare";

                Squares[key] = CreateGameObject(name, x, y);
                Figures[key] = CreateGameObject("p", x, y);
            }
        }

        Promotions["q"] = CreateGameObject("q", 2, 8);
        Promotions["r"] = CreateGameObject("r", 3, 8);
        Promotions["b"] = CreateGameObject("b", 4, 8);
        Promotions["n"] = CreateGameObject("n", 5, 8);
    }

    public static void PickObject(Vector2 from)
    {
        if (!IsBlackMove)
        {
            return;
        }

        if (OnPromotionMove == string.Empty)
        {
            MarkSquaresTo(from);
            return;
        }

        int x = Convert.ToInt32(from.x);

        if (OnPromotionMove[0] == 'p')
        {
            if (x == 2)
            {
                OnPromotionMove += 'q';
            }
            if (x == 3)
            {
                OnPromotionMove += 'r';
            }
            if (x == 4)
            {
                OnPromotionMove += 'b';
            }
            if (x == 5)
            {
                OnPromotionMove += 'n';
            }
        }

        Chess = Chess.Move(OnPromotionMove);
        OnPromotionMove = string.Empty;

        ShowPromotionsFigures();
        ShowFigures();
        UnmarkSquares();
        ChangeMoveColor();
    }

    static public void MarkSquaresFrom()
    {
        UnmarkSquares();
        foreach (var move in Chess.YieldValidMoves())
        {
            ShowSquare(move[1] - 'a', (move[2] - '1'), true);
        }
    }

    static public void MarkSquaresTo(Vector2 from)
    {
        UnmarkSquares();
        var position = VectorToSquare(from);

        foreach (var move in Chess.YieldValidMoves())
        {
            if (move.Substring(1, 2) == position)
            {
                ShowSquare(move[3] - 'a', move[4] - '1', true);
            }
        }
    }

    public static string VectorToSquare(Vector2 vector)
    {
        int x = Convert.ToInt32(vector.x);
        int y = 7 - Convert.ToInt32(vector.y);

        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
        {
            return $"{(char)('a' + x)}{y + 1}";
        }

        return string.Empty;
    }

    static void ShowSquare(int x, int y, bool marked = false)
    {
        string square = ((x + y) % 2 == 1) ? "whiteSquare" : "blackSquare"; ;

        if (marked)
        {
            square += "Marked";
        }

        BoardRenderer.SetSprite(Squares[$"{x}{7 - y}"], square);
    }

    static public void UnmarkSquares()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ShowSquare(x, y);
            }
        }
    }

    static public void RenderMove(Vector2 from, Vector2 to)
    {
        var f = VectorToSquare(from);
        var t = VectorToSquare(to);

        var figure = Chess.GetFigureAt(f);

        string move = $"{figure}{f}{t}";

        if (t == string.Empty || f == string.Empty || move.Length != 5 || Chess == Chess.Move(move))
        {
            //not valid move
            ShowFigures();
            MarkSquaresFrom();
            return;
        }

        if (figure == 'P' && t[1] == '8' || figure == 'p' && t[1] == '1')
        {
            //handle promotion
            OnPromotionMove = move;
            ShowPromotionsFigures(figure);
            return;
        }

        Debug.Log(move);

        //make move
        Chess = Chess.Move(move);

        ShowFigures();
        UnmarkSquares();
        ChangeMoveColor();
    }

    static GameObject CreateGameObject(string pattern, int x, int y)
    {
        var go = Instantiate(GameObject.Find(pattern));
        go.transform.position = new Vector2(x, y);
        go.name = pattern;

        return go;
    }
}
