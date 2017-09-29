using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopPhrases
{
    public class Program
    {
        static void Main(string[] args)
        {
            string document = System.IO.File.ReadAllText(@"Documents\TestFile3.txt");
            var top10phrases = GetTop10Phrases(document);
            foreach (var phrase in top10phrases)
            {
                Console.WriteLine(phrase.Key + ": " + phrase.Value);
            }
            Console.ReadKey();
        }

        public static Dictionary<string, int> GetTop10Phrases(string document)
        {
            Dictionary<string, int> Phrases = new Dictionary<string, int>();

            char[] sentenceDelimiters = { '.', '!', '?' };
            string[] sentences = document.Split(sentenceDelimiters);
            char wordDelimiter = ' ';

            foreach (var sentence in sentences)
            {
                string cleanedString = System.Text.RegularExpressions.Regex.Replace(sentence, @"\s+", " ");
                string[] words = cleanedString.Split(wordDelimiter);

                if (words.Count() < 3) continue;
                if (words.Count() >= 3)
                {
                    //Points to the index of the last word that we start iterating backwards from to 
                    //store its value along with the 2-9 words before it to form a phrase
                    int end = words.Count() - 1;

                    while (end >= 2)
                    { 
                        string subsentence = words[end - 1] + wordDelimiter + words[end];

                        //Loop through up to 7 times before decrementing the ending index
                        for (int i = 2; i < 9; i++)
                        {
                            // If we have passed the start of the sentence
                            if (end - i < 0 || words[end - i] == "") break;

                            subsentence = words[end - i] + wordDelimiter + subsentence;
                            CheckDictionary(subsentence.ToLower(), Phrases);
                        }
                        //If we can keep iterating backwards
                        if (words[end - 1] != null)
                        {
                            end--;
                        }
                    }
                }
            }
            
            //Non-foolproof way to make it run slightly faster by only sending the top 70 phrases to the RemoveSubsets function
            var top70Phrases = Phrases.OrderByDescending(x => x.Value).Take(70).ToDictionary(pair => pair.Key, pair => pair.Value); 

            return RemoveSubsets(top70Phrases).Take(10).ToDictionary(pair => pair.Key, pair => pair.Value); 
        }

        public static void CheckDictionary(string phrase, Dictionary<string, int> Phrases)
        {
            if (Phrases.Count() != 0)
            {
                if (Phrases.ContainsKey(phrase))
                    Phrases[phrase]++;
                else
                    Phrases[phrase] = 1;
            }
            //If the dictionary is currently empty, add the first value
            else
            {
                Phrases.Add(phrase, 1);
            }
        }

        //Loops through each phrase then first checks if there are any other phrases that contain the current phrase (any supersets of it) 
        //and if there are, it then checks if the count values of it are equal. If equal counts, remove the subset one from the returned list. 
        //This could be optimized to be made more efficient.
        public static Dictionary<string, int> RemoveSubsets(Dictionary<string, int> Phrases)
        {
            foreach (var phrase in Phrases.ToList())
            {
                var currentPhrase = Phrases.Where(x => phrase.Key == x.Key);
                var phraseSupersets = Phrases.Where(x => x.Key.Contains(phrase.Key)).Except(currentPhrase).ToList();
                foreach (var phraseSuperSet in phraseSupersets)
                {
                    if (phraseSuperSet.Value == phrase.Value)
                    {
                        Phrases.Remove(phrase.Key);
                    }
                }
            }
            return Phrases;
        }
    }
}
