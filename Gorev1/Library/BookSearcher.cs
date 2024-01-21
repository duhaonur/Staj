namespace Library
{
    internal class BookSearcher
    {
        ValidateInputs _validateInput = new ValidateInputs();

        public (List<Book>, bool terminate) FindBook(IEnumerable<Book> books)
        {
            bool terminate = false;

            Console.Clear();
            Console.WriteLine("Please select the search method:");
            Console.WriteLine("1: Search by Book Name");
            Console.WriteLine("2: Search by Author Name");
            Console.WriteLine("3: Search by ISBN");

            ConsoleKeyInfo key = Console.ReadKey();

            List<Book> foundBooks = new List<Book>();

            if (key.Key == ConsoleKey.D1)
            {
                Console.Clear();
                Console.WriteLine("Please enter the name of the book:");

                (string input, terminate) = _validateInput.GetValidInput(_validateInput.NotNullOrWhiteSpace, _validateInput.ReturnString, "Invalid input. Enter the book name or press enter to return to the menu:");

                if (terminate)
                    return (null, true);

                foreach (var book in books)
                {
                    if (book.BookName.ToLower().StartsWith(input.ToLower()))
                    {
                        foundBooks.Add(book);
                    }
                }
                return (foundBooks, false);
            }
            else if (key.Key == ConsoleKey.D2)
            {
                Console.Clear();
                Console.WriteLine("Please enter the name of the author:");

                (string input, terminate) = _validateInput.GetValidInput(_validateInput.NotNullOrWhiteSpace, _validateInput.ReturnString, "Invalid input. Enter the author's name or press enter to return to the menu:");

                if (terminate)
                    return (null, true);

                foreach (var book in books)
                {
                    if (book.AuthorOfTheBook.ToLower().StartsWith(input.ToLower()))
                    {
                        foundBooks.Add(book);
                    }
                }
                return (foundBooks, false);
            }
            else if (key.Key == ConsoleKey.D3)
            {
                Console.Clear();
                Console.WriteLine("Please enter the ISBN:");

                (int input, terminate) = _validateInput.GetValidInput(_validateInput.IsIntegerDashesAndWhiteSpacesRemoved, _validateInput.ConvertToIntegerDashesAndWhiteSpacesRemoved, "Invalid input. Enter the ISBN or press enter to return to the menu:");

                if (terminate)
                    return (null, true);

                foreach (var book in books)
                {
                    if (book.Isbn.StartsWith(input.ToString()))
                    {
                        foundBooks.Add(book);
                    }
                }
                return (foundBooks, false);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Wrong Input, Press enter to return to the menu");
                Console.ReadLine();
            }

            return (null, true);
        }
    }
}
