using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
namespace ChessGame
{
    public class Move
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Value { get; set; }
        public int AttackValue { get; set; }
        public bool IsAttack { get; set; }
        public int KingPosition { get; set; }

        public Move()
        {
            To = 0;
            From = 0;
        }

        public Move(int pFrom, int pTo)
        {
            this.From = pFrom;
            this.To = pTo;
        }

        public Move(int pTo, int pFrom, int pValue, bool pAttack)
        {
            this.To = pTo;
            this.From = pFrom;
            this.Value = pValue;
            this.IsAttack = pAttack;
        }

        /******************************************************************
        *     Checks if selected move is in list of valid moves
        ******************************************************************/
        public bool IsValidMove(List<Move> validMoves)
        {
            foreach (Move m in validMoves)
            {
                if (m.To == this.To && m.From == this.From)
                {
                    return true;
                }
            }
            return false;
        }

        /******************************************************************
        *     Checks if selected move will result in check
        ******************************************************************/
        public bool IsCheckMove(List<Move> validMoves, int pos, int dest, Board b, string player)
        {
            List<Move> valids = new List<Move>();
            List<Move> aValid = new List<Move>();

            //check if the valid move results in check from opponent
            Int64 startMask = 1L << b.pieceIdBoard[dest].PiecePosition;
            Int64 endMask = 1L << b.pieceIdBoard[pos].PiecePosition;
            //make move
            switch (b.pieceIdBoard[pos].PieceName)
            {
                case "Wp":
                    b.WP = (b.WP & ~endMask) | startMask;
                    break;
                case "Wn":
                    b.WN = (b.WN & ~endMask) | startMask;
                    break;
                case "Wk":
                    b.WK = (b.WK & ~endMask) | startMask;
                    break;
                case "Wb":
                    b.WB = (b.WB & ~endMask) | startMask;
                    break;
                case "Wr":
                    b.WR = (b.WR & ~endMask) | startMask;
                    break;
                case "Wq":
                    b.WQ = (b.WQ & ~endMask) | startMask;
                    break;
                case "BP":
                    b.BP = (b.BP & ~endMask) | startMask;
                    break;
                case "BN":
                    b.BN = (b.BN & ~endMask) | startMask;
                    break;
                case "BK":
                    b.BK = (b.BK & ~endMask) | startMask;
                    break;
                case "BB":
                    b.BB = (b.BB & ~endMask) | startMask;
                    break;
                case "BR":
                    b.BR = (b.BR & ~endMask) | startMask;
                    break;
                case "BQ":
                    b.BQ = (b.BQ & ~endMask) | startMask;
                    break;
                default:
                    break;
            }
            switch (b.pieceIdBoard[dest].PieceName)
            {
                case "Wp":
                    b.WP = (b.WP) ^ (startMask);
                    break;
                case "Wn":
                    b.WN = (b.WN) ^ (startMask);
                    break;
                case "Wk":
                    b.WK = (b.WK) ^ (startMask);
                    break;
                case "Wb":
                    b.WB = (b.WB) ^ (startMask);
                    break;
                case "Wr":
                    b.WR = (b.WR) ^ (startMask);
                    break;
                case "Wq":
                    b.WQ = (b.WQ) ^ (startMask);
                    break;
                case "BP":
                    b.BP = (b.BP) ^ (startMask);
                    break;
                case "BN":
                    b.BN = (b.BN) ^ (startMask);
                    break;
                case "BK":
                    b.BK = (b.BK) ^ (startMask);
                    break;
                case "BB":
                    b.BB = (b.BB) ^ (startMask);
                    break;
                case "BR":
                    b.BR = (b.BR) ^ (startMask);
                    break;
                case "BQ":
                    b.BQ = (b.BQ) ^ (startMask);
                    break;
                default:
                    break;
            }

            b.Whitepieces = b.WP | b.WB | b.WK | b.WN | b.WQ | b.WR;
            b.Blackpieces = b.BP | b.BB | b.BK | b.BN | b.BQ | b.BR;
            b.EmptySpaces = ~(b.Whitepieces | b.Blackpieces);

            List<Move> list2 = new List<Move>();
            Rules newRules2 = new Rules();

            if (player == "W")
            {
                // get opponent's moves
                list2 = newRules2.GetComputerRules(b);

                // if white king position is same as opponent's to position, 
                // this would result in check
                foreach (Move move2 in list2)
                {
                    if (((Convert.ToString(b.WK, 2).Length) - 1) == move2.To)
                    {
                        return true;
                    }
                }
                return false;
            }
            list2 = newRules2.GetRules(b);
            // if my white king position is equal to a valid black moves destination position, then this results in check, so do not include in 
            //possible valid moves
            foreach (Move move2 in list2)
            {
                if (((Convert.ToString(b.BK, 2).Length) - 1) == move2.To)
                {
                    return true;
                }
            }
            return false;

        }

        /******************************************************************
        *     Checks if selected move results in opponent check
        ******************************************************************/
        public bool OpponentInCheck(List<Move> allLegalMoves, int pos, int dest, ref Board b, string player)
        {
            List<Move> valids = new List<Move>();
            List<Move> aValid = new List<Move>();

            //check if the valid move results in check from opponent
            Int64 startMask = 1L << b.pieceIdBoard[dest].PiecePosition;
            Int64 endMask = 1L << b.pieceIdBoard[pos].PiecePosition;
            //make move
            switch (b.pieceIdBoard[pos].PieceName)
            {
                case "Wp":
                    b.WP = (b.WP & ~endMask) | startMask;
                    break;
                case "Wn":
                    b.WN = (b.WN & ~endMask) | startMask;
                    break;
                case "Wk":
                    b.WK = (b.WK & ~endMask) | startMask;
                    break;
                case "Wb":
                    b.WB = (b.WB & ~endMask) | startMask;
                    break;
                case "Wr":
                    b.WR = (b.WR & ~endMask) | startMask;
                    break;
                case "Wq":
                    b.WQ = (b.WQ & ~endMask) | startMask;
                    break;
                case "BP":
                    b.BP = (b.BP & ~endMask) | startMask;
                    break;
                case "BN":
                    b.BN = (b.BN & ~endMask) | startMask;
                    break;
                case "BK":
                    b.BK = (b.BK & ~endMask) | startMask;
                    break;
                case "BB":
                    b.BB = (b.BB & ~endMask) | startMask;
                    break;
                case "BR":
                    b.BR = (b.BR & ~endMask) | startMask;
                    break;
                case "BQ":
                    b.BQ = (b.BQ & ~endMask) | startMask;
                    break;
                default:
                    break;
            }
            switch (b.pieceIdBoard[dest].PieceName)
            {
                case "Wp":
                    b.WP = (b.WP) ^ (startMask);
                    break;
                case "Wn":
                    b.WN = (b.WN) ^ (startMask);
                    break;
                case "Wk":
                    b.WK = (b.WK) ^ (startMask);
                    break;
                case "Wb":
                    b.WB = (b.WB) ^ (startMask);
                    break;
                case "Wr":
                    b.WR = (b.WR) ^ (startMask);
                    break;
                case "Wq":
                    b.WQ = (b.WQ) ^ (startMask);
                    break;
                case "BP":
                    b.BP = (b.BP) ^ (startMask);
                    break;
                case "BN":
                    b.BN = (b.BN) ^ (startMask);
                    break;
                case "BK":
                    b.BK = (b.BK) ^ (startMask);
                    break;
                case "BB":
                    b.BB = (b.BB) ^ (startMask);
                    break;
                case "BR":
                    b.BR = (b.BR) ^ (startMask);
                    break;
                case "BQ":
                    b.BQ = (b.BQ) ^ (startMask);
                    break;
                default:
                    break;
            }

            b.Whitepieces = b.WP | b.WB | b.WK | b.WN | b.WQ | b.WR;
            b.Blackpieces = b.BP | b.BB | b.BK | b.BN | b.BQ | b.BR;
            b.EmptySpaces = ~(b.Whitepieces | b.Blackpieces);

            List<Move> list2 = new List<Move>();
            Rules newRules2 = new Rules();
            if (player == "W")
            {

                list2 = newRules2.GetRules(b);
                foreach (Move move2 in list2)
                {
                    if (((Convert.ToString(b.BK, 2).Length) - 1) == move2.To)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                list2 = newRules2.GetComputerRules(b);
                foreach (Move move2 in list2)
                {
                    if (((Convert.ToString(b.WK, 2).Length) - 1) == move2.To)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /******************************************************************
        *     Evaluates best computer move using MinMax algorithm with 
        *     alpha-beta pruning optimization
        ******************************************************************/
        public List<Move> EvaluateMoves(int depth, Board b)
        {
            KingPosition = b.GetPosition(b.WK);
            List<Move> tnewGameMoves = new List<Move>();
            Rules rules3 = new Rules();
            tnewGameMoves = rules3.GetComputerRules(b);

            List<Move> newGameMoves = tnewGameMoves.OrderByDescending(o => o.Value).ToList();
            int alpha = Int32.MinValue;
            int beta = Int32.MaxValue;
            int bestMove = alpha;
            Move bestMoveFound = new Move();
            List<Move> returnList = new List<Move>();
            bool immediateAttack = false;

            for (int j = 0; j < newGameMoves.Count; j++)
            {
                Move myMove = newGameMoves[j];
                string tempToName = b.pieceIdBoard[myMove.To].PieceName;
                int tempToValue = b.pieceIdBoard[myMove.To].PieceValue;
                string tempFromName = b.pieceIdBoard[myMove.From].PieceName;
                int tempFromValue = b.pieceIdBoard[myMove.From].PieceValue;
                AttackValue = 0;

                Int64 startMask = 1L << b.pieceIdBoard[myMove.From].PiecePosition;
                Int64 endMask = 1L << b.pieceIdBoard[myMove.To].PiecePosition;

                //Make the move
                MakeMove(b, myMove, startMask, endMask);

                //check for immediate attack
                immediateAttack = CheckForImmediateAttack(b, rules3, immediateAttack, myMove, tempToValue);

                // if none, get the best move
                if (!immediateAttack)
                {
                    int value = MinMax(depth - 1, false, alpha, beta, b);

                    if (value >= bestMove)
                    {
                        if (bestMoveFound.To == 0 && bestMoveFound.From == 0)
                        {
                            bestMoveFound = myMove;
                        }
                        bestMoveFound = myMove;
                        returnList.Add(bestMoveFound);
                        bestMove = value;
                    }
                }

                UndoMove(b, myMove, tempToName, tempToValue, tempFromName, tempFromValue, startMask, endMask);

                immediateAttack = false;
            }
            List<Move> ret = returnList.OrderBy(o => o.Value).ToList();
            return ret;
        }

        /****************************************************************************
        *     Checks if move made will result in immediate attack
        ****************************************************************************/
        private static bool CheckForImmediateAttack(Board b, Rules rules3, bool skip, Move myMove, int tempToValue)
        {
            List<Move> pnewGameMoves = rules3.GetRules(b);
            foreach (Move m in pnewGameMoves)
            {
                if (m.To == myMove.To)
                {
                    if (tempToValue <= Math.Abs(b.pieceIdBoard[myMove.To].PieceValue))
                    {
                        skip = true;
                    }
                }
            }
            return skip;
        }

        /****************************************************************************
       *    Undoes move made to restore original board
       ****************************************************************************/
        private static void UndoMove(Board b, Move myMove, string tempToName, int tempToValue, string tempFromName, int tempFromValue, long startMask, long endMask)
        {
            b.pieceIdBoard[myMove.From].PieceName = tempFromName;
            b.pieceIdBoard[myMove.From].PieceValue = tempFromValue;
            b.pieceIdBoard[myMove.To].PieceName = tempToName;
            b.pieceIdBoard[myMove.To].PieceValue = tempToValue;

            switch (tempFromName)
            {
                case "Wp":
                    b.WP = (b.WP & ~endMask) | startMask;
                    b.boardUtils.whiteFirstMove = (b.boardUtils.whiteFirstMove & ~endMask) | startMask;
                    break;
                case "Wn":
                    b.WN = (b.WN & ~endMask) | startMask;
                    break;
                case "Wk":
                    b.WK = (b.WK & ~endMask) | startMask;
                    break;
                case "Wb":
                    b.WB = (b.WB & ~endMask) | startMask;
                    break;
                case "Wr":
                    b.WR = (b.WR & ~endMask) | startMask;
                    break;
                case "Wq":
                    b.WQ = (b.WQ & ~endMask) | startMask;
                    break;
                case "BP":
                    b.BP = (b.BP & ~endMask) | startMask;
                    b.boardUtils.blackFirstMove = (b.boardUtils.blackFirstMove & ~endMask) | startMask;
                    break;
                case "BN":
                    b.BN = (b.BN & ~endMask) | startMask;
                    break;
                case "BK":
                    b.BK = (b.BK & ~endMask) | startMask;
                    break;
                case "BB":
                    b.BB = (b.BB & ~endMask) | startMask;
                    break;
                case "BR":
                    b.BR = (b.BR & ~endMask) | startMask;
                    break;
                case "BQ":
                    b.BQ = (b.BQ & ~endMask) | startMask;
                    break;
                default:
                    break;
            }

            switch (tempToName)
            {
                case "Wp":
                    b.WP = (b.WP) | endMask;
                    b.boardUtils.whiteFirstMove = (b.boardUtils.whiteFirstMove) | endMask;
                    break;
                case "Wn":
                    b.WN = (b.WN) | endMask;
                    break;
                case "Wk":
                    b.WK = (b.WK) | endMask;
                    break;
                case "Wb":
                    b.WB = (b.WB) | endMask;
                    break;
                case "Wr":
                    b.WR = (b.WR) | endMask;
                    break;
                case "Wq":
                    b.WQ = (b.WQ) | endMask;
                    break;
                case "BP":
                    b.BP = (b.BP) | endMask;
                    b.boardUtils.blackFirstMove = (b.boardUtils.blackFirstMove) | endMask;
                    break;
                case "BN":
                    b.BN = (b.BN) | endMask;
                    break;
                case "BK":
                    b.BK = (b.BK) | endMask;
                    break;
                case "BB":
                    b.BB = (b.BB) | endMask;
                    break;
                case "BR":
                    b.BR = (b.BR) | endMask;
                    break;
                case "BQ":
                    b.BQ = (b.BQ) | endMask;
                    break;
                default:
                    break;
            }
            b.Whitepieces = b.WP | b.WB | b.WK | b.WN | b.WQ | b.WR;
            b.Blackpieces = b.BP | b.BB | b.BK | b.BN | b.BQ | b.BR;
        }

        /****************************************************************************
       *     Makes move to create new game state
       ****************************************************************************/
        private static void MakeMove(Board b, Move myMove, long startMask, long endMask)
        {
            switch (b.pieceIdBoard[myMove.From].PieceName)
            {
                case "Wp":
                    b.WP = (b.WP ^ startMask) | endMask;
                    b.boardUtils.whiteFirstMove = (b.boardUtils.whiteFirstMove ^ ~startMask) | endMask;
                    break;
                case "Wn":
                    b.WN = (b.WN ^ startMask) | endMask;
                    break;
                case "Wk":
                    b.WK = (b.WK ^ startMask) | endMask;
                    break;
                case "Wb":
                    b.WB = (b.WB ^ startMask) | endMask;
                    break;
                case "Wr":
                    b.WR = (b.WR ^ startMask) | endMask;
                    break;
                case "Wq":
                    b.WQ = (b.WQ ^ startMask) | endMask; ;
                    break;
                case "BP":
                    b.BP = (b.BP ^ startMask) | endMask;
                    b.boardUtils.blackFirstMove = (b.boardUtils.blackFirstMove ^ startMask) | endMask;
                    break;
                case "BN":
                    b.BN = (b.BN ^ startMask) | endMask;
                    break;
                case "BK":
                    b.BK = (b.BK & startMask) | endMask;
                    break;
                case "BB":
                    b.BB = (b.BB ^ startMask) | endMask;
                    break;
                case "BR":
                    b.BR = (b.BR ^ startMask) | endMask;
                    break;
                case "BQ":
                    b.BQ = (b.BQ ^ startMask) | endMask;
                    break;
                default:
                    break;
            }

            switch (b.pieceIdBoard[myMove.To].PieceName)
            {
                case "Wp":
                    b.WP = b.WP & ~endMask;
                    b.boardUtils.whiteFirstMove = b.boardUtils.whiteFirstMove & ~endMask;
                    break;
                case "Wn":
                    b.WN = b.WN & ~endMask;
                    break;
                case "Wk":
                    b.WK = b.WK & ~endMask;
                    break;
                case "Wb":
                    b.WB = b.WB & ~endMask;
                    break;
                case "Wr":
                    b.WR = b.WR & ~endMask;
                    break;
                case "Wq":
                    b.WQ = b.WQ & ~endMask;
                    break;
                case "BP":
                    b.BP = b.BP & ~endMask;
                    b.boardUtils.blackFirstMove = b.boardUtils.blackFirstMove & ~endMask;
                    break;
                case "BN":
                    b.BN = b.BN & ~endMask;
                    break;
                case "BK":
                    b.BK = b.BK & ~endMask;
                    break;
                case "BB":
                    b.BB = b.BB & ~endMask;
                    break;
                case "BR":
                    b.BR = b.BR & ~endMask;
                    break;
                case "BQ":
                    b.BQ = b.BQ & ~endMask;
                    break;
                default:
                    break;
            }

            b.Whitepieces = b.WP | b.WB | b.WK | b.WN | b.WQ | b.WR;
            b.Blackpieces = b.BP | b.BB | b.BK | b.BN | b.BQ | b.BR;
            b.boardUtils.emptySpaces = ~b.Whitepieces & ~b.Blackpieces;
            b.pieceIdBoard[myMove.To].PieceName = b.pieceIdBoard[myMove.From].PieceName;
            b.pieceIdBoard[myMove.To].PieceValue = b.pieceIdBoard[myMove.From].PieceValue;
            b.pieceIdBoard[myMove.From].PieceName = "-";
            b.pieceIdBoard[myMove.From].PieceValue = 0;
        }

        /****************************************************************************
       *     Creates test game state by making sequential moves up to a specified 
       *     depth and returns the total value of the game state
       ****************************************************************************/
        public int MinMax(int depth, bool isMaximiser, int alpha, int beta, Board b)
        {
            //end of specified traversal length, evaluate board
            if (depth == 0)
            {
                int k = -EvaluateBoard(b.pieceIdBoard);
                return k;
            }

            List<Move> newGameMoves = new List<Move>();
            List<Move> pnewGameMoves = new List<Move>();
            Rules rules3 = new Rules();
            if (isMaximiser)
            {
                pnewGameMoves = rules3.GetComputerRules(b);
                newGameMoves = pnewGameMoves.OrderByDescending(o => o.Value).ToList();
                int bestMove = Int32.MinValue;
                for (int i = 0; i < newGameMoves.Count; i++)
                {
                    Move myMove = newGameMoves[i];
                    string tempToName = b.pieceIdBoard[myMove.To].PieceName;
                    int tempToValue = b.pieceIdBoard[myMove.To].PieceValue;
                    string tempFromName = b.pieceIdBoard[myMove.From].PieceName;
                    int tempFromValue = b.pieceIdBoard[myMove.From].PieceValue;
                    int minValue = b.pieceIdBoard[myMove.To].PieceValue;

                    Int64 startMask = 1L << b.pieceIdBoard[myMove.From].PiecePosition;
                    Int64 endMask = 1L << b.pieceIdBoard[myMove.To].PiecePosition;

                    MakeMove(b, myMove, startMask, endMask);

                    bestMove = Math.Max(bestMove, MinMax(depth - 1, false, alpha, beta, b));

                    UndoMove(b, myMove, tempToName, tempToValue, tempFromName, tempFromValue, startMask, endMask);

                    alpha = Math.Max(alpha, bestMove);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return bestMove;
            }
           
            else
            {
                pnewGameMoves = rules3.GetRules(b);
                newGameMoves = pnewGameMoves.OrderByDescending(o => o.Value).ToList();
                int bestMove = Int32.MaxValue;

                for (int i = 0; i < newGameMoves.Count; i++)
                {
                    Move myMove = newGameMoves[i];
                    int maxValue = b.pieceIdBoard[myMove.To].PieceValue;
                    string tempToName = b.pieceIdBoard[myMove.To].PieceName;
                    int tempToValue = b.pieceIdBoard[myMove.To].PieceValue;
                    string tempFromName = b.pieceIdBoard[myMove.From].PieceName;
                    int tempFromValue = b.pieceIdBoard[myMove.From].PieceValue;
                    int minValue = b.pieceIdBoard[myMove.To].PieceValue;

                    Int64 startMask = 1L << b.pieceIdBoard[myMove.From].PiecePosition;
                    Int64 endMask = 1L << b.pieceIdBoard[myMove.To].PiecePosition;

                    MakeMove(b, myMove, startMask, endMask);
                    bestMove = Math.Min(bestMove, MinMax(depth - 1, true, alpha, beta, b));
                    UndoMove(b, myMove, tempToName, tempToValue, tempFromName, tempFromValue, startMask, endMask);

                    beta = Math.Min(beta, bestMove);

                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return bestMove;
            }
        }

        /****************************************************************************
        *     Adds values of pieceIdBoard after test moves are made, determines best
        *     move in EvaluateMove
        ****************************************************************************/
        public int EvaluateBoard(Piece[] pieceIdBoard)
        {
            int totalEvaluation = 0;
            for (int i = 0; i < Board.BoardSize; i++)
            {
                totalEvaluation += pieceIdBoard[i].PieceValue;
            }
            totalEvaluation += AttackValue;

            return totalEvaluation;
        }

       


    }

}