﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChessGame.Properties;
//
namespace ChessGame
{
    public class Board
    {
        public long Whitepieces { get; set; }
        public long Blackpieces { get; set; }
        public long EmptySpaces { get; set; }
        public long WP, WB, WN, WR, WQ, WK, BP, BB, BN, BR, BQ, BK;
        public BoardUtilities boardUtils;
        public Piece[] pieceIdBoard;
        public PictureBox[] pieceBorder;
        public bool CheckMate { get; set; }     
        public bool FirstMouseClick { get; set; }
        public string[] chessBoard;
        public int whiteInCheck, blackInCheck, startPiecePosition;
        public Move lastMove;
        public const int BoardSize = 64;

        public Board()
        {
            boardUtils = new BoardUtilities();
            CheckMate = false;
            FirstMouseClick = false;
            whiteInCheck = -1;
            blackInCheck = -1;
            pieceIdBoard = new Piece[64];
            pieceBorder = new PictureBox[64];
            lastMove = new Move();
        }

        public Board(Board b)
        {
            boardUtils = new BoardUtilities();
            CheckMate = b.CheckMate;
            FirstMouseClick = b.FirstMouseClick;
            whiteInCheck = b.whiteInCheck;
            blackInCheck = b.blackInCheck;
            pieceIdBoard = new Piece[64];
            pieceBorder = new PictureBox[64];
            lastMove = new Move();
            Whitepieces = b.Whitepieces;
            Blackpieces = b.Blackpieces;
            EmptySpaces = b.EmptySpaces;

            for(int i = 0; i < BoardSize; i++) {
                pieceIdBoard[i] = new Piece();
                pieceIdBoard[i].PieceName = b.pieceIdBoard[i].PieceName;
                pieceIdBoard[i].PiecePosition = b.pieceIdBoard[i].PiecePosition;
                pieceIdBoard[i].IsFirstMove = b.pieceIdBoard[i].IsFirstMove;
                pieceIdBoard[i].PieceValue = b.pieceIdBoard[i].PieceValue;
            }

            WP = b.WP;
            WN = b.WN;
            WB = b.WB;
            WK = b.WK;
            WQ = b.WQ;
            WR = b.WR;
            BP = b.BP;
            BN = b.BN;
            BB = b.BB;
            BK = b.BK;
            BQ = b.BQ;
            BR = b.BR;
        }

        /******************************************************************
        *    Create intiial board representation and set bitboards
        *******************************************************************/
        public void InitiateChess()
        {
            WP = WR = WN = WB = WK = WQ = BP = BR = BN = BB = BK = BQ = 0L;
            chessBoard = new string[]
                                {"r", "n", "b", "q", "k", "b", "n", "r",
                               "p", "p", "p", "p", "p", "p", "p", "p",
                                " ", " ", " ", " ", " ", " ", " ", " ",
                                " ", " ", " ", " ", " ", " ", " ", " ",
                                " ", " ", " ", " ", " ", " ", " ", " ",
                                " ", " ", " ", " ", " ", " ", " ", " ",
                                "P", "P", "P", "P", "P", "P", "P", "P",
                                "R", "N", "B", "Q", "K", "B", "N", "R",
                                };
            ConvertArrayToBitboard();
            UpdateBitboards();
        }

        /******************************************************************
        *   Set individual Int64 bitboards to represent pieces
        *******************************************************************/
        private void ConvertArrayToBitboard()
        {
            string binaryArray;
            for (int i = 0; i < BoardSize; i++)
            {
                binaryArray = "0000000000000000000000000000000000000000000000000000000000000000";
                binaryArray = binaryArray.Substring(i + 1) + "1" + binaryArray.Substring(0, i);
                switch (chessBoard[i])
                {
                    case "P":
                        BP += ConvertStringToBitboard(binaryArray);
                        break;
                    case "N":
                        BN += ConvertStringToBitboard(binaryArray);
                        break;
                    case "B":
                        BB += ConvertStringToBitboard(binaryArray);
                        break;
                    case "R":
                        BR += ConvertStringToBitboard(binaryArray);
                        break;
                    case "K":
                        BK += ConvertStringToBitboard(binaryArray);
                        break;
                    case "Q":
                        BQ += ConvertStringToBitboard(binaryArray);
                        break;
                    case "p":
                        WP += ConvertStringToBitboard(binaryArray);
                        break;
                    case "n":
                        WN += ConvertStringToBitboard(binaryArray);
                        break;
                    case "b":
                        WB += ConvertStringToBitboard(binaryArray);
                        break;
                    case "r":
                        WR += ConvertStringToBitboard(binaryArray);
                        break;
                    case "k":
                        WK += ConvertStringToBitboard(binaryArray);
                        break;
                    case "q":
                        WQ += ConvertStringToBitboard(binaryArray);
                        break;
                }
            }
        }

        /******************************************************************
         *    Converts binary string to Int64
        *******************************************************************/
        public static Int64 ConvertStringToBitboard(String Binary)
        {
            // If this is a positive number
            if (Binary.ElementAt(0) == '0')
            {
                return Convert.ToInt64(Binary, 2);
            }
            else
            {
                return Convert.ToInt64("1" + Binary.Substring(2), 2) * 2;
            }
        }

        /******************************************************************
        *    Update general bitboards to represent current game state
        *******************************************************************/
        private void UpdateBitboards()
        {
            Whitepieces = WP | WB | WK | WN | WQ | WR;
            Blackpieces = BP | BB | BK | BN | BQ | BR;
            EmptySpaces = ~(Whitepieces | Blackpieces);
        }

        /******************************************************************
        *    Creates GUI pieces and sets values
        *******************************************************************/
        private void CreateInterfaceAndSetPieceValues(Form form)
        {
            form.Size = new Size(690, 715);
            form.BackgroundImage = boardUtils.boardImage;
            form.BackgroundImageLayout = ImageLayout.None;

            for (int i = 0; i < BoardSize; i++)
            {
                CreatePiece(i, form);
                switch (chessBoard[i])
                {
                    case "p":
                        SetPieceIdBoardValues(boardUtils.pawnW, "Wp", i, 10);
                        break;
                    case "n":
                        SetPieceIdBoardValues(boardUtils.knightW, "Wn", i, 30);
                        break;
                    case "b":
                        SetPieceIdBoardValues(boardUtils.bishopW, "Wb", i, 30);
                        break;
                    case "r":
                        SetPieceIdBoardValues(boardUtils.rookW, "Wr", i, 50);
                        break;
                    case "k":
                        SetPieceIdBoardValues(boardUtils.kingW, "Wk", i, 900);
                        break;
                    case "q":
                        SetPieceIdBoardValues(boardUtils.queenW, "Wq", i, 90);
                        break;
                    case "P":
                        SetPieceIdBoardValues(boardUtils.pawnB, "BP", i, -10);
                        break;
                    case "N":
                        SetPieceIdBoardValues(boardUtils.knightB, "BN", i, -30);
                        break;
                    case "B":
                        SetPieceIdBoardValues(boardUtils.bishopB, "BB", i, -30);
                        break;
                    case "R":
                        SetPieceIdBoardValues(boardUtils.rookB, "BR", i, -50);
                        break;
                    case "K":
                        SetPieceIdBoardValues(boardUtils.kingB, "BK", i, -900);
                        break;
                    case "Q":
                        SetPieceIdBoardValues(boardUtils.queenB, "BQ", i, -90);
                        break;
                    default:
                        pieceIdBoard[i].PieceName = "-";
                        pieceIdBoard[i].PiecePosition = i;
                        pieceIdBoard[i].BackColor = Color.Transparent;
                        pieceIdBoard[i].PieceValue = 0;
                        break;
                }
                pieceIdBoard[i].SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            }
        }

        /******************************************************************
        *    Creates individual PictureBox pieces for GUI
        *******************************************************************/
        private void CreatePiece(int index, Form form)
        {
            pieceBorder[index] = new PictureBox
            {
                Location = new Point(((index % 8) % 8) * 84, 672 - (84 * (index / 8 + 1))),
                Size = new Size(84, 84),
                BackColor = Color.Transparent
            };
            form.Controls.Add(pieceBorder[index]);
            pieceIdBoard[index] = new Piece
            {
                Location = new Point((index % 8) * 84, 672 - (84 * (index / 8 + 1))),
                Size = new Size(81, 81),
                Padding = new Padding(4)
            };
            pieceIdBoard[index].Click += new EventHandler(PieceClicked);
            form.Controls.Add(pieceIdBoard[index]);
            pieceIdBoard[index].BackgroundImageLayout = ImageLayout.Center;
            pieceIdBoard[index].BringToFront();
            pieceIdBoard[index].BackColor = Color.Transparent;

        }

        /******************************************************************
        *    Event Handler for GUI mouse clicks, main game loop
        *******************************************************************/
        private void PieceClicked(object sender, EventArgs e)
        {           

            Piece piece = sender as Piece;

            //if this is initial piece to move from, highlight piece
            if (!FirstMouseClick)
            {
                pieceBorder[piece.PiecePosition].BackColor = Color.Chocolate;
                pieceIdBoard[piece.PiecePosition].BackColor = Color.Chocolate;
                FirstMouseClick = true;
                startPiecePosition = piece.PiecePosition;
            }
            //else this is piece to move to, 
            else
            {
                List<Move> allLegalMoves = GetAllLegalMoves();
                Int64 pieceMask = 1L << piece.PiecePosition;
                Move newMove = new Move(startPiecePosition, piece.PiecePosition);
                // If invalid move, reset piece color 
                if (!newMove.IsaValidMove(allLegalMoves))
                {
                    SetPieceColor(startPiecePosition, Color.Transparent);
                }
                // Else move is valid
                else
                {
                    Board b = new Board(this);
                    //If this would result in current player's own check, invalid move
                    if (newMove.IsCheckMove(allLegalMoves, startPiecePosition, piece.PiecePosition, b, "W"))
                    {
                        SetPieceColor(startPiecePosition, Color.Transparent);
                    }

                    //else make the move
                    else
                    {
                        UpdateBoard(newMove.To);
                        Board b2 = new Board(this);
                        Int64 fm = boardUtils.whiteFirstMove;
                        int whiteKingPosition = GetPosition(WK);
                        SetPieceColor(whiteKingPosition, Color.Transparent);
                        SetPieceColor(startPiecePosition, Color.Transparent);
                        SetPieceColor(piece.PiecePosition, Color.Transparent);
                        if (newMove.OpponentInCheck(allLegalMoves, startPiecePosition, piece.PiecePosition, ref b2, "W"))
                        {
                            string msg = "Check!";
                            DialogResult dResult;
                            dResult = MessageBox.Show(msg);
                        }
                        bool result = InitiateComputerMove();
                        while (!(result))
                        {
                            result = InitiateComputerMove();
                        }
                        if (CheckMate == true)
                        {
                            string msg = "Checkmate! White wins!!!";
                            DialogResult dResult;
                            dResult = MessageBox.Show(msg);
                            Application.Exit();
                        }
                        FirstMouseClick = false;
                        DrawArray(this);
                    }
                }
                FirstMouseClick = false;
            }
            DrawArray(this);
        }

        /******************************************************************
        *     Sets GUI clicked piece color
        ******************************************************************/
        private void SetPieceColor(int position, Color myColor)
        {
            pieceIdBoard[position].BackColor = myColor;
            pieceBorder[position].BackColor = myColor;
            pieceIdBoard[position].Refresh();
            pieceBorder[position].Refresh();
        }

        /******************************************************************
       *     Sets piece image, name, position, and value
       ******************************************************************/
        private void SetPieceIdBoardValues(Image pImage, string pName, int pIndex, int pValue)
        {
            pieceIdBoard[pIndex].Image = pImage;
            pieceIdBoard[pIndex].PieceName = pName;
            pieceIdBoard[pIndex].PiecePosition = pIndex;
            pieceIdBoard[pIndex].PieceValue = pValue;
        }

        /******************************************************************
       *     Gets position on pieceIdBoard of specified piece
       ******************************************************************/
        private int GetPosition(Int64 pieceMap)
        {
            return (Convert.ToString(pieceMap, 2).Length) - 1;
        }

        /******************************************************************
       *     Draws pieceIdBoard to console, used for debugging purposes
       ******************************************************************/
        public void DrawArray(Board board)
        {
            String[,] consoleBoard = new String[8, 8];
            for (int i = 0; i < 64; i++)
            {
                if (((WP >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "p"; }
                else if (((WN >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "n"; }
                else if (((WB >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "b"; }
                else if (((WR >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "r"; }
                else if (((WQ >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "q"; }
                else if (((WK >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "k"; }
                else if (((BP >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "P"; }
                else if (((BN >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "N"; }
                else if (((BB >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "B"; }
                else if (((BR >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "R"; }
                else if (((BQ >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "Q"; }
                else if (((BK >> i) & 1) == 1) { consoleBoard[i / 8, i % 8] = "K"; }
                else { consoleBoard[i / 8, i % 8] = "-"; }
            }
        }

        /******************************************************************
       *     Initiates computer's turn, implements MinMax algorithm and
       *     alpha-beta pruning to find best possible move(s)
       *     using 64 int bitboards
       ******************************************************************/
        public bool InitiateComputerMove()
        {
            Move bestMove = new Move();
            List<Move> returnMove = new List<Move>();
            Rules computerRules = new Rules();
            List<Move> list = new List<Move>();
            Board b = new Board(this);

            if (blackInCheck > 0)
            {

                list = computerRules.GetOutOfCheck(b);

                if (CheckForCheckMate(list, "B", b))
                {
                    return true;
                }
                if (list.Count == 0)
                {
                    CheckMate = true;
                    return true;
                }
                else
                {
                    foreach (Move m in list)
                    {
                        if (m.To == blackInCheck)
                        {
                            UpdateBlackPiece(m);
                            UpdateBoard(m.To);

                            return true;
                        }
                    }
                    UpdateBlackPiece(list[0]);
                    UpdateBoard(list[0].To);
                }
                return true;
            }

            returnMove = bestMove.EvaluateMoves(3, true, b);

            if (returnMove.Count == 1)
            {
                if (!returnMove[0].IsCheckMove(returnMove, returnMove[0].From, returnMove[0].To, b, "B"))
                {
                    if (returnMove[0].To != lastMove.From || returnMove[0].From != lastMove.To)
                    {
                        UpdateBlackPiece(returnMove[0]);
                        UpdateBoard(returnMove[0].To);
                        return true;
                    }

                }
            }

            for (int i = returnMove.Count - 1; i >= 0; i--)
            {
                if (!returnMove[i].IsCheckMove(returnMove, returnMove[i].From, returnMove[i].To, b, "B"))
                {
                    if (returnMove[i].To != lastMove.From || returnMove[i].From != lastMove.To)
                    {
                        UpdateBlackPiece(returnMove[i]);
                        UpdateBoard(returnMove[i].To);
                        return true;
                    }
                }
            }
            return false;
        }

        /******************************************************************
       *     Checks for ending game move checkmate in relation to 
       *     available legal moves
       ******************************************************************/
        private bool CheckForCheckMate(List<Move> list, string v, Board b)
        {
            if (list.Count == 1)
            {
                if (list[0].IsCheckMove(list, list[0].From, list[0].To, b, v))
                {
                    CheckMate = true;
                    return true;
                }
            }
            else if (list.Count == 0)
            {
                CheckMate = true;
                return true;
            }
            
            return false;
        }

        /******************************************************************
       *     Sets GUI piece color and makes move slowly so that
       *     human player can visually see the move being made by computer
       ******************************************************************/
        private void UpdateBlackPiece(Move newMove)
        {
            lastMove.To = newMove.To;
            lastMove.From = newMove.From;

            SetPieceColor(newMove.From, Color.MediumSeaGreen);
            startPiecePosition = newMove.From;
            System.Threading.Thread.Sleep(300);
            SetPieceColor(newMove.To, Color.MediumSeaGreen);
            System.Threading.Thread.Sleep(300);
            SetPieceColor(newMove.From, Color.Transparent);
            SetPieceColor(newMove.To, Color.Transparent);
            blackInCheck = -1;
        }

        /******************************************************************
       *     Updates bitboards and pieceIDBoard based on player's move,
       *     checks for check
       ******************************************************************/
        internal void UpdateBoard(int piecePosition)
        {
            Int64 startMask = 0L;
            Int64 endMask = 0L;
            startMask = 1L << startPiecePosition;
            endMask = 1L << piecePosition;

            switch (pieceIdBoard[piecePosition].PieceName)
            {
                case "Wp":
                    WP = (WP & ~endMask);
                    break;
                case "Wn":
                    WN = (WN & ~endMask);
                    break;
                case "Wk":
                    WK = (WK & ~endMask);
                    break;
                case "Wb":
                    WB = (WB & ~endMask);
                    break;
                case "Wr":
                    WR = (WR & ~endMask);
                    break;
                case "Wq":
                    WQ = (WQ & ~endMask);
                    break;
                case "BP":
                    BP = (BP & ~endMask);
                    break;
                case "BN":
                    BN = (BN & ~endMask);
                    break;
                case "BK":
                    BK = (BK & ~endMask);
                    break;
                case "BB":
                    BB = (BB & ~endMask);
                    break;
                case "BR":
                    BR = (BR & ~endMask);
                    break;
                case "BQ":
                    BQ = (BQ & ~endMask);
                    break;
                default:
                    break;
            }

            switch (pieceIdBoard[startPiecePosition].PieceName)
            {
                case "Wp":
                    WP = (WP | endMask) ^ (startMask);
                    boardUtils.whiteFirstMove = boardUtils.whiteFirstMove & ~startMask;
                    break;
                case "Wn":
                    WN = (WN | endMask) ^ (startMask);
                    break;
                case "Wk":
                    WK = (WK | endMask) ^ (startMask);
                    break;
                case "Wb":
                    WB = (WB | endMask) ^ (startMask);
                    break;
                case "Wr":
                    WR = (WR | endMask) ^ (startMask);
                    break;
                case "Wq":
                    WQ = (WQ | endMask) ^ (startMask);
                    break;
                case "BP":
                    BP = (BP | endMask) ^ (startMask);
                    boardUtils.blackFirstMove = boardUtils.blackFirstMove & ~startMask;
                    break;
                case "BN":
                    BN = (BN | endMask) ^ (startMask);
                    break;
                case "BK":
                    BK = (BK | endMask) ^ (startMask);
                    break;
                case "BB":
                    BB = (BB | endMask) ^ (startMask);
                    break;
                case "BR":
                    BR = (BR | endMask) ^ (startMask);
                    break;
                case "BQ":
                    BQ = (BQ | endMask) ^ (startMask);
                    break;
                default:
                    break;
            }

            UpdatePiece(piecePosition);
            UpdateBitboards();
            ResultsInCheck();
        }

        /******************************************************************
        *     Updates pieceIdBoard and GUI
        ******************************************************************/
        private void UpdatePiece(int piecePosition)
        {
            FirstMouseClick = false;
            pieceIdBoard[piecePosition].Image = pieceIdBoard[startPiecePosition].Image;
            pieceIdBoard[startPiecePosition].Image = null;
            pieceIdBoard[piecePosition].PieceName = pieceIdBoard[startPiecePosition].PieceName;
            pieceIdBoard[startPiecePosition].PieceName = " ";
            pieceIdBoard[piecePosition].PieceValue = pieceIdBoard[startPiecePosition].PieceValue;
            pieceIdBoard[startPiecePosition].PieceValue = 0;
            if (pieceIdBoard[startPiecePosition].IsFirstMove == true)
            {
                pieceIdBoard[startPiecePosition].IsFirstMove = false;
            }
        }

        /******************************************************************
      *     Checks if either player is in check
      ******************************************************************/
        private bool ResultsInCheck()
        {
            // Location of white king
            int loc = (Convert.ToString(WK, 2).Length) - 1;
            Rules checkRules = new Rules();
            List<Move> list = new List<Move>();
            Board b = new Board(this);
            // Get list of valid black moves
            list = checkRules.GetComputerRules(b);

            foreach (Move move in list)
            {
                if (move.To == loc)
                {
                    string msg = "Player in check!";
                    pieceIdBoard[loc].BackColor = Color.Red;
                    pieceIdBoard[loc].Refresh();
                    DialogResult result;
                    whiteInCheck = move.From;
                    result = MessageBox.Show(msg);
                    return true;
                }
            }

            int loc2 = (Convert.ToString(BK, 2).Length) - 1;
            Rules checkRules2 = new Rules();
            List<Move> list2 = new List<Move>();
            list2 = checkRules2.GetRules(b);

            foreach (Move move in list2)
            {
                if (move.To == loc2)
                {
                    string msg = "Player in check!";
                    pieceIdBoard[loc2].BackColor = Color.Red;
                    pieceIdBoard[loc2].Refresh();
                    DialogResult result;
                    blackInCheck = move.From;
                    result = MessageBox.Show(msg);
                    return true;
                }
            }
            pieceIdBoard[loc2].BackColor = Color.Transparent;
            pieceIdBoard[loc].Refresh();
            return false;
        }

        /******************************************************************
        *     Gets all legal moves for human player
        ******************************************************************/
        public List<Move> GetAllLegalMoves()
        {
            //UpdateBitboards();
            Rules chessRules = new Rules();
            Board p2 = new Board(this);

            List<Move> results = chessRules.GetRules(p2);
            return results;

        }
    }
}
