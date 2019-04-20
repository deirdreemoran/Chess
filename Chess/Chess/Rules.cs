using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessGame
{
    public class Rules
    {
        public List<Move> GetRules(Board b)
        {
            List<List<Move>> allMoves = new List<List<Move>>();
            List<Move> moves = new List<Move>();
            List<Move> final = new List<Move>();

            if ((moves = GetWhitePawnMoves(b)).Count != 0) { 
                allMoves.Add(moves);
            }
            if ((moves = CalculateMoves(b.WR, "R", b.Whitepieces, b)).Count != 0) {
                allMoves.Add(moves);
            }
            if ((moves = CalculateMoves(b.WN, "N", b.Whitepieces, b)).Count != 0) {
                allMoves.Add(moves);
            }
            if ((moves = CalculateMoves(b.WK, "K", b.Whitepieces, b)).Count != 0) {
                allMoves.Add(moves);
            }
            if ((moves = CalculateMoves(b.WB, "B", b.Whitepieces, b)).Count != 0) {
                allMoves.Add(moves);
            }
            if ((moves = CalculateMoves(b.WQ, "R", b.Whitepieces, b)).Count != 0) {
                allMoves.Add(moves);
            }
            if ((moves = CalculateMoves(b.WQ, "B", b.Whitepieces, b)).Count != 0) {
                allMoves.Add(moves);
            }

            foreach (List<Move> list in allMoves)
            {
                foreach (Move m in list)
                {
                    final.Add(m);
                    System.Console.WriteLine("FROM: " + m.From + " TO: " + m.To);
                }
            }

            return final;
        }

        public List<Move> GetComputerRules(Board b)
        {
            List<List<Move>> allMoves = new List<List<Move>>();
            List<Move> moves = new List<Move>();
            List<Move> final = new List<Move>();
            if ((moves = GetBlackPawnMoves(b)).Count != 0) { allMoves.Add(moves); }
            if ((moves = CalculateBlackMoves(b.BR, "R", b.Blackpieces, b)).Count != 0) { allMoves.Add(moves); }
            if ((moves = CalculateBlackMoves(b.BN, "N", b.Blackpieces, b)).Count != 0) { allMoves.Add(moves); }
            if ((moves = CalculateBlackMoves(b.BK, "K", b.Blackpieces, b)).Count != 0) { allMoves.Add(moves); }
            if ((moves = CalculateBlackMoves(b.BB, "B", b.Blackpieces, b)).Count != 0) { allMoves.Add(moves); }
            if ((moves = CalculateBlackMoves(b.BQ, "R", b.Blackpieces, b)).Count != 0) { allMoves.Add(moves); }
            if ((moves = CalculateBlackMoves(b.BQ, "B", b.Blackpieces, b)).Count != 0) { allMoves.Add(moves); }

            System.Console.WriteLine();
            System.Console.WriteLine("computer");
            foreach (List<Move> list in allMoves)
            {
                foreach (Move m in list)
                {
                    final.Add(m);
                    System.Console.WriteLine("FROM: " + m.From + " TO: " + m.To);

                }
            }

            return final;
        }

        private Int64 GetKingMoves(Int64 pieces, Int64 pieceAlliance, Int64 notColumnSeven, Int64 notColumnZero)
        {
            int loc = (Convert.ToString(pieces, 2).Length) - 1;
            Int64 result = (((pieces & notColumnZero) >> 1) |
                       ((pieces & notColumnSeven) << 1) |
                       ((pieces & notColumnSeven) >> 7) |
                       ((pieces & notColumnZero) << 7) |
                       ((pieces & notColumnZero) >> 9) |
                       ((pieces & notColumnSeven) << 9) |
                       pieces << 8 | pieces >> 8) & (~pieceAlliance);
            return result;
        }

        private List<Move> CalculateMoves(Int64 piece, string W, Int64 alliancePieces, Board b)
        {
            List<Move> allValidMoves = new List<Move>();
            Int64 pieces = piece & ~(piece - 1);
            Int64 result = piece;
            Int64 potentialMove = 0L;
            int loc = 0;
            while (pieces != 0)
            {
                loc = (Convert.ToString(pieces, 2).Length) - 1;
                if (W == "R")
                {
                    potentialMove = GetVerticalAndHorizontal(loc, alliancePieces, b.Blackpieces, b);
                    potentialMove &= ~alliancePieces;
                }
                if (W == "B")
                {
                    potentialMove = GetDiagonals(loc, b);
                    potentialMove &= ~alliancePieces;
                }
                if (W == "N") { potentialMove = GetKnightMoves(loc, alliancePieces, b); }
                if (W == "K") { potentialMove = GetKingMoves(pieces, alliancePieces, b.boardUtils.notColumnSeven, b.boardUtils.notColumnZero); }

                for (int j = 0; j < 64; j++)
                {
                    if (((potentialMove >> j) & 1) == 1)
                    {
                        Int64 end = potentialMove & b.Whitepieces;
                        if (((end >> j) & 1) == 1)
                        {
                            allValidMoves.Add(new Move(j, loc, b.pieceIdBoard[j].PieceValue, true));
                        }
                        else
                        {
                            allValidMoves.Add(new Move(j, loc, b.pieceIdBoard[j].PieceValue, false));
                        }
                        potentialMove &= ~j;
                    }
                }
                result &= ~pieces;
                pieces = result & ~(result - 1);
            }
            return allValidMoves;
        }


        private List<Move> CalculateBlackMoves(Int64 piece, string W, Int64 alliancePieces, Board b)
        {
            List<Move> allValidMoves = new List<Move>();
            Int64 pieces = piece & ~(piece - 1);
            Int64 result = piece;
            Int64 potentialMove = 0L;
            int loc = 0;
            while (pieces != 0)
            {
                loc = (Convert.ToString(pieces, 2).Length) - 1;
                if (W == "R")
                {
                    potentialMove = GetVerticalAndHorizontal(loc, alliancePieces, b.Whitepieces, b);
                    potentialMove &= ~alliancePieces;
                }

                if (W == "B")
                {
                    potentialMove = GetDiagonals(loc, b);
                    potentialMove &= ~alliancePieces;
                }
                if (W == "N") { potentialMove = GetKnightMoves(loc, alliancePieces, b); }
                if (W == "K") { potentialMove = GetKingMoves(pieces, alliancePieces, b.boardUtils.notColumnSeven, b.boardUtils.notColumnZero); }

                for (int j = 0; j < 64; j++)
                {
                    if (((potentialMove >> j) & 1) == 1)
                    {
                        Int64 end = potentialMove & b.Blackpieces;
                        if (((end >> j) & 1) == 1)
                        {
                            allValidMoves.Add(new Move(j, loc, b.pieceIdBoard[j].PieceValue, true));
                        }
                        else
                        {
                            allValidMoves.Add(new Move(j, loc, b.pieceIdBoard[j].PieceValue, false));
                        }
                        potentialMove &= ~j;
                    }
                }
                result &= ~pieces;
                pieces = result & ~(result - 1);
            }
            return allValidMoves;
        }

        private Int64 GetKnightMoves(int num, Int64 alliancePieces, Board b)
        {
            Int64 numBitMap = 1L << num;
            Int64 result =
                     (((numBitMap & b.boardUtils.notColumnZero) << 15) |
                     ((numBitMap & b.boardUtils.notColumnSeven) << 17) |
                     ((numBitMap & b.boardUtils.notColumnSeven) >> 15) |
                     ((numBitMap & b.boardUtils.notColumnZero) >> 17) |
                     ((numBitMap & b.boardUtils.emptyColumnZeroToOne) << 6) |
                     ((numBitMap & b.boardUtils.emptyColumnSixToSeven) << 10) |
                     ((numBitMap & b.boardUtils.emptyColumnSixToSeven) >> 6) |
                     ((numBitMap & b.boardUtils.emptyColumnZeroToOne) >> 10)) & ~alliancePieces;
            return result;
        }

        private List<Move> GetBlackPawnMoves(Board b)
        {
            List<Move> allValidMoves = new List<Move>();
            Int64 result = 0L;
            Int64 result3 = 0L;
            Int64 result2 = 0L;
            Int64 result4 = 0L;
            Int64 result5 = 0L;
            Int64 result6 = 0L;
            Int64 result7 = 0L;
            //for all valid forward one moves
            result = (b.BP >> 8) & b.EmptySpaces;
            result2 = (b.BP >> 7) & ~b.Blackpieces & b.Whitepieces & b.boardUtils.notColumnSeven;
            result3 = (b.BP >> 9) & ~b.Blackpieces & b.Whitepieces & b.boardUtils.notColumnZero;
            result4 = (b.BP >> 16) & ~b.Whitepieces & ~b.Blackpieces;
            result5 = (b.BP >> 8) & b.EmptySpaces;
            result6 = (result5 >> 8) & result4;
            result7 = ((b.boardUtils.blackFirstMove & b.BP) >> 16) & result6;

            // forward two 
            for (int i = 0; i < 64; i++)
            {
                if (((result >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i + 8, b.pieceIdBoard[i].PieceValue, false)); }
                if (((result2 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i + 7, b.pieceIdBoard[i].PieceValue, true)); }
                if (((result3 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i + 9, b.pieceIdBoard[i].PieceValue, true)); }
                if (((result7 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i + 16, b.pieceIdBoard[i].PieceValue, false)); }
            }
            return allValidMoves;
        }


        private List<Move> GetWhitePawnMoves(Board b)
        {
            //TODO en passant, promotions, incorporate knowledge of first move
            List<Move> allValidMoves = new List<Move>();
            //for all valid one up moves
            Int64 result = (b.WP << 8) & b.EmptySpaces;
            Int64 result2 = (b.WP << 7) & b.Blackpieces & b.boardUtils.notColumnZero;
            Int64 result3 = (b.WP << 9) & b.Blackpieces & b.boardUtils.notColumnSeven;
            Int64 result4 = (b.WP << 16) & ~b.Whitepieces & ~b.Blackpieces;
            Int64 result5 = (b.WP << 8) & ~b.Whitepieces & ~b.Blackpieces;
            Int64 result6 = (result5 << 8) & result4;
            Int64 result7 = ((b.boardUtils.whiteFirstMove & b.WP) << 16) & result6;

            // result of moving up two and empty up one and blackorempty at up 2
            for (int i = 0; i < Board.BoardSize; i++)
            {
                if (((result >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i - 8, b.pieceIdBoard[i].PieceValue, false)); }
                if (((result2 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i - 7, b.pieceIdBoard[i].PieceValue, true)); }
                if (((result3 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i - 9, b.pieceIdBoard[i].PieceValue, true)); }
                if (((result7 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i - 16, b.pieceIdBoard[i].PieceValue, false)); }
            }
            return allValidMoves;
        }


        public Int64 Reverse(Int64 num)
        {
            Int64 output = Convert.ToInt64(string.Concat(Convert.ToString(num, 2).PadLeft(64, '0').Reverse()), 2);
            return output;
        }

        public Int64 GetVerticalAndHorizontal(int num, Int64 whitePieces, Int64 blackPieces, Board b)
        {
            Int64 occupied = blackPieces | whitePieces;
            Int64 numBitMap = 1L << num;
            Int64 horizontal = ((blackPieces | whitePieces) - 2 * numBitMap) ^ Reverse(Reverse(occupied) - 2 * Reverse(numBitMap));
            Int64 vertical = (((blackPieces | whitePieces) & b.boardUtils.columnMasks[num % 8]) -
                    (2 * numBitMap)) ^ Reverse(Reverse(occupied & b.boardUtils.columnMasks[num % 8]) - (2 * Reverse(numBitMap)));
            Int64 res = (horizontal & b.boardUtils.rowMasks[num / 8]) | (vertical & b.boardUtils.columnMasks[num % 8]);
            return res;
        }

        public Int64 GetDiagonals(int num, Board b)
        {
            Int64 occupied = b.Blackpieces | b.Whitepieces;
            Int64 numBitMap = 1L << num;
            Int64 possibilitiesDiagonal = ((occupied & b.boardUtils.diagonalMasks1[(num / 8) + (num % 8)]) - (2 * numBitMap)) ^ Reverse(Reverse(occupied & b.boardUtils.diagonalMasks1[(num / 8) + (num % 8)]) - (2 * Reverse(numBitMap)));
            Int64 possibilitiesAntiDiagonal = ((occupied & b.boardUtils.diagonalMasks2[(num / 8) + 7 - (num % 8)]) - (2 * numBitMap)) ^ Reverse(Reverse(occupied & b.boardUtils.diagonalMasks2[(num / 8) + 7 - (num % 8)]) - (2 * Reverse(numBitMap)));
            return (possibilitiesDiagonal & b.boardUtils.diagonalMasks1[(num / 8) + (num % 8)]) | (possibilitiesAntiDiagonal & b.boardUtils.diagonalMasks2[(num / 8) + 7 - (num % 8)]);
        }

        public List<Move> GetOutOfCheck(Board b)
        {
            List<Move> validMove = new List<Move>();

            int loc = (Convert.ToString(b.BK, 2).Length) - 1;

            // generate potential moves for all current player pieces
            List<Move> list = new List<Move>();
            Rules newRules = new Rules();
            bool atLeastOneAttack = false;
            list = newRules.GetComputerRules(b);

            foreach (Move move in list)
            {
                Int64 startMask = 1L << move.To;
                Int64 endMask = 1L << move.From;

                switch (b.pieceIdBoard[move.From].PieceName)
                {
                    case "Wp":
                        b.WP = (b.WP & ~endMask) | startMask;
                        if (b.pieceIdBoard[move.From].IsFirstMove) { 
                            b.pieceIdBoard[move.From].IsFirstMove = false;
                        }
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


                b.Whitepieces = b.WP | b.WB | b.WK | b.WN | b.WQ | b.WR;
                b.Blackpieces = b.BP | b.BB | b.BK | b.BN | b.BQ | b.BR;

                b.EmptySpaces = ~(b.Whitepieces | b.Blackpieces);

                List<Move> list2 = new List<Move>();
                Rules newRules2 = new Rules();
                list2 = newRules2.GetRules(b);


                foreach (Move move2 in list2)
                {
                    if (((Convert.ToString(b.BK, 2).Length) - 1) == move2.To)
                    {
                        atLeastOneAttack = true;
                    }
                }

                if (!atLeastOneAttack)
                {
                    validMove.Add(new Move(move.To, move.From, b.pieceIdBoard[move.To].PieceValue, false));
                }
                atLeastOneAttack = false;
            }
            return validMove;
        }



    }
}