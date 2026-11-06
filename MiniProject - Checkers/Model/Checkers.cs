using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace MiniProject___Checkers.Model
{
    public delegate void SelectPosition((int r, int c) position);
    public delegate bool ModifyBoard((int row, int col) position, (int newRow, int newCol) newPosition);
    internal class Checkers
    {
        //Basic Board Info
        public int RowSize { get; } = 8;
        public int ColumnSize { get; } = 8;
        public char Turn { get; private set; }


        //Reference Piece Actions
        private int[,] _referencePosition;
        public int[,] JumpedCoords;
        private bool isJumped;
        public bool IsJumped { get { return isJumped; } }

        private string _playerMoved;
        public string PlayerMoved { get { return _playerMoved; } }

        //Game Play Loop Conditions
        public bool IsLoop { get; private set; }
        private string _hasWon;
        public string HasWon { get { return _hasWon; } }


        //Board Items
        public Dictionary<string, char> CheckerBoard;
        private char[] _pieces = { 'x', 'X', 'y', 'Y'};


        public Checkers()
        {
            CheckerBoard = new Dictionary<string, char>();
            CreateBoard(RowSize, ColumnSize);
            isJumped = false;
            IsLoop = true;
            Turn = SetRandomTurn();

            ResetPosition();
            _playerMoved = Turn == _pieces[0] ? "Black" : "Red";
            JumpedCoords = new int[1,2];
        }

        public void SwitchTurn()
        {
            isJumped = false;

            if (Turn == _pieces[0]) Turn = _pieces[2];
            else Turn = _pieces[0];

            _playerMoved = Turn == _pieces[0] ? "Black" : "Red";
        }

        private char SetRandomTurn()
        {
            Random random = new Random();

            return random.Next(2) == 0 ? 'x' : 'y';
        }

        public void CreateBoard(int row, int column)
        {

            for (int x = 0; x < row; x++)
            {
                for (int y = 0; y < column; y++)
                {
                    if ((x + y) % 2 != 0) { CheckerBoard.Add($"{x} {y}", 'W'); }
                    else
                    {
                        if (x < 3) { CheckerBoard.Add($"{x} {y}", _pieces[0]); }
                        else if (x > 4) { CheckerBoard.Add($"{x} {y}", _pieces[2]); }
                        else { CheckerBoard.Add($"{x} {y}", 'B'); }
                    }
                    
                    
                }
            }
        }

        public char PiecePosition(int row, int column)
        {
            return CheckerBoard[$"{row} {column}"];
        }

        public bool IsRed(char piece)
        {
            return piece == _pieces[2] || piece == _pieces[3];
        }

        public bool IsBlack(char piece)
        {
            return piece == _pieces[0] || piece == _pieces[1];
        }

        private int BlackMove = 1;
        private int RedMove = -1;

        public bool IsValidMove((int x, int y) current, (int x, int y) spot)
        {
            char piece = PiecePosition(current.x, current.y);

            if(PiecePosition(spot.x, spot.y) != 'B') return false;
            if(Turn == 'x' ? !IsBlack(piece) : !IsRed(piece)) return false;

            int rowGap = spot.x - current.x;
            int colGap = Math.Abs(spot.y - current.y);

            int forward = IsBlack(piece) ? BlackMove : RedMove;
            bool isKing = piece == _pieces[1] || piece == _pieces[3];

            if(rowGap != forward && rowGap != 2 * forward && !isKing) return false;
            if (Math.Abs(rowGap) == 1 && colGap == 1) return true;

            if(Math.Abs(rowGap) == 2 && colGap == 2)
            {
                int midRow = (current.x + spot.x) / 2;
                int midCol = (current.y + spot.y) / 2;
                char middlePiece = PiecePosition(midRow, midCol);

                bool isOpposite = IsBlack(piece) ? IsRed(middlePiece) : IsBlack(middlePiece);
                if (isOpposite)
                {
                    JumpedCoords[0, 0] = midRow;
                    JumpedCoords[0, 1] = midCol;
                    isJumped = true;
                    return true;
                }

            }
            return false;
        }

        private void AtEnd((int row, int col) position)
        {
            if (Turn == _pieces[2])
            {
                if (position.row == 0 && CheckerBoard[$"{position.row} {position.col}"] == _pieces[2])
                {
                    CheckerBoard[$"{position.row} {position.col}"] = _pieces[3];
                }
            }

            if (Turn == _pieces[0])
            {
                if (position.row == 7 && CheckerBoard[$"{position.row} {position.col}"] == _pieces[0])
                {
                    CheckerBoard[$"{position.row} {position.col}"] = _pieces[1];
                }
            }
        }

        private bool IsWon()
        {
            if (!CheckerBoard.ContainsValue(_pieces[0]) && !CheckerBoard.ContainsValue(_pieces[1]))
            {
                _hasWon = $"Player Red has won!";
                return true;
            }

            if (!CheckerBoard.ContainsValue(_pieces[2]) && !CheckerBoard.ContainsValue(_pieces[3]))
            {
                _hasWon = $"Player Black has won!";
                return true;
            }

            return false;
        }

        public bool IsInBounds()
        {
            return Change(IsValidMove);
        }

        public bool PlaceChip()
        {
            
            if (IsInBounds())
            {
                CheckerBoard[$"{_referencePosition[1, 0]} {_referencePosition[1, 1]}"] = CheckerBoard[$"{_referencePosition[0, 0]} {_referencePosition[0, 1]}"];
                CheckerBoard[$"{_referencePosition[0, 0]} {_referencePosition[0, 1]}"] = 'B';
                

                if(isJumped) CheckerBoard[$"{JumpedCoords[0, 0]} {JumpedCoords[0, 1]}"] = 'B';

                AtEnd((_referencePosition[1, 0], _referencePosition[1, 1]));

                if (IsWon() && IsLoop)
                {
                    IsLoop = false;
                }

                return true;
            }

            return false;
        }

        private int positionCount = 0;


        public void PrePosition((int row, int col) position)
        {
            _referencePosition[0, 0] = position.row;
            _referencePosition[0, 1] = position.col;
            positionCount++;
        }

        public void NewPosition((int row, int col) position)
        {
            _referencePosition[1, 0] = position.row;
            _referencePosition[1, 1] = position.col;
        }

        private bool IsOpposite(char piece, char other)
        {
            return IsBlack(piece) ? IsRed(other) : IsBlack(other);
        }

        private bool CanBlackMove() { return CanMove(true); }
        private bool CanRedMove() { return CanMove(false); }

        private bool CanMove(bool checkBlack)
        {
            foreach ((string s, char p) in CheckerBoard)
            {
                bool isMine = checkBlack ? IsBlack(p) : IsRed(p);
                if (!isMine) continue;

                int row = int.Parse(s[0].ToString());
                int col = int.Parse(s[2].ToString());

                int forward = IsBlack(p) ? BlackMove : RedMove;
                bool isKing = p == _pieces[1] || p == _pieces[3];

                if (CanMoveInDirection(p, row, col, forward)) return true;
                if(isKing && CanMoveInDirection(p, row, col, -forward)) return true;

            }
            return false;
        }

        private bool CanMoveInDirection(char piece, int row, int col, int direction)
        {
            int nextRow = row + direction;
            int jumpRow = row + 2 * direction;
            int leftCol = col - 1;
            int rightCol = col + 1;

            if(nextRow >= 0 && nextRow <= 7)
            {
                if(leftCol >= 0 && PiecePosition(nextRow, leftCol) == 'B') return true;
                if (rightCol <= 7 && PiecePosition(nextRow, rightCol) == 'B') return true;
            }

            if(jumpRow >= 0 && jumpRow <= 7)
            {
                if (leftCol - 1 >= 0
                    && IsOpposite(piece, PiecePosition(nextRow, leftCol))
                    && PiecePosition(jumpRow, leftCol + 1) == 'B') return true;

                if (rightCol + 1 <= 7
                    && IsOpposite(piece, PiecePosition(nextRow, rightCol))
                    && PiecePosition(jumpRow, rightCol + 1) == 'B') return true;
            }

            return false;
        }

        
        public bool IsDraw()
        {
            if (!CanBlackMove() && !CanRedMove()) return true;
            if (!CanBlackMove() && Turn == _pieces[0]) SwitchTurn();
            if (!CanRedMove() && Turn == _pieces[2]) SwitchTurn();
            return false;
        }

        public void ResetPosition()
        {
            positionCount = 0;
            _referencePosition = new int[2, 2];
        }

        public bool Change(ModifyBoard board)
        {
            return board((_referencePosition[0, 0], _referencePosition[0, 1]), (_referencePosition[1,0], _referencePosition[1,1]));
        }

        public void Insert((int row, int col) position, SelectPosition select) {
            select(position);
        }

    }
}
