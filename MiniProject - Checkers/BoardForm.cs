using MiniProject___Checkers.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MiniProject___Checkers
{
    public partial class BoardForm : Form
    {
        Checkers newCheckers = new Checkers();

        
        public BoardForm()
        {
            InitializeComponent();
        }

        //Visual Representation of the board
        Button[,] buttons = new Button[8, 8];
        void SetUp()
        {
            int space = 60;
            for (int x = 0; x < newCheckers.RowSize; x++)
            {
                for (int y = 0; y < newCheckers.ColumnSize; y++)
                {
                    Button button = new Button();
                    button.Size = new Size(60, 60);
                    button.Location = new Point( (y + 1) * space, (x + 1) * space);

                    buttons[x, y] = button;
                    this.Controls.Add(button);
                }
            }
        }

        //Repaints and updates the board based on positions
        void CurrentBoard()
        {
            for (int x = 0; x < newCheckers.RowSize; x++)
            {
                for (int y = 0; y < newCheckers.ColumnSize; y++)
                {
                    Button button = buttons[x, y];

                    switch (newCheckers.CheckerBoard[$"{x} {y}"]) { 
                        case 'B':
                            button.BackColor = Color.Gray;
                            button.Tag = null;
                            button.Image = null;
                            break;
                        case 'x':
                            button.BackColor = Color.Gray;
                            button.Image = new Bitmap(Properties.Resources.Black_Piece, new Size(30, 30));
                            button.Tag = 'x';
                            break;
                        case 'y':
                            button.BackColor = Color.Gray;
                            button.Image = new Bitmap(Properties.Resources.Red_Piece, new Size(30, 30));
                            button.Tag = 'y';
                            break;
                        case 'X':
                            button.BackColor = Color.Gray;
                            button.Image = new Bitmap(Properties.Resources.Black_King_Piece, new Size(30, 30));
                            button.Tag = 'X';
                            break;
                        case 'Y':
                            button.BackColor = Color.Gray;
                            button.Image = new Bitmap(Properties.Resources.Red_King_Piece, new Size(30, 30));
                            button.Tag = 'Y';
                            break;
                        case 'W':
                            button.BackColor = Color.White;
                            button.Tag = null;
                            button.Image = null;
                            break;
                    }
                    button.Invalidate();
                }
               

            }
        }
        

        //Checks for button click to make a decision
        private Button[] selected = new Button[2];
        private int counter = 0;
        private void AnyButton_Click(object sender, EventArgs e)
        {
            
            Button clicked = (Button)sender;

            int col = clicked.Location.X / 60 - 1;
            int row = clicked.Location.Y / 60 - 1;
            

            if (clicked.BackColor == Color.Gray) 
            {
                if (counter == 0 && clicked.Image != null)
                {
                        if (newCheckers.Turn == 'x' && clicked.Tag.ToString() == "X" 
                            || newCheckers.Turn == 'y' && clicked.Tag.ToString() == "Y"
                            || clicked.Tag.ToString() == newCheckers.Turn.ToString())
                        {
                            selected[counter] = clicked;
                            txtGridInfo.Text = $"{newCheckers.PlayerMoved} Piece: {row + 1}-{col + 1}";
                            newCheckers.ResetPosition();

                            newCheckers.Insert((row, col), newCheckers.PrePosition);
                            counter++;
                            
                        }
                  
                }
                else if (counter == 1)
                {
                    selected[1] = clicked;

                    int prevCol = selected[0].Location.X / 60 - 1, prevRow = selected[0].Location.Y / 60 - 1;
                    newCheckers.Insert((row, col), newCheckers.NewPosition);


                    if (newCheckers.PlaceChip())
                    {
                        selected[1].Image = selected[0].Image;
                        selected[1].Tag = selected[0].Tag;

                        selected[0].Image = null;
                        selected[0].Tag = null;

                        txtGridInfo.Text = $"{newCheckers.PlayerMoved} moved {prevRow + 1}-{prevCol + 1} : {row + 1}-{col + 1}";

                        newCheckers.SwitchTurn();
                        
                    }
                    else
                    {
                        txtGridInfo.Text = $"{row}-{col}: is not a valid choice";
                        
                    }

                    selected[0].BackColor = Color.Gray;
                    counter = 0;
                    selected = new Button[2];

                }  
            }
            else
            {
                counter = 0;
                txtGridInfo.Text = $"That is not a playable area!";
            }

            CurrentBoard();
            GameProgress();
            ShowTurn();
            

        }

        private void ShowTurn()
        {
            if (newCheckers.Turn == 'x') pBoxTurn.Image = new Bitmap(Properties.Resources.Black_Piece, new Size(60, 60));
            else pBoxTurn.Image = new Bitmap(Properties.Resources.Red_Piece, new Size(60, 60));
        }

        private void BoardForm_Load(object sender, EventArgs e)
        {
            SetUp();
            CurrentBoard();

            if (Data.IsSinglePlayer) SinglePlayer();
            else Multiplayer();

            ShowTurn();

            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    button.Click += AnyButton_Click;
                }
            }
        }

        private void SinglePlayer()
        {
            
        }

        private void Multiplayer()
        {
            MessageBox.Show("Multiplayer Player Game", "Game Starting");
        }

        private void GameProgress()
        {
            if (!newCheckers.IsLoop)
            {
                MessageBox.Show(newCheckers.HasWon);

                MainMenuForm form1 = new MainMenuForm();

                form1.Show();
                this.Close();
                return;

            }

            if (newCheckers.IsDraw())
            {
                MessageBox.Show("It's a draw!");
                MainMenuForm form1 = new MainMenuForm();

                form1.Show();
                this.Close();
                return;
            }
        }

    }
}
