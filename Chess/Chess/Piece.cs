using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ChessGame
{
    public class Piece : PictureBox
    {
        public string PieceName { get; set; }
        public int PiecePosition { get; set; }
        public bool IsFirstMove { get; set; }
        public int PieceValue { get; set; }

        public Piece()
        {
            PieceName = "-";
            PiecePosition = 0;
            IsFirstMove = true;
            PieceValue = 0;
        }
    }
}