using System;
using System.Collections.Generic;
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

        public bool IsInXRange((int row, int col) position, (int newRow, int newCol) newPosition)
        {
            if (CheckerBoard[$"{newPosition.newRow} {newPosition.newCol}"] != 'B')
            {
                return false;
            }

            int rowDifference = newPosition.newRow - position.row;
            int colDifference = Math.Abs(position.col - newPosition.newCol);

            switch (CheckerBoard[$"{position.row} {position.col}"]) 
            {
                case 'x':
                    if (rowDifference == 1 && colDifference == 1) return true;
                    if (rowDifference == 2 && colDifference == 2)
                    {
                        int midRow = (position.row + newPosition.newRow) / 2;
                        int midCol = (position.col + newPosition.newCol) / 2;
                        char jumpped = CheckerBoard[$"{midRow} {midCol}"];

                        if(jumpped == _pieces[2] || jumpped == _pieces[3])
                        {
                            JumpedCoords[0, 0] = midRow;
                            JumpedCoords[0, 1] = midCol;

                            CheckerBoard[$"{midRow} {midCol}"] = 'B';

                            isJumped = true;
                            return true;
                        }
                    }

                    break;
                case 'X':
                    int positiveRowDifference = Math.Abs(rowDifference);

                    if (positiveRowDifference == 1 && colDifference == 1) return true;
                    if (positiveRowDifference == 2 && colDifference == 2)
                    {
                        int midRow = (position.row + newPosition.newRow) / 2;
                        int midCol = (position.col + newPosition.newCol) / 2;
                        char jumped = CheckerBoard[$"{midRow} {midCol}"];

                        if(jumped == _pieces[2] ||  jumped == _pieces[3])
                        {
                            JumpedCoords[0, 0] = midRow;
                            JumpedCoords[0, 1] = midCol;

                            CheckerBoard[$"{midRow} {midCol}"] = 'B';

                            isJumped = true;
                            return true;
                        }
                    }
                    
                    break;
            
            }

            return false;
        }

        public bool IsInYRange((int row, int col) position, (int newRow, int newCol) newPosition)
        {
            if (CheckerBoard[$"{newPosition.newRow} {newPosition.newCol}"] != 'B')
            {
                return false;
            }

            int rowDifference = newPosition.newRow - position.row;
            int colDifference = Math.Abs(position.col - newPosition.newCol);

            switch (CheckerBoard[$"{position.row} {position.col}"])
            {
                case 'y':
                    if (rowDifference == - 1 && colDifference == 1) return true;
                    if (rowDifference == - 2 && colDifference == 2)
                    {
                        int midRow = (position.row + newPosition.newRow) / 2;
                        int midCol = (position.col + newPosition.newCol) / 2;
                        char jumpped = CheckerBoard[$"{midRow} {midCol}"];

                        if (jumpped == _pieces[0] || jumpped == _pieces[1])
                        {
                            JumpedCoords[0, 0] = midRow;
                            JumpedCoords[0, 1] = midCol;

                            CheckerBoard[$"{midRow} {midCol}"] = 'B';

                            isJumped = true;
                            return true;
                        }
                    }

                    break;
                case 'Y':
                    int positiveRowDifference = Math.Abs(rowDifference);

                    if (positiveRowDifference == 1 && colDifference == 1) return true;
                    if (positiveRowDifference == 2 && colDifference == 2)
                    {
                        int midRow = (position.row + newPosition.newRow) / 2;
                        int midCol = (position.col + newPosition.newCol) / 2;
                        char jumped = CheckerBoard[$"{midRow} {midCol}"];

                        if (jumped == _pieces[0] || jumped == _pieces[1])
                        {
                            JumpedCoords[0, 0] = midRow;
                            JumpedCoords[0, 1] = midCol;

                            CheckerBoard[$"{midRow} {midCol}"] = 'B';

                            isJumped = true;
                            return true;
                        }
                    }

                    break;

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
            switch (Turn) 
            {
                case 'x':
                    return Change(IsInXRange);
                case 'y':
                    return Change(IsInYRange);
            }


            return false;
        }

        public bool PlaceChip()
        {
            
            if (IsInBounds())
            {
                CheckerBoard[$"{_referencePosition[1, 0]} {_referencePosition[1, 1]}"] = CheckerBoard[$"{_referencePosition[0, 0]} {_referencePosition[0, 1]}"];
                CheckerBoard[$"{_referencePosition[0, 0]} {_referencePosition[0, 1]}"] = 'B';
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

        private bool CanBlackMove()
        {
            foreach ((string s, char ch) in CheckerBoard)
            {
                int row = int.Parse(s[0].ToString());
                int col = int.Parse(s[2].ToString());
                int positive = 1, negative = -1;
                int positiveJump = 2, negativeJump = -2;

                int leftCol = col + negative;
                int rightCol = col + positive;
                int prevRow = row - 1;
                int nextRow = row + 1;
                int backJump = row - 2;
                int jump = row + 2;
                switch (ch) 
                {
                    case 'x':
                        if(nextRow <= 7)
                        {
                            if(leftCol >= 0)
                            {
                                if (CheckerBoard[$"{nextRow} {leftCol}"] == 'B') return true;
                            }

                            if(rightCol <= 7)
                            {
                                if (CheckerBoard[$"{nextRow} {rightCol}"] == 'B') return true;
                            }
                        }

                        if(jump <= 7)
                        {

                            if(leftCol - 1 >= 0)
                            {
                                char jumpped = CheckerBoard[$"{nextRow} {leftCol}"];

                                if(jumpped == _pieces[2] ||  jumpped == _pieces[3])
                                {
                                    if (CheckerBoard[$"{jump} {leftCol - 1}"] == 'B') return true;
                                }
                            }

                            if(rightCol + 1 <= 7)
                            {
                                char jumpped = CheckerBoard[$"{nextRow} {rightCol}"];

                                if (jumpped == _pieces[2] || jumpped == _pieces[3])
                                {
                                    if (CheckerBoard[$"{jump} {rightCol + 1}"] == 'B') return true;
                                }
                            }
                        }
                        
                    break;

                    case 'X':
                        //Down
                        if (nextRow <= 7)
                        {
                            if (leftCol >= 0)
                            {
                                if (CheckerBoard[$"{nextRow} {leftCol}"] == 'B') return true;
                            }

                            if (rightCol <= 7)
                            {
                                if (CheckerBoard[$"{nextRow} {rightCol}"] == 'B') return true;
                            }
                        }

                        //Up
                        if(prevRow >= 0)
                        {
                            if (leftCol >= 0)
                            {
                                if (CheckerBoard[$"{prevRow} {leftCol}"] == 'B') return true;
                            }

                            if (rightCol <= 7)
                            {
                                if (CheckerBoard[$"{prevRow} {rightCol}"] == 'B') return true;
                            }
                        }

                        //Down Jump
                        if (jump <= 7)
                        {
                            
                            if (leftCol - 1 >= 0)
                            {
                                char jumpped = CheckerBoard[$"{nextRow} {leftCol}"];

                                if (jumpped == _pieces[2] || jumpped == _pieces[3])
                                {
                                    if (CheckerBoard[$"{jump} {leftCol - 1}"] == 'B') return true;
                                }
                            }

                            if (rightCol + 1 <= 7)
                            {
                                char jumpped = CheckerBoard[$"{nextRow} {rightCol}"];

                                if (jumpped == _pieces[2] || jumpped == _pieces[3])
                                {
                                    if (CheckerBoard[$"{jump} {rightCol + 1}"] == 'B') return true;
                                }
                            }
                        }

                        //Up Jump
                        if (backJump >= 0)
                        {
                            
                            if(leftCol - 1 >= 0)
                            {
                                char jumpped = CheckerBoard[$"{prevRow} {leftCol}"];

                                if (jumpped == _pieces[2] || jumpped == _pieces[3])
                                {
                                    if (CheckerBoard[$"{backJump} {leftCol - 1}"] == 'B') return true;
                                }
                            }

                            if(rightCol + 1 <= 7)
                            {
                                char jumpped = CheckerBoard[$"{prevRow} {rightCol}"];

                                if(jumpped == _pieces[2] || jumpped == _pieces[3])
                                {
                                    if (CheckerBoard[$"{backJump} {rightCol + 1}"] == 'B') return true;
                                }
                            }

                        }
                    break;
                }

            }
            return false;
        }

        private bool CanRedMove()
        {
            foreach ((string s, char ch) in CheckerBoard)
            {
                int row = int.Parse(s[0].ToString());
                int col = int.Parse(s[2].ToString());

                int leftCol = col + - 1;
                int rightCol = col + 1;
                int prevRow = row - 1;
                int nextRow = row + 1;
                int backJump = row - 2;
                int jump = row + 2;
                switch (ch)
                {
                    case 'y':
                        if (prevRow >= 0)
                        {
                            if (leftCol >= 0)
                            {
                                if (CheckerBoard[$"{prevRow} {leftCol}"] == 'B') return true;
                            }

                            if (rightCol <= 7)
                            {
                                if (CheckerBoard[$"{prevRow} {rightCol}"] == 'B') return true;
                            }
                        }

                        if (backJump >= 0)
                        {

                            if (leftCol - 1 >= 0)
                            {
                                char jumpped = CheckerBoard[$"{prevRow} {leftCol}"];

                                if (jumpped == _pieces[0] || jumpped == _pieces[1])
                                {
                                    if (CheckerBoard[$"{backJump} {leftCol - 1}"] == 'B') return true;
                                }
                            }

                            if (rightCol + 1 <= 7)
                            {
                                char jumpped = CheckerBoard[$"{prevRow} {rightCol}"];

                                if (jumpped == _pieces[0] || jumpped == _pieces[1])
                                {
                                    if (CheckerBoard[$"{backJump} {rightCol + 1}"] == 'B') return true;
                                }
                            }
                        }
                        
                        break;

                    case 'Y':
                        //Down
                        if (nextRow <= 7)
                        {
                            if (leftCol >= 0)
                            {
                                if (CheckerBoard[$"{nextRow} {leftCol}"] == 'B') return true;
                            }

                            if (rightCol <= 7)
                            {
                                if (CheckerBoard[$"{nextRow} {rightCol}"] == 'B') return true;
                            }
                        }

                        //Up
                        if (prevRow >= 0)
                        {
                            if (leftCol >= 0)
                            {
                                if (CheckerBoard[$"{prevRow} {leftCol}"] == 'B') return true;
                            }

                            if (rightCol <= 7)
                            {
                                if (CheckerBoard[$"{prevRow} {rightCol}"] == 'B') return true;
                            }
                        }

                        //Down Jump
                        if (jump <= 7)
                        {

                            if (leftCol - 1 >= 0)
                            {
                                char jumpped = CheckerBoard[$"{nextRow} {leftCol}"];

                                if (jumpped == _pieces[0] || jumpped == _pieces[1])
                                {
                                    if (CheckerBoard[$"{jump} {leftCol - 1}"] == 'B') return true;
                                }
                            }

                            if (rightCol + 1 <= 7)
                            {
                                char jumpped = CheckerBoard[$"{nextRow} {rightCol}"];

                                if (jumpped == _pieces[0] || jumpped == _pieces[1])
                                {
                                    if (CheckerBoard[$"{jump} {rightCol + 1}"] == 'B') return true;
                                }
                            }
                        }

                        //Up Jump
                        if (backJump >= 0)
                        {

                            if (leftCol - 1 >= 0)
                            {
                                char jumpped = CheckerBoard[$"{prevRow} {leftCol}"];

                                if (jumpped == _pieces[0] || jumpped == _pieces[1])
                                {
                                    if (CheckerBoard[$"{backJump} {leftCol - 1}"] == 'B') return true;
                                }
                            }

                            if (rightCol + 1 <= 7)
                            {
                                char jumpped = CheckerBoard[$"{prevRow} {rightCol}"];

                                if (jumpped == _pieces[0] || jumpped == _pieces[1])
                                {
                                    if (CheckerBoard[$"{backJump} {rightCol + 1}"] == 'B') return true;
                                }
                            }

                        }
                        break;
                }

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
