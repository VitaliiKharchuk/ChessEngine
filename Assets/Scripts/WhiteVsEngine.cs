using UnityEngine;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using ChessRules;
using System;
using Assets.Scripts.Common;
using SolverApi;

public class WhiteVsEngine : MonoBehaviour
{
    static bool IsWhiteMove = true;
    static string OnPromotionMove = string.Empty;
    //"8/5k2/8/8/8/1R6/1KQ5/8 w - - 0 1"
    static Chess Chess = new Chess();
    static ChessSolver Solver = new ChessSolver(Constants.QuiesceDeath);
    DragAndDrop Dad = new DragAndDrop(PickObject, RenderMove);

    static Dictionary<string, GameObject> Squares = new Dictionary<string, GameObject>();
    static Dictionary<string, GameObject> Figures = new Dictionary<string, GameObject>();
    static Dictionary<string, GameObject> Promotions = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitGameObjects(Squares, Figures, Promotions);
        BoardRenderer.ShowFigures(Chess, Squares, Figures);
        BoardRenderer.MarkSquaresFrom(Chess, Squares);
        BoardRenderer.ShowPromotionsFigures(Promotions);
    }

    // Update is called once per frame
    void Update()
    {
        if (Chess.IsCheckMate || Chess.IsStaleMate)
        {
            return;
        }

        if (!IsWhiteMove)
        {
            var bestMove = Solver.FindBestMove(Chess, Constants.DefaultSeachDeath);
            Debug.Log(bestMove);
            Chess = Chess.Move(bestMove);

            BoardRenderer.ShowFigures(Chess, Squares, Figures);
            BoardRenderer.MarkSquaresFrom(Chess, Squares);

            ChangeMoveColor();
        }
        else
        {
            Dad.Action();
        }
    }

    static void ChangeMoveColor()
    {
        IsWhiteMove = !IsWhiteMove;
    }

    public static void InitGameObjects(Dictionary<string, GameObject> Squares, Dictionary<string, GameObject> Figures, Dictionary<string, GameObject> Promotions)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                string key = $"{x}{y}";
                string name = (x + y) % 2 == 0 ? "blackSquare" : "whiteSquare";

                Squares[key] = CreateGameObject(name, x, y);
                Figures[key] = CreateGameObject("p", x, y);
            }
        }

        Promotions["Q"] = CreateGameObject("Q", 2, 8);
        Promotions["R"] = CreateGameObject("R", 3, 8);
        Promotions["B"] = CreateGameObject("B", 4, 8);
        Promotions["N"] = CreateGameObject("N", 5, 8);


        Promotions["q"] = CreateGameObject("q", 2, -1);
        Promotions["r"] = CreateGameObject("r", 3, -1);
        Promotions["b"] = CreateGameObject("b", 4, -1);
        Promotions["n"] = CreateGameObject("n", 5, -1);
    }

    public static void PickObject(Vector2 from)
    {
        if (!IsWhiteMove)
        {
            return;
        }

        if (OnPromotionMove != string.Empty)
        {
            int x = Convert.ToInt32(from.x);
            if (OnPromotionMove[0] == 'P')
            {
                if (x == 2)
                {
                    OnPromotionMove += 'Q';
                }
                if (x == 3)
                {
                    OnPromotionMove += 'R';
                }
                if (x == 4)
                {
                    OnPromotionMove += 'B';
                }
                if (x == 5)
                {
                    OnPromotionMove += 'N';
                }
            }
            else if (OnPromotionMove[0] == 'p')
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
            BoardRenderer.ShowPromotionsFigures(Promotions);
            BoardRenderer.ShowFigures(Chess, Squares, Figures);
            BoardRenderer.MarkSquaresFrom(Chess, Squares);
            ChangeMoveColor();
            return;
        }

        BoardRenderer.MarkSquaresTo(Chess, Squares, from);
    }

    public static void RenderMove(Vector2 from, Vector2 to)
    {
        var f = BoardRenderer.VectorToSquare(from);
        var t = BoardRenderer.VectorToSquare(to);

        var figure = Chess.GetFigureAt(f);

        string move = $"{figure}{f}{t}";

        if (t == string.Empty || f == string.Empty || move.Length != 5 || Chess == Chess.Move(move))
        {
            BoardRenderer.ShowFigures(Chess, Squares, Figures);
            BoardRenderer.MarkSquaresFrom(Chess, Squares);
            return;
        }

        Debug.Log(move);

        if (figure == 'P' && t[1] == '8' || figure == 'p' && t[1] == '1')
        {
            if (Chess != Chess.Move(move))
            {
                OnPromotionMove = move;
                BoardRenderer.ShowPromotionsFigures(Promotions, figure);
                return;
            }
        }
        Chess = Chess.Move(move);
        BoardRenderer.ShowFigures(Chess, Squares, Figures);
        BoardRenderer.UnmarkSquares(Squares);

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
