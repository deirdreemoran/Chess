using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChessGame
{
        public partial class Form1 : Form
        {   
                Board myBoard = new Board();
                public Form1()
                {
                        InitializeComponent();

                }

                private void Form1_Load(object sender, EventArgs e)
                {
                        myBoard.InitiateChess();
                        myBoard.CreateInterfaceAndSetPieceValues(this);
                }
        }
}
