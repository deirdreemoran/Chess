using ChessGame.Properties;
using System;
using System.Drawing;

namespace ChessGame
{
    public class BoardUtilities
    {
        public Bitmap boardImage = Resources.board;
        public readonly Image pawnW = Resources.WP;
        public readonly Image pawnB = Resources.BP;
        public readonly Image rookB = Resources.BR;
        public readonly Image rookW = Resources.WR;
        public readonly Image knightW = Resources.WN;
        public readonly Image knightB = Resources.BN;
        public readonly Image bishopB = Resources.BB;
        public readonly Image bishopW = Resources.WB;
        public readonly Image queenW = Resources.WQ;
        public readonly Image queenB = Resources.BQ;
        public readonly Image kingW = Resources.WK;
        public readonly Image kingB = Resources.BK;
        public Int64[] columnMasks = { ~(-72340172838076674L), ~(-144680345676153347L),
                                        ~(-289360691352306693L), ~(-578721382704613385L), ~(-1157442765409226769L),
                                        ~(-2314885530818453537L), ~(-4629771061636907073L), ~(9187201950435737471L) };
        public Int64[] diagonalMasks1 = { 1L, 258L, 66052L, 16909320L, 4328785936L, 1108169199648L,
                                                283691315109952L, 72624976668147840L, 145249953336295424L, 290499906672525312L,
                                                580999813328273408L, 1161999622361579520L, 2323998145211531264L, 4647714815446351872L, -9223372036854775808L };
        public Int64[] diagonalMasks2 = { 128L, 32832L, 8405024L, 2151686160L, 550831656968L, 141012904183812L, 36099303471055874L, -9205322385119247871L, 4620710844295151872L, 2310355422147575808L, 1155177711073755136L, 577588855528488960L, 288794425616760832L, 144396663052566528L, 72057594037927936L };
        public Int64[] rowMasks = { 255L, 65280L, 16711680L, 4278190080L, 1095216660480L,
                                            280375465082880L, 71776119061217280L, -72057594037927936L};

        public Int64 emptyColumnZeroToOne = -72340172838076674L & -144680345676153347L;
        public Int64 emptyColumnSixToSeven = -4629771061636907073L & 9187201950435737471L;
        public Int64 emptyColumnFiveToSeven = (-4629771061636907073L & 9187201950435737471L) & -2314885530818453537L;
        public Int64 emptySpaces = -1L;
        public Int64 notColumnSeven = 9187201950435737471;
        public Int64 notColumnZero = -72340172838076674L;
        public Int64 whiteFirstMove = 65280L;
        public Int64 blackFirstMove = 71776119061217280L;
    }
}