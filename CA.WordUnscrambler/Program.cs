using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CA.WordUnscrambler
{
    class Program
    {
        private const string dictionaryFileName = "0_palabras_todas.txt";
        private static Dictionary<int, string> mainDictionary;
        private static Dictionary<string, List<string>> helperDictionary;

        static void Main(string[] args)
        {
            // Load spanish list of words in a dictionary
            // Key - Word
            // 1   - a
            // 2   - aba
            // 3   - abaá
            mainDictionary = new Dictionary<int, string>();

            string[] allWords = File.ReadAllLines(dictionaryFileName);

            int L = allWords.Length;
            for (int i = 0; i < L; i++)
            {
                mainDictionary.Add(i+1, allWords[i]);
            }


            // Create the reference helper in another dictionary
            // Ref  - List of words
            // mara - {amar, arma, mara, rama}
            // 
            // This is done by applying a transformation to every word
            // which consists in sorting (rama --> aamr)

            helperDictionary = new Dictionary<string, List<string>>();
            char[] ch;
            string key;
            foreach (string w in allWords)
            {
                ch = w.ToCharArray();
                Array.Sort(ch);
                key = new string(ch);
                if (helperDictionary.ContainsKey(key))
                {
                    var matchingWords = helperDictionary[key];
                    matchingWords.Add(w);
                    helperDictionary[key] = matchingWords;
                }
                else
                {
                    helperDictionary.Add(key, new List<string>() { w });
                }
            }


            // Main bussiness logic
            // Just asks the user to type a word
            // Repeat it forever, unless user types exit
            bool exit = false;
            do
            {
                Console.Write("Type a word: ");
                string inWord = Console.ReadLine().ToLower() ?? string.Empty;

                // Find matching words
                List<string> matchedWords = FindMatchingWords(inWord);

                // Write results in console
                if (matchedWords != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{inWord}");

                    int inWordLength = inWord.Length;
                    char[] lineInConsole = new char[inWordLength];
                    for (int i = 0; i < inWordLength; i++)
                        lineInConsole[i] = '-';
                    
                    Console.WriteLine(lineInConsole);
                    foreach (string w in matchedWords)
                        Console.WriteLine($"{w}");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("No matching found.");
                }

                // Try again?
                Console.WriteLine();
                Console.Write("Would you like to try again? [y/n]: ");
                string answer = Console.ReadLine().ToLower() ?? string.Empty;

                if (!answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    exit = true;
                
                Console.WriteLine();

            } while (!exit);


        }

        private static List<string> FindMatchingWords(string inWord)
        {
            List<string> matchedWords = new List<string>();

            // Sort input word as char[]
            char[] ch = inWord.ToCharArray();
            Array.Sort(ch);

            // Find key in refDictionary
            string key = new string(ch);
            helperDictionary.TryGetValue(key, out matchedWords);

            return matchedWords;
        }


    }
}
