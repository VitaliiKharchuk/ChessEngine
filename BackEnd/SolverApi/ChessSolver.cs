using ChessRules;

namespace SolverApi
{
    public class ChessSolver
    {
        int QuiesceDeath;
        public ChessSolver(int quiesceDeath)
        {
            QuiesceDeath = quiesceDeath;
        }
        public string FindBestMove(Chess chess, int deapth)
        {
            string bestMove = string.Empty;
            bool isMaximizingPlayer = chess.MoveColor == Color.white;
            var validMoves = chess.GetValidMoves();

            if (isMaximizingPlayer)
            {
                double max = int.MinValue;

                foreach (var move in validMoves)
                {
                    var ans = AlphaBetaMin(chess.Move(move), max, int.MaxValue, deapth - 1);
                    if (ans > max)
                    {
                        max = ans;
                        bestMove = move;
                    }
                }
            }
            else
            {
                double min = int.MaxValue;
                foreach (var move in validMoves)
                {
                    var ans = AlphaBetaMax(chess.Move(move), int.MinValue, min, deapth - 1);
                    if (ans < min)
                    {
                        min = ans;
                        bestMove = move;
                    }
                }
            }

            return bestMove;
        }

        public double EvaluatePosition(Chess chess)
        {
            double total = 0.0;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var figure = chess.GetFigureAt(x, y);
                    if (figure == '.')
                    {
                        continue;
                    }

                    var figurePrice = FiguresConstants.FigurePrice[figure];
                    var figureFactor = FiguresConstants.GetFigureFactorAtPosition(figure, y, x);

                    total += (figurePrice + figureFactor);
                }
            }

            return total;
        }

        private double AlphaBetaMax(Chess chess, double alpha, double beta, int depthleft)
        {
            if (alpha == int.MaxValue)
            {
                return alpha;
            }
            if (chess.IsStaleMate)
            {
                return 0;
            }

            if (chess.IsCheckMate)
            {
                return int.MinValue;
            }

            if (depthleft == 0) return QuiesceMax(chess, alpha, beta, QuiesceDeath);

            var moves = chess.GetValidMoves();
            foreach (var move in moves)
            {
                var score = AlphaBetaMin(chess, alpha, beta, depthleft - 1);
                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }
            return alpha;
        }

        private double AlphaBetaMin(Chess chess, double alpha, double beta, int depthleft)
        {
            if (beta == int.MinValue)
            {
                return beta;
            }

            if (chess.IsStaleMate)
            {
                return 0;
            }

            if (chess.IsCheckMate)
            {
                return int.MaxValue;
            }

            if (depthleft == 0) return QuiesceMin(chess, alpha, beta, QuiesceDeath);

            var moves = chess.GetValidMoves();
            foreach (var move in moves)
            {
                var score = AlphaBetaMax(chess, alpha, beta, depthleft - 1);
                if (score <= alpha)
                    return alpha; // fail hard alpha-cutoff
                if (score < beta)
                    beta = score; // beta acts like min in MiniMax
            }
            return beta;
        }

        private double QuiesceMax(Chess chess, double alpha, double beta, int deapth)
        {
            if (chess.IsStaleMate)
            {
                return 0;
            }

            if (chess.IsCheckMate)
            {
                return int.MinValue;
            }

            if (deapth == 0)
            {
                return EvaluatePosition(chess);
            }

            var score = EvaluatePosition(chess);
            if (score >= beta)
                return beta;
            if (alpha < score)
                alpha = score;

            var captures = chess.GetValidCaptures();
            foreach (var capture in captures)
            {
                score = QuiesceMin(chess.Move(capture), alpha, beta, deapth - 1);

                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }

            return alpha;
        }

        private double QuiesceMin(Chess chess, double alpha, double beta, int deapth)
        {
            if (chess.IsStaleMate)
            {
                return 0;
            }

            if (chess.IsCheckMate)
            {
                return int.MaxValue;
            }

            if (deapth == 0)
            {
                return EvaluatePosition(chess);
            }

            var score = EvaluatePosition(chess);
            if (score <= alpha)
                return alpha; // fail hard alpha-cutoff
            if (score < beta)
                beta = score; // beta acts like min in MiniMax

            var captures = chess.GetValidCaptures();
            foreach (var capture in captures)
            {
                score = QuiesceMax(chess.Move(capture), alpha, beta, deapth - 1);

                if (score <= alpha)
                    return alpha;

                if (score < beta)
                    beta = score;
            }

            return beta;
        }
    }
}

