using Connect4;
using System;
using System.Collections.Generic;

namespace Connect4
{
    class Player
    {
        private String name;
        private char counterSymbol;

        public Player(string name, char counterSymbol)
        {
            this.Name = name;
            this.CounterSymbol = counterSymbol;
        }

        // Returns player name as a string.
        public override string ToString()
        {
            return this.Name;
        }

        internal string Name { get; set; }
        internal char CounterSymbol { get; set; }
    }
    class Counter
    {
        private Player player;

        public Counter(Player player)
        {
            this.Player = player;
        }

        internal Player Player { get => player; set => player = value; }

        // Returns player's counter char as a string. 
        public override string ToString()
        {
            return this.Player.CounterSymbol + "";
        }

        // 'Define a boolean equals method in your Counter class that returns true if, when it is passed 
        // another Object, the other object is a Counter from the same Player.'
        public override bool Equals(object obj)
        {
            var objectType = obj.GetType();
            Counter objAsCounter = (Counter)obj;
            if (objectType.IsInstanceOfType(this.GetType()) && objAsCounter.Player == this.Player)
            {
                return true;
            }
            return false;
        }

    }

    /*
     * 'Create a class to represent a column. It should have a numRows attribute that is set by the 
     * constructor. It should have an array with numRows elements that can store Counter objects. 
     * Note that the top of the column in the game will be represented by position 0 in your array, and 
     * the bottom (i.e. the position the first counter would fall into) will be position numRows-1.'
     */
    class Column
    {
        private int numRows;
        private Counter[] rows;

        internal Counter[] Rows { get => rows; set => rows = value; }

        public Column(int numRows)
        {
            this.numRows = numRows;
            Rows = new Counter[numRows];
        }
        // 'Your class should have a boolean method called isFull that returns true if the
        // column is full'
        public bool IsFull()
        {
            if (Rows[0] == null) { return false; }
            return true;
        }

        // 'Your class should have an add method that takes as its arguments a Counter object and returns a 
        // boolean(true or false). If the column is not full, the Counter should be added to the correct 
        // position and the method should return true. If the column is full, the method should return 
        // false'.
        public bool Add(Counter counter)
        {
            if (!this.IsFull())
            {
                for (int rowNum = this.numRows - 1; rowNum > -1; rowNum--)
                {
                    if (this.Rows[rowNum] is null)
                    {
                        this.Rows[rowNum] = counter;
                        return true;
                    }
                }
            }
            return false;
        }

        /*
         * 'Give your Column class a method called displayRow that takes a row number as an argument. 
         * Assume that the row number is always within an acceptable range. The method should return a 
         * String consisting of the counter’s character if there is a counter at that position and a String 
         * including a space character if there is no counter in that position.'
         */

        public string DisplayRow(int rowNumber)
        {
            if (Rows[rowNumber] is null)
            {
                return " ";
            }
            return Rows[rowNumber].Player.CounterSymbol + "";
        }

        // 'Give your Column class a display method that displays each row on a separate line'.
        public void display()
        {
            for (int i = 0; i < this.numRows; i++)
            {
                Console.WriteLine(this.DisplayRow(i));
            }
        }
    }

    class Board
    {
        private int numRows;
        private int numColumns;
        private Column[] columns;

        public Board(int numRows, int numColumns)
        {
            this.NumRows = numRows;
            this.NumColumns = numColumns;
            columns = new Column[numColumns];
            for (int i = 0; i < numColumns; i++)
            {
                columns[i] = new Column(numRows);
            }
        }

        public int NumRows { get => numRows; set => numRows = value; }
        public int NumColumns { get => numColumns; set => numColumns = value; }


        // 'have a boolean add method that takes a reference to a Counter object and a
        // column number as arguments(in that order) and returns true if the board
        //successfully adds the counter to that column, and false otherwise.'
        public bool Add(Counter counter, int colNum)
        {
            if (columns[colNum].IsFull()) { return false; }
            int rowOfFirstEmptySquare = this.NumRows - 1;
            for (int i = this.NumRows - 1; i > -1; i--)
            {
                if (columns[colNum].Rows[i] is null)
                {
                    columns[colNum].Rows[i] = counter;
                    return true;
                }
            }
            return true; // To avoid compile error.
        }

        public override string ToString()
        {
            // Top line
            string topLine = "\n|";
            for (int i = 0; i < this.NumColumns; i++)
            {
                string toAdd = i + "|";
                topLine += toAdd;
            }
            // Second line - Adds 2 hyphens per column
            string secondLine = "\n";
            for (int i = 0; i < this.NumColumns; i++)
            {
                secondLine += "--";
            }
            // Remaining lines
            string[] rows = new string[this.NumRows];
            for (int i = 0; i < this.NumRows; i++)
            {
                rows[i] = "\n|";
                for (int j = 0; j < this.NumColumns; j++)
                {
                    if (columns[j].Rows[i] is null)
                    {
                        rows[i] += " |";
                        continue;
                    }
                    if (columns[j].Rows[i].Player.CounterSymbol == 'x')
                    {
                        rows[i] += "x|";
                    }
                    if (columns[j].Rows[i].Player.CounterSymbol == 'o')
                    {
                        rows[i] += "o|";
                    }
                }
            }
            string firstTwoLines = topLine + secondLine;
            string remainingLines = "";
            foreach (string row in rows)
            {
                remainingLines += row;
            }
            return firstTwoLines + remainingLines;
        }

        // Is the board full? 
        public bool IsFull()
        {
            for (int i = 0; i < this.NumColumns; i++)
            {
                if (!columns[i].IsFull())
                {
                    return false;
                }
            }
            return true;
        }

        // Returns the counter at the given row and col position. 
        public Counter GetCounterAt(int row, int col)
        {
            return this.columns[col].Rows[row];
        }

        // Checks if there has been a winner. Looks for 'n in a row'. 
        public bool IsThereAWinner(int numberToConnect)
        {
            for (int row = 0; row < this.NumRows; row++)
            {
                for (int col = 0; col < this.NumColumns; col++)
                {
                    // If starting square is empty, move onto next square. 
                    if (this.GetCounterAt(row, col) is null)
                    {
                        continue;
                    }
                    // Is there a horizonal winner?
                    if (IsHorizontalWinner(row, col, numberToConnect)) { return true; }
                    // Is there a vertical winner?
                    if (IsVerticalWinner(row, col, numberToConnect)) { return true; }
                    // Is there a diagonal \ winner?
                    if (IsDiagDownwardsFromLeftToRightWinner(row, col, numberToConnect)) { return true; }
                    // Is there a diagonal / winner?
                    if (IsDiagDownwardsFromRightToLeftWinner(row, col, numberToConnect)) { return true; }
                }
            }
            return false;
        }

        bool IsHorizontalWinner(int row, int col, int numberToConnect)
        {
            int numColsInBoard = this.NumColumns;
            // If there are insufficient columns to the right for a win to be possible. 
            if ((numColsInBoard - col) < numberToConnect)
            {
                return false;
            }
            // Put the contents of 'numberToConnect' squares to the right of the current square 
            // into a list. 
            List<Counter> listOfCounters = new List<Counter>();
            int count = 0;
            while (count < numberToConnect)
            {
                if (this.GetCounterAt(row, col) is null)
                {
                    return false;
                }
                else
                {
                    listOfCounters.Add(this.GetCounterAt(row, col));
                    count++;
                    col++;
                }
            }
            if (!IsListOfCountersHomogenous(listOfCounters))
            {
                return false;
            }
            Console.WriteLine("\nHorizontal win");
            return true;
        }


        bool IsVerticalWinner(int row, int col, int numberToConnect)
        {
            int numRowsInBoard = this.NumRows;
            // If there are insufficient rows below current square for a win to be possible. 
            if ((numRowsInBoard - row) < numberToConnect)
            {
                return false;
            }
            // Put the contents of 'numberToConnect' squares beneath the current square into a
            // list
            List<Counter> listOfCounters = new List<Counter>();
            int count = 0;
            while (count < numberToConnect)
            {
                if (this.GetCounterAt(row, col) is null)
                {
                    return false;
                }
                else
                {
                    listOfCounters.Add(this.GetCounterAt(row, col));
                    count++;
                    row++;
                }
            }
            if (!IsListOfCountersHomogenous(listOfCounters))
            {
                return false;
            }
            Console.WriteLine("\nVertical win");
            return true;
        }

        bool IsDiagDownwardsFromLeftToRightWinner(int row, int col, int numberToConnect)
        {
            int numColsInBoard = this.NumColumns;
            int numRowsInBoard = this.NumRows;
            // If there are insufficient columns to the right for a win to be possible. 
            if ((numColsInBoard - col) < numberToConnect)
            {
                return false;
            }
            // If there are insufficient rows below current square for a win to be possible. 
            if ((numRowsInBoard - row) < numberToConnect)
            {
                return false;
            }
            // Put the contents of the next 'numberToConnect' squares to the left of the current square 
            // into a list. 
            List<Counter> listOfCounters = new List<Counter>();
            int count = 0;
            while (count < numberToConnect)
            {
                if (this.GetCounterAt(row, col) is null)
                {
                    return false;
                }
                else
                {
                    listOfCounters.Add(this.GetCounterAt(row, col));
                    count++;
                    row++;
                    col++;
                }
            }
            // Check if the Counter references in the list all point to the same object. 
            if (!IsListOfCountersHomogenous(listOfCounters))
            {
                return false;
            }
            Console.WriteLine("\nDiag down from left to right win");
            return true;
        }


        bool IsDiagDownwardsFromRightToLeftWinner(int row, int col, int numberToConnect)
        {
            int numColsInBoard = this.NumColumns;
            int numRowsInBoard = this.NumRows;
            // If there are insufficient columns to the left for a win to be possible. 
            if ((col + 1) - numberToConnect < 0)
            {
                return false;
            }
            // If there are insufficient rows below current square for a win to be possible. 
            if ((numRowsInBoard - row) < numberToConnect)
            {
                return false;
            }
            // Put the contents of the next 'numberToConnect' squares to the left of the current square 
            // into a list. 
            List<Counter> listOfCounters = new List<Counter>();
            int count = 0;
            while (count < numberToConnect)
            {
                if (this.GetCounterAt(row, col) is null)
                {
                    return false;
                }
                else
                {
                    listOfCounters.Add(this.GetCounterAt(row, col));
                    count++;
                    row++;
                    col--;
                }
            }
            if (!IsListOfCountersHomogenous(listOfCounters))
            {
                return false;
            }
            Console.WriteLine("\nDiag down from right to left win");
            return true;
        }
        
        // Check if the Counter references in the list all point to the same object. 
        private bool IsListOfCountersHomogenous(List<Counter> listOfCounters)
        {
            foreach (Counter toCheck in listOfCounters)
            {
                if (listOfCounters[0] != toCheck)
                {
                    return false;
                }
            }
            return true;
        }
    }

    class ConnectFour
    {
        // Two players playing randomly
        static void RandomPlay()
        {
            Board board = new Board(6, 7); //row, column
            Player p1 = new Player("Bob", 'x');
            Player p2 = new Player("Frank", 'o');

            bool isPlayerOneTurn = true;
            bool gameOver = false;
            var rand = new Random();
            Counter p1Counter = new Counter(p1);
            Counter p2Counter = new Counter(p2);
            Console.WriteLine("Are the two Counter objects equal? " + p1Counter.Equals(p2Counter));

            while (!gameOver)
            {
                if (isPlayerOneTurn)
                {
                    int randNum = rand.Next(board.NumColumns);
                    if (board.Add(p1Counter, randNum))
                    {
                        isPlayerOneTurn = false;
                        Console.WriteLine(board);
                    }
                }
                else
                {
                    int randNum = rand.Next(board.NumColumns);
                    if (board.Add(p2Counter, randNum))
                    {
                        isPlayerOneTurn = true;
                        Console.WriteLine(board);
                    }
                }
                // Assuming '4 in a row' required for a winner. 
                if (board.IsThereAWinner(4))
                {
                    gameOver = true;
                    Console.WriteLine("Game over - There is a winner");
                    break; // In case winning move renders the board full - avoids two 'game over' messages. 
                }
                if (board.IsFull())
                {
                    gameOver = true;
                    Console.WriteLine("Game over - Board full");
                }
            }
        }

        static void Main(string[] args)
        { 
            Console.WriteLine("Random play begins\n");
            RandomPlay();
        }
    }
}
