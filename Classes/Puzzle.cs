using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapanezePuzzle.Classes
{
    // Class for japanese puzzles
    public class Puzzle
    {
        // Private fields
        private int _id;
        private string _name;
        private int _rows;
        private int _cols;
        private int _difficulty;
        private bool _isSolved;
        private int[][][] _puzzleNumbers;
        private int[,] _puzzleCells;

        [JsonProperty("id")]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [JsonProperty("rows")]
        public int Rows
        {
            get => _rows;
            set => _rows = value;
        }

        [JsonProperty("cols")]
        public int Cols
        {
            get => _cols;
            set => _cols = value;
        }

        [JsonProperty("difficulty")]
        public int Difficulty
        {
            get => _difficulty;
            set => _difficulty = value;
        }

        [JsonProperty("isSolved")]
        public bool IsSolved
        {
            get => _isSolved;
            set => _isSolved = value;
        }

        /// <summary>
        /// A three-dimensional jagged array representing 
        /// the numeric hints of rows and columns (e.g., puzzleNumbers[0] for rows, puzzleNumbers[1] for columns).
        /// </summary>
        [JsonProperty("puzzleNumbers")]
        public int[][][] PuzzleNumbers
        {
            get => _puzzleNumbers;
            set => _puzzleNumbers = value;
        }

        /// <summary>
        /// A special property to handle the 2D puzzle cells (int[,]) 
        /// as a jagged array (int[][]) so JSON serialization works easily.
        /// </summary>
        [JsonProperty("puzzleCells")]
        public int[][] PuzzleCells
        {
            get
            {
                // Convert the private int[,] to a jagged int[][] before serialization
                var rows = _puzzleCells.GetLength(0);
                var cols = _puzzleCells.GetLength(1);

                var jagged = new int[rows][];
                for (int i = 0; i < rows; i++)
                {
                    jagged[i] = new int[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        jagged[i][j] = _puzzleCells[i, j];
                    }
                }
                return jagged;
            }
            set
            {
                // Convert back from int[][] to int[,] after deserialization
                var rows = value.Length;
                var cols = value[0].Length;

                _puzzleCells = new int[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        _puzzleCells[i, j] = value[i][j];
                    }
                }
            }
        }

        [JsonIgnore]
        public int[,] PuzzleCellMatrix => _puzzleCells;


        /// <summary>
        /// This constructor will be used by the serializer during deserialization.
        /// </summary>
        [JsonConstructor]
        public Puzzle(int id, int rows, int cols, int[][][] puzzleNumbers,
                      string name, int difficulty, bool isSolved, int[][] puzzleCells)
        {
            _id = id;
            _rows = rows;
            _cols = cols;
            _puzzleNumbers = puzzleNumbers;
            _name = name;
            _difficulty = difficulty;
            _isSolved = isSolved;
            // Use the PuzzleCells property setter to fill the private _puzzleCells
            PuzzleCells = puzzleCells;
        }

        /// <summary>
        /// Your original constructor for a new puzzle that is not yet solved.
        /// </summary>
        public Puzzle(int currentMaxId, int rows, int cols, int[][][] puzzleNumbers,
                      string name = null, int difficulty = 0)
        {
            _id = currentMaxId + 1;
            _rows = rows;
            _cols = cols;
            _difficulty = difficulty;
            _isSolved = false;
            _name = name;
            _puzzleNumbers = puzzleNumbers;

            // Initialize _puzzleCells as an all-zero 2D array
            _puzzleCells = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _puzzleCells[i, j] = 0;
                }
            }
        }
        public Puzzle(int currentMaxId, int rows, int cols, int[,] puzzleCells = null, 
                    string name = null, int difficulty = 0)
        {
            _id = currentMaxId + 1;
            Rows = rows;
            Cols = cols;
            Difficulty = difficulty;
            IsSolved = true;
            Name = name;
            if (puzzleCells != null)
            {
                _puzzleNumbers = CalculatePuzzleNumbers(puzzleCells);
                _puzzleCells = puzzleCells;
            }
            else
            {
                _puzzleCells = new int[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        _puzzleCells[i, j] = 0;
                    }
                }
            }
        }

        public void MarkAsSolved(bool isSolved = true)
        {
            IsSolved = isSolved;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        private int[][][] CalculatePuzzleNumbers(int[,] puzzleCells)
        {
            int length = puzzleCells.GetLength(0);
            int width = puzzleCells.GetLength(1);

            int[][][] puzzleNumbers = new int[][][]
            {
                new int[length][],
                new int[width][]
            };

            for (int i = 0; i < puzzleNumbers.Length; i++)
            {
                for (int j = 0; j < (i > 0 ? length : width); j++)
                {
                    int value = 0;
                    int maxPossibleNumbersCount = (length + 1) / 2;
                    int[] numbers = new int[maxPossibleNumbersCount];
                    int numberIndex = 0;

                    for (int k = 0; k < (i > 0 ? width : length); k++)
                    {
                        switch (i > 0 ? puzzleCells[k, j] : puzzleCells[j, k])
                        {
                            case 0:
                                if (value != 0)
                                {
                                    numbers[numberIndex++] = value;
                                    value = 0;
                                }
                                break;
                            case 1:
                                value++;
                                break;
                        }
                    }
                    if (value != 0)
                    {
                        numbers[numberIndex++] = value;
                        value = 0;
                    }
                    puzzleNumbers[i][j] = numbers;
                }
            }
            return puzzleNumbers;
        }
    }
}
