using System.Text.RegularExpressions;

namespace Library
{
    internal class ValidateInputs
    {
        public (T, bool) GetValidInput<T>(Func<string, bool> validationFunc, Func<string, T> conversionFunc, string warning)
        {
            string userInput = Console.ReadLine();

            if (!validationFunc(userInput))
            {
                if (!NotNullOrWhiteSpace(userInput))
                {
                    return (conversionFunc(userInput), true);
                }
                Console.Clear();
                Console.WriteLine(warning);
                return GetValidInput(validationFunc, conversionFunc, warning);
            }

            return (conversionFunc(userInput), false);
        }
        public bool NotNullOrWhiteSpace(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
        public bool IsInteger(string input)
        {
            input = Regex.Replace(input, @"[^\d]", "");
            return int.TryParse(input, out _);
        }
        public bool IsIntegerDashesAndWhiteSpacesRemoved(string input)
        {
            input = input.Replace("-", "").Replace(" ", "");
            return int.TryParse(input, out _);
        }
        public bool IsDateTime(string input)
        {
            return DateTime.TryParseExact(input, "yyyy-M-d h:m", null, System.Globalization.DateTimeStyles.None, out DateTime _);
        }
        public int ConvertToIntegerDashesAndWhiteSpacesRemoved(string input)
        {
            input = input.Replace("-", "").Replace(" ", "");
            return int.TryParse(input, out int result) ? result : 0;
        }
        public int RemoveNonDigits(string input)
        {
            string newString = Regex.Replace(input, @"[^\d]", "");
            if (int.TryParse(newString, out int value))
            {
                return value;
            }
            else
            {

                return 0;
            }
        }
        public int ConverToInteger(string input)
        {
            return int.TryParse(input, out int result) ? result : 0;
        }
        public string ReturnString(string input)
        {
            return input;
        }
    }
}
