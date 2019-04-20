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

       
        //RANDOM NUMBER GENERATOR
        public Int64 Random64()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[64];
            provider.GetBytes(byteArray);
            Int64 randomInt64 = BitConverter.ToInt64(byteArray, 0);
            Console.WriteLine(randomInt64);
            return randomInt64;


        }
    }

}