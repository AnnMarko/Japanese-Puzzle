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
        public int[,] PuzzleCellMatrix
        {
            get => _puzzleCells;
            set => _puzzleCells = value;
        }


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

            // Initialize PuzzleCellMatrix as an all-zero 2D array
            PuzzleCellMatrix = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    PuzzleCellMatrix[i, j] = 0;
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
                PuzzleNumbers = CalculatePuzzleNumbers();
                PuzzleCellMatrix = puzzleCells;
            }
            else
            {
                PuzzleCellMatrix = new int[rows, cols];
                FillAllCellsWithZero();
                PuzzleNumbers = CalculatePuzzleNumbers();
            }
        }
        public void FillAllCellsWithZero()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    PuzzleCellMatrix[i, j] = 0;
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

        public int[][][] CalculatePuzzleNumbers()
        {
            int length = PuzzleCellMatrix.GetLength(0);
            int width = PuzzleCellMatrix.GetLength(1);

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
                        switch (i > 0 ? PuzzleCellMatrix[k, j] : PuzzleCellMatrix[j, k])
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
                    int numbersCount = 0;
                    for (int k = 0; k < numbers.Length; k++)
                    {
                        if (numbers[k] != 0)
                        {
                            numbersCount++;
                        }
                    }
                    int[] numbersToSave;
                    if (numbersCount == 0)
                    {
                        numbersToSave = new int[] { 0 };
                    }
                    else
                    {
                        numbersToSave = new int[numbersCount];
                        for (int k = 0; k < numbersToSave.Length; k++)
                        {
                            numbersToSave[k] = numbers[k];
                        }
                    }
                    puzzleNumbers[i][j] = numbersToSave;
                }
            }
            return puzzleNumbers;
        }

        public bool HasAtLeastOneNumber()
        {
            int length = PuzzleCellMatrix.GetLength(0);
            int width = PuzzleCellMatrix.GetLength(1);

            for (int i = 0; i < PuzzleNumbers.Length; i++)
            {
                for (int j = 0; j < (i > 0 ? length : width); j++)
                {
                    for (int k = 0; k < PuzzleNumbers[i][j].Length; k++)
                    {
                        if (PuzzleNumbers[i][j][k] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static public List<Puzzle> CreateHardcodedPuzzles()
        {
            var puzzles = new List<Puzzle>();

            puzzles.Add(
            new Puzzle(
                -1, 13, 12,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 8 },
                        new int[] { 2, 2 },
                        new int[] { 2, 2 },
                        new int[] { 1, 4, 1 },
                        new int[] { 1, 2, 2, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 2, 2, 2 },
                        new int[] { 2, 3, 3 },
                        new int[] { 2 },
                        new int[] { 2, 2 },
                        new int[] { 7 }
                    },
                    new int[][]
                    {
                        new int[] { 8 },
                        new int[] { 2, 2 },
                        new int[] { 2, 2 },
                        new int[] { 1, 5, 2 },
                        new int[] { 1, 2, 2, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 2, 1, 1 },
                        new int[] { 1, 6, 1 },
                        new int[] { 2, 1, 2 },
                        new int[] { 2, 2, 1 },
                        new int[] { 7 }
                    }
                },
                "At",
                1
                )
            );

            puzzles.Add(
            new Puzzle(
                0, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1, 1 },
                        new int[] { 5 },
                        new int[] { 5 },
                        new int[] { 3 },
                        new int[] { 1 },
                    },
                    new int[][]
                    {
                        new int[] { 2 },
                        new int[] { 4 },
                        new int[] { 4 },
                        new int[] { 4 },
                        new int[] { 2 },
                    }
                },
                "Heart"
                )
            );

            puzzles.Add(
            new Puzzle(
                1, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1 },
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1 },
                        new int[] { 1, 1, 1 },
                    },
                    new int[][]
                    {
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1 },
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1 },
                        new int[] { 1, 1, 1 },
                    }
                },
                "Chess"
                )
            );

            puzzles.Add(
            new Puzzle(
                2, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 2 },
                        new int[] { 1 },
                        new int[] { 5 },
                        new int[] { 3 },
                    },
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 2 },
                        new int[] { 5 },
                        new int[] { 1, 2 },
                        new int[] { 1 },
                    }
                },
                "Boat"
                )
            );

            puzzles.Add(
            new Puzzle(
                3, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 2, 1 },
                        new int[] { 4 },
                        new int[] { 2, 1 },
                        new int[] { 1 },
                    },
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 3 },
                        new int[] { 5 },
                        new int[] { 1 },
                        new int[] { 1, 1 },
                    }
                },
                "Fish"
                )
            );

            puzzles.Add(
            new Puzzle(
                4, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 2, 1 },
                        new int[] { 2 },
                        new int[] { 2, 2 },
                        new int[] { 2 },
                        new int[] { 3 },
                    },
                    new int[][]
                    {
                        new int[] { 1, 2 },
                        new int[] { 1, 2 },
                        new int[] { 1, 1 },
                        new int[] { 2, 1 },
                        new int[] { 1, 1, 1 },
                    }
                },
                "Tetris"
                )
            );

            puzzles.Add(
            new Puzzle(
                5, 10, 9,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 3 },
                        new int[] { 2, 2 },
                        new int[] { 2, 2 },
                        new int[] { 9 },
                        new int[] { 7 },
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1, 1 },
                        new int[] { 4, 1 },
                        new int[] { 4, 1 },
                    },
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 7 },
                        new int[] { 4, 2 },
                        new int[] { 2, 2, 2 },
                        new int[] { 2, 6 },
                        new int[] { 2, 2 },
                        new int[] { 4 },
                        new int[] { 7 },
                        new int[] { 1 },
                    }
                },
                "House",
                1
                )
            );

            puzzles.Add(
            new Puzzle(
                6, 9, 9,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 5 },
                        new int[] { 7 },
                        new int[] { 9 },
                        new int[] { 1, 1, 1, 1, 1 },
                        new int[] { 1 },
                        new int[] { 1 },
                        new int[] { 1, 1 },
                        new int[] { 3 },
                    },
                    new int[][]
                    {
                        new int[] { 2 },
                        new int[] { 2 },
                        new int[] { 4, 2 },
                        new int[] { 3, 1 },
                        new int[] { 9 },
                        new int[] { 3 },
                        new int[] { 4 },
                        new int[] { 2 },
                        new int[] { 2 },
                    }
                },
                "Umbrella",
                1
                )
            );

            puzzles.Add(
            new Puzzle(
                7, 15, 15,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 3 },
                        new int[] { 3, 1 },
                        new int[] { 3, 1 },
                        new int[] { 5, 2 },
                        new int[] { 6, 3 },
                        new int[] { 2, 6, 2 },
                        new int[] { 4, 8 },
                        new int[] { 2, 2, 8 },
                        new int[] { 5, 5, 2 },
                        new int[] { 3, 5, 2 },
                        new int[] { 7, 2 },
                        new int[] { 5, 1 },
                        new int[] { 4, 1 },
                        new int[] { 4 },
                        new int[] { 3 },
                    },
                    new int[][]
                    {
                        new int[] { 2 },
                        new int[] { 4 },
                        new int[] { 2, 2 },
                        new int[] { 7 },
                        new int[] { 1, 3, 2 },
                        new int[] { 3, 3 },
                        new int[] { 10 },
                        new int[] { 12 },
                        new int[] { 13 },
                        new int[] { 4, 6, 3 },
                        new int[] { 2, 4, 2 },
                        new int[] { 1, 2, 1 },
                        new int[] { 4 },
                        new int[] { 8 },
                        new int[] { 4, 5 },
                    }
                },
                "Big fish",
                2
                )
            );

            puzzles.Add(
            new Puzzle(
                8, 15, 18,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 3, 4 },
                        new int[] { 1, 6 },
                        new int[] { 1, 1, 1, 1, 1, 6 },
                        new int[] { 3, 1, 6 },
                        new int[] { 5, 2, 1, 2 },
                        new int[] { 2, 6, 1, 2, 1 },
                        new int[] { 1, 2, 3, 2, 1 },
                        new int[] { 3, 1 },
                        new int[] { 2, 2 },
                        new int[] { 3, 3 },
                        new int[] { 3, 3 },
                        new int[] { 3, 5 },
                        new int[] { 3, 5 },
                        new int[] { 1, 1, 1 },
                        new int[] { 8 },
                    },
                    new int[][]
                    {
                        new int[] { 1, 6 },
                        new int[] { 3, 4 },
                        new int[] { 3, 2, 2 },
                        new int[] { 4, 3 },
                        new int[] { 1, 2, 2, 1 },
                        new int[] { 1, 3 },
                        new int[] { 1, 1 },
                        new int[] { 1, 3 },
                        new int[] { 3, 2, 2, 1 },
                        new int[] { 1, 1, 1, 4 },
                        new int[] { 1, 1, 3, 2, 1 },
                        new int[] { 1, 3, 1 },
                        new int[] { 5, 3 },
                        new int[] { 4, 2, 3 },
                        new int[] { 4, 4 },
                        new int[] { 4 },
                        new int[] { 5 },
                        new int[] { 6 },
                    }
                },
                "Pelican",
                2
                )
            );

            return puzzles;
        }
    }
}
