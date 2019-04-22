using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessGame
{
    public class Rules
    {
        /******************************************************************
        *     Generates list of legal moves for all white 
        *     (human player) pieces
        ******************************************************************/
        public List<Move> GetWhiteRules(Board b)
        {
            List<Move> pieceMoves = new List<Move>();
            List<Move> allMoves = new List<Move>();

            if ((pieceMoves = GetWhitePawnMoves(b)).Count != 0) { 
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateWhiteMoves(b.WR, "R", b.WhitePieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateWhiteMoves(b.WN, "N", b.WhitePieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateWhiteMoves(b.WK, "K", b.WhitePieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateWhiteMoves(b.WB, "B", b.WhitePieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateWhiteMoves(b.WQ, "R", b.WhitePieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateWhiteMoves(b.WQ, "B", b.WhitePieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            return allMoves;
        }

        /******************************************************************
        *     Generates list of legal moves for all black (computer) pieces
        ******************************************************************/
        public List<Move> GetBlackRules(Board b)
        {
            List<Move> pieceMoves = new List<Move>();
            List<Move> allMoves = new List<Move>();

            if ((pieceMoves = GetBlackPawnMoves(b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateBlackMoves(b.BR, "R", b.BlackPieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateBlackMoves(b.BN, "N", b.BlackPieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateBlackMoves(b.BK, "K", b.BlackPieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateBlackMoves(b.BB, "B", b.BlackPieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateBlackMoves(b.BQ, "R", b.BlackPieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            if ((pieceMoves = CalculateBlackMoves(b.BQ, "B", b.BlackPieces, b)).Count != 0) {
                allMoves.AddRange(pieceMoves);
            }
            return allMoves;
        }

        /******************************************************************
       *     Generates list of legal moves for a player's king piece
       ******************************************************************/
        private long GetKingMoves(long pieces, long pieceAlliance, long notColumnSeven, long notColumnZero)
        {
            long result = (((pieces & notColumnZero) >> 1) |
                           ((pieces & notColumnSeven) << 1) |
                           ((pieces & notColumnSeven) >> 7) |
                           ((pieces & notColumnZero) << 7) |
                           ((pieces & notColumnZero) >> 9) |
                           ((pieces & notColumnSeven) << 9) |
                            pieces << 8 | pieces >> 8) 
                            & (~pieceAlliance);
            return result;
        }

        /******************************************************************
        *    Helper function to generate list of legal moves for all
        *    white pieces  
        ******************************************************************/
        private List<Move> CalculateWhiteMoves(long piece, string W, long alliancePieces, Board b)
        {
            List<Move> allValidMoves = new List<Move>();
            long pieces = piece & ~(piece - 1);
            long result = piece;
            long potentialMove = 0L;
            int loc = 0;
            while (pieces != 0)
            {
                loc = (Convert.ToString(pieces, 2).Length) - 1;
                if (W == "R")
                {
                    potentialMove = GetVerticalAndHorizontal(loc, alliancePieces, b.BlackPieces, b);
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
                        long end = potentialMove & b.WhitePieces;
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

        /******************************************************************
        *    Helper function to generate list of legal moves for all
        *    black pieces  
        ******************************************************************/
        private List<Move> CalculateBlackMoves(long piece, string W, long alliancePieces, Board b)
        {
            List<Move> allValidMoves = new List<Move>();
            long pieces = piece & ~(piece - 1);
            long result = piece;
            long potentialMove = 0L;
            int loc = 0;
            while (pieces != 0)
            {
                loc = (Convert.ToString(pieces, 2).Length) - 1;
                if (W == "R")
                {
                    potentialMove = GetVerticalAndHorizontal(loc, alliancePieces, b.WhitePieces, b);
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
                        long end = potentialMove & b.BlackPieces;
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

        /******************************************************************
        *    Helper function to generate list of legal moves for either
        *    player's knight pieces 
        ******************************************************************/
        private long GetKnightMoves(int num, long alliancePieces, Board b)
        {
            long numBitMap = 1L << num;
            long result =
                     (((numBitMap & b.boardUtils.notColumnZero) << 15) |
                     ((numBitMap & b.boardUtils.notColumnSeven) << 17) |
                     ((numBitMap & b.boardUtils.notColumnSeven) >> 15) |
                     ((numBitMap & b.boardUtils.notColumnZero) >> 17) |
                     ((numBitMap & b.boardUtils.emptyColumnZeroToOne) << 6) |
                     ((numBitMap & b.boardUtils.emptyColumnSixToSeven) << 10) |
                     ((numBitMap & b.boardUtils.emptyColumnSixToSeven) >> 6) |
                     ((numBitMap & b.boardUtils.emptyColumnZeroToOne) >> 10)) 
                     & ~alliancePieces;
            return result;
        }

        /******************************************************************
        *    Helper function to generate list of legal moves for black pawn
        ******************************************************************/
        private List<Move> GetBlackPawnMoves(Board b)
        {
            List<Move> allValidMoves = new List<Move>();
            
            long result = (b.BP >> 8) & b.EmptySpaces;
            long result2 = (b.BP >> 7) & ~b.BlackPieces & b.WhitePieces & b.boardUtils.notColumnZero;
            long result3 = (b.BP >> 9) & ~b.BlackPieces & b.WhitePieces & b.boardUtils.notColumnSeven;
            long result4 = (b.BP >> 16) & ~b.WhitePieces & ~b.BlackPieces;
            long result5 = (b.BP >> 8) & b.EmptySpaces;
            long result6 = (result5 >> 8) & result4;
            long result7 = ((b.boardUtils.blackFirstMove & b.BP) >> 16) & result6;

            for (int i = 0; i < Board.BoardSize; i++)
            {
                if (((result >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i + 8, b.pieceIdBoard[i].PieceValue, false)); }
                if (((result2 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i + 7, b.pieceIdBoard[i].PieceValue, true)); }
                if (((result3 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i + 9, b.pieceIdBoard[i].PieceValue, true)); }
                if (((result7 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i + 16, b.pieceIdBoard[i].PieceValue, false)); }
            }
            return allValidMoves;
        }

        /******************************************************************
        *    Helper function to generate list of legal moves for white pawn
        ******************************************************************/
        private List<Move> GetWhitePawnMoves(Board b)
        {
            //TODO en passant, promotions, incorporate knowledge of first move
            List<Move> allValidMoves = new List<Move>();

            long result = (b.WP << 8) & b.EmptySpaces;
            long result2 = (b.WP << 7) & b.BlackPieces & b.boardUtils.notColumnZero;
            long result3 = (b.WP << 9) & b.BlackPieces & b.boardUtils.notColumnSeven;
            long result4 = (b.WP << 16) & ~b.WhitePieces & ~b.BlackPieces;
            long result5 = (b.WP << 8) & ~b.WhitePieces & ~b.BlackPieces;
            long result6 = (result5 << 8) & result4;
            long result7 = ((b.boardUtils.whiteFirstMove & b.WP) << 16) & result6;

            for (int i = 0; i < Board.BoardSize; i++)
            {
                if (((result >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i - 8, b.pieceIdBoard[i].PieceValue, false)); }
                if (((result2 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i - 7, b.pieceIdBoard[i].PieceValue, true)); }
                if (((result3 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i - 9, b.pieceIdBoard[i].PieceValue, true)); }
                if (((result7 >> i) & 1) == 1) { allValidMoves.Add(new Move(i, i - 16, b.pieceIdBoard[i].PieceValue, false)); }
            }
            return allValidMoves;
        }

        /******************************************************************
        *    Reverses a long int by converting binary number to string, 
        *    reversing it, then converting back to long int
        ******************************************************************/
        public long Reverse(long num)
        {
            long output = Convert.ToInt64(string.Concat(Convert.ToString(num, 2).PadLeft(64, '0').Reverse()), 2);
            return output;
        }

        /******************************************************************
        *    Generates a bitmap of all vertical and horizontal moves
        *    for sliding pieces
        ******************************************************************/
        public long GetVerticalAndHorizontal(int num, long whitePieces, long blackPieces, Board b)
        {
            long occupied = blackPieces | whitePieces;
            long numBitMap = 1L << num;
            long horizontal = ((blackPieces | whitePieces) - 2 * numBitMap) ^ Reverse(Reverse(occupied) - 2 * Reverse(numBitMap));
            long vertical = (((blackPieces | whitePieces) & b.boardUtils.columnMasks[num % 8]) -
                    (2 * numBitMap)) ^ Reverse(Reverse(occupied & b.boardUtils.columnMasks[num % 8]) - (2 * Reverse(numBitMap)));
            long res = (horizontal & b.boardUtils.rowMasks[num / 8]) | (vertical & b.boardUtils.columnMasks[num % 8]);
            return res;
        }

        /******************************************************************
        *    Generates a bitmap of all diagonal moves for sliding pieces
        ******************************************************************/
        public long GetDiagonals(int num, Board b)
        {
            long occupied = b.BlackPieces | b.WhitePieces;
            long numBitMap = 1L << num;
            long possibilitiesDiagonal = ((occupied & b.boardUtils.diagonalMasks1[(num / 8) + (num % 8)]) - (2 * numBitMap)) ^ Reverse(Reverse(occupied & b.boardUtils.diagonalMasks1[(num / 8) + (num % 8)]) - (2 * Reverse(numBitMap)));
            long possibilitiesAntiDiagonal = ((occupied & b.boardUtils.diagonalMasks2[(num / 8) + 7 - (num % 8)]) - (2 * numBitMap)) ^ Reverse(Reverse(occupied & b.boardUtils.diagonalMasks2[(num / 8) + 7 - (num % 8)]) - (2 * Reverse(numBitMap)));
            return (possibilitiesDiagonal & b.boardUtils.diagonalMasks1[(num / 8) + (num % 8)]) | (possibilitiesAntiDiagonal & b.boardUtils.diagonalMasks2[(num / 8) + 7 - (num % 8)]);
        }

        /******************************************************************
        *    Generates all valid moves to get out of check
        ******************************************************************/
        public List<Move> GetOutOfCheck(Board b)
        {
            List<Move> validMove = new List<Move>();

            int loc = (Convert.ToString(b.BK, 2).Length) - 1;

            List<Move> list = new List<Move>();
            Rules newRules = new Rules();
            bool atLeastOneAttack = false;
            list = newRules.GetBlackRules(b);

            foreach (Move move in list)
            {
                long startMask = 1L << move.To;
                long endMask = 1L << move.From;

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


                b.WhitePieces = b.WP | b.WB | b.WK | b.WN | b.WQ | b.WR;
                b.BlackPieces = b.BP | b.BB | b.BK | b.BN | b.BQ | b.BR;
                b.EmptySpaces = ~(b.WhitePieces | b.BlackPieces);

                List<Move> list2 = new List<Move>();
                Rules newRules2 = new Rules();
                list2 = newRules2.GetWhiteRules(b);

                foreach (Move move2 in list2) { 
                    if (((Convert.ToString(b.BK, 2).Length) - 1) == move2.To) { 
                        atLeastOneAttack = true;
                    }
                }

                if (!atLeastOneAttack) { 
                    validMove.Add(new Move(move.To, move.From, b.pieceIdBoard[move.To].PieceValue, false));
                }
                atLeastOneAttack = false;
            }

            return validMove;
        }
        
    }
}