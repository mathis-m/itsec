using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAnalysis
{
    public class FrequencyAnalyzer
    {
        private Dictionary<char, int> _charCounter = new();
        private int _textLength = 0;
        public FrequencyAnalyzer(string text)
        {
            _textLength = text.Length;
            var upper = text.ToUpper();
            foreach (var currentChar in upper.Where(char.IsLetterOrDigit))
            {
                if (!_charCounter.ContainsKey(currentChar))
                {
                    _charCounter[currentChar] = 1;
                }
                else
                {
                    _charCounter[currentChar]++;
                }
            }
        }

        private List<KeyValuePair<char, int>> GetCharCountsSorted()
        {
            return _charCounter
                .ToList()
                .OrderByDescending(kv1 => kv1.Value)
                .ToList();
        }

        public void PrintResults()
        {
            foreach (var (currentChar, count) in GetCharCountsSorted())
            {
                var frequency = Math.Round((double)count / _textLength * 100, 2);
                Console.WriteLine($"The letter {currentChar} occurs {count} times in the text and the frequency in percent is {frequency}");
            }
        }

        public string GetCsvData()
        {
            var sb = new StringBuilder();
            var currentChar = 'A';
            sb.AppendLine("Char;Frequency");
            
            do
            {
                var count = _charCounter.ContainsKey(currentChar) ? _charCounter[currentChar] : 0;
                var frequency = Math.Round((double)count / _textLength * 100, 2);
                sb.AppendLine($"{currentChar};{frequency}");
            } while (currentChar++ != 'Z');
        
            return sb.ToString();
        }
    }
}
