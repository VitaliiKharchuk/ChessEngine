using UnityEngine;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using ChessRules;
using System;
using Assets.Scripts.Common;

public class Board : MonoBehaviour
{
    static Dictionary<string, GameObject> Squares = new Dictionary<string, GameObject>();
    static Dictionary<string, GameObject> Figures = new Dictionary<string, GameObject>();
    static Dictionary<string, GameObject> Promotions = new Dictionary<string, GameObject>();

    DragAndDrop dad = new DragAndDrop(PickObject, RenderMove);
    static Chess chess = new Chess();
    static string onPromotionMove = string.Empty;

    void Start()
    {
        //"rnbqkbnr/ppPppp1p/8/8/8/8/PP1PPPpP/RNBQKBNR w KQkq - 0 1"
        InitGameObjects(Squares, Figures, Promotions);
        BoardRenderer.ShowFigures(chess, Squares, Figures);
        BoardRenderer.MarkSquaresFrom(chess, Squares);
        BoardRenderer.ShowPromotionsFigures(Promotions);
    }

    // Update is called once per frame
    void Update()
    {
        dad.Action();
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
        if (onPromotionMove != string.Empty)
        {
            int x = Convert.ToInt32(from.x);
            if (onPromotionMove[0] == 'P')
            {
                if (x == 2)
                {
                    onPromotionMove += 'Q';
                }
                if (x == 3)
                {
                    onPromotionMove += 'R';
                }
                if (x == 4)
                {
                    onPromotionMove += 'B';
                }
                if (x == 5)
                {
                    onPromotionMove += 'N';
                }
            }
            else if (onPromotionMove[0] == 'p')
            {
                if (x == 2)
                {
                    onPromotionMove += 'q';
                }
                if (x == 3)
                {
                    onPromotionMove += 'r';
                }
                if (x == 4)
                {
                    onPromotionMove += 'b';
                }
                if (x == 5)
                {
                    onPromotionMove += 'n';
                }
            }

            chess = chess.Move(onPromotionMove);
            onPromotionMove = string.Empty;
            BoardRenderer.ShowPromotionsFigures(Promotions);
            BoardRenderer.ShowFigures(chess, Squares, Figures);
            BoardRenderer.MarkSquaresFrom(chess, Squares);
            return;
        }

        BoardRenderer.MarkSquaresTo(chess, Squares, from);
    }

    public static void RenderMove(Vector2 from, Vector2 to)
    {
        var f = BoardRenderer.VectorToSquare(from);
        var t = BoardRenderer.VectorToSquare(to);
        var figure = chess.GetFigureAt(f);

        string move = $"{figure}{f}{t}";

      
        if (t == string.Empty || f == string.Empty || move.Length != 5 || chess == chess.Move(move))
        {
            //not valid move
            BoardRenderer.ShowFigures(chess, Squares, Figures);
            BoardRenderer.MarkSquaresFrom(chess, Squares);
            return;
        }
        Debug.Log(move);

        if (figure == 'P' && t[1] == '8' || figure == 'p' && t[1] == '1')
        {
            if (chess != chess.Move(move))
            {
                onPromotionMove = move;
                BoardRenderer.ShowPromotionsFigures(Promotions, figure);
                return;
            }
        }
        chess = chess.Move(move);
        BoardRenderer.ShowFigures(chess, Squares, Figures);
        BoardRenderer.MarkSquaresFrom(chess, Squares);
    }

    static GameObject CreateGameObject(string pattern, int x, int y)
    {
        var go = Instantiate(GameObject.Find(pattern));
        go.transform.position = new Vector2(x, y);
        go.name = pattern;

        return go;
    }
}


