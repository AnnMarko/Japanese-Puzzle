﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JapanezePuzzle.Classes
{
    /// <summary>
    /// Static class for saving and loading puzzles from json file.
    /// </summary>
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

            // Deserialize the JSON back into a list of Puzzle objects
            try
            {
                var puzzles = JsonConvert.DeserializeObject<List<Puzzle>>(json);

                return puzzles ?? new List<Puzzle>();
            }
            catch (Exception) // if file is corrupted, rewrite it
            {

                var puzzles = Classes.Puzzle.CreateHardcodedPuzzles();
                PuzzleStorage.SavePuzzles(puzzles);
                return puzzles ?? new List<Puzzle>();
            }
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

        /// <summary>
        /// Gets the max index of puzzles in the storage.
        /// </summary>
        public static int GetMaxId(string filePath = DefaultFileName)
        {
            List<Puzzle> puzzles = LoadPuzzles(filePath);
            if (puzzles.Count == 0)
            {
                // If no puzzles are found, return -1
                return -1;
            }

            return puzzles.Max(p => p.Id);
        }
    }
}
