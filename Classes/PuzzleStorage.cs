using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace JapanezePuzzle.Classes
{
    // Static class for saving and loading puzzles from json file
    public static class PuzzleStorage
    {
        private const string DefaultFileName = "puzzles.json";

        /// <summary>
        /// Saves the list of puzzles to a JSON file on disk.
        /// </summary>
        public static void SavePuzzles(List<Puzzle> puzzles, string filePath = DefaultFileName)
        {
            // Serialize with indentation for human readability
            var json = JsonConvert.SerializeObject(puzzles, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Loads a list of puzzles from a JSON file on disk.
        /// </summary>
        public static List<Puzzle> LoadPuzzles(string filePath = DefaultFileName)
        {
            if (!File.Exists(filePath))
            {
                // If file doesn't exist, return an empty list
                return new List<Puzzle>();
            }

            var json = File.ReadAllText(filePath);
            var puzzles = JsonConvert.DeserializeObject<List<Puzzle>>(json);

            return puzzles ?? new List<Puzzle>();
        }

        /// <summary>
        /// Updates a puzzle by ID. If it exists, it will be replaced. If not, it will be added.
        /// </summary>
        public static void SavePuzzle(Puzzle puzzle, string filePath = DefaultFileName)
        {
            List<Puzzle> puzzles = LoadPuzzles(filePath);

            // Find the index of the puzzle with the same ID
            int index = puzzles.FindIndex(p => p.Id == puzzle.Id);

            if (index != -1)
            {
                // Replace the existing puzzle
                puzzles[index] = puzzle;
            }
            else
            {
                // Add new puzzle if not found
                puzzles.Add(puzzle);
            }

            // Save updated list back to file
            SavePuzzles(puzzles, filePath);
        }
    }
}
