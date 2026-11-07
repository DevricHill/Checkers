using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MiniProject___Checkers
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            rtxtInstruction.Text =
                "First player is chosen at random." +
                "\nEach turn: one move only." +
                "\nStep: one square diagonally forward to an empty square." +
                "\nJump: over one adjacent enemy piece to the empty square beyond. Remove that piece." +
                "\nNo double jumps. One jump ends your turn." +
                "\nKings: reach the far row to become a king (this ends your turn). " +
                "\nKings step or jump forward or backward, still one square or one jump at a time." +
                "\nWin: capture all of your opponent's pieces." +
                "\nNo legal moves: your turn is skipped." +
                "\nDraw: neither player can move.";
        }
    }
}
