namespace Library
{
    internal class LibraryManager
    {
        public Dictionary<string, Book> BooksInLibrary;

        public List<Rent> RentedBooks;
        public List<Book> DuplicateBooks;

        private readonly TableCreator _tableCreator;
        private readonly BookSearcher _bookSearcher;
        private readonly SaveLoadSystem _saveLoad;
        private readonly ValidateInputs _validateInputs;

        private readonly string _booksFilePath = "books.dat";
        private readonly string _duplicateBooks = "duplicateBooks.dat";
        private readonly string _rentedBooksFilePath = "rentedBooks.dat";

        public LibraryManager()
        {
            _tableCreator = new TableCreator();
            _bookSearcher = new BookSearcher();
            _saveLoad = new SaveLoadSystem();
            _validateInputs = new ValidateInputs();

            BooksInLibrary = _saveLoad.LoadData<Dictionary<string, Book>>(_booksFilePath) ?? new Dictionary<string, Book>();
            DuplicateBooks = _saveLoad.LoadData<List<Book>>(_duplicateBooks) ?? new List<Book>();
            RentedBooks = _saveLoad.LoadData<List<Rent>>(_rentedBooksFilePath) ?? new List<Rent>();
        }
        public void SaveData()
        {
            _saveLoad.SaveData(BooksInLibrary, _booksFilePath);
            _saveLoad.SaveData(DuplicateBooks, _duplicateBooks);
            _saveLoad.SaveData(RentedBooks, _rentedBooksFilePath);
        }
        public void RemoveDuplicateBooks()
        {
            if (DuplicateBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No duplicate books found. Press any key to return to the menu.");
                Console.ReadLine();
            }
            else
            {
                DuplicateBooks.Clear();
                Console.Clear();
                Console.WriteLine("Duplicate books have been removed. Press any key to return to the menu.");
                Console.ReadLine();
            }
        }
        public void AddNewBook()
        {
            bool terminate = false;

            (Book newBook, terminate) = CreateNewBook();

            if (terminate)
                return;

            if (!BooksInLibrary.ContainsKey(newBook.Isbn))
            {
                BooksInLibrary.Add(newBook.Isbn, newBook);
                Console.Clear();
                Console.WriteLine("Book added to the library. Press any key to return to the menu");
                Console.ReadLine();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("A book with the same ISBN already exists in the library. Do you want to continue and add the duplicate? (Y/N):");
                DisplayDuplicateBook(BooksInLibrary[newBook.Isbn]);

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Y)
                {
                    AddDuplicateBook(newBook);
                    Console.Clear();
                    Console.WriteLine("Book added as a duplicate. Press any key to return to the menu.");
                    Console.ReadLine();
                }
                else if (key.Key == ConsoleKey.N)
                {
                    Console.Clear();
                    Console.WriteLine("Book didn't add. Press any key to return to the menu.");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Input, returning to the menu");
                    Thread.Sleep(3000);
                }
            }
        }

        public void DisplayBooksInLibrary()
        {
            if (BooksInLibrary.Values.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No books found in the library. Press any key to return to the menu.");
                Console.ReadLine();
                return;
            }

            int maxNameLength = BooksInLibrary.Values.Max(book => book.BookName.Length);
            int maxAuthorLength = BooksInLibrary.Values.Max(book => book.AuthorOfTheBook.Length);
            int maxIsbnLength = BooksInLibrary.Values.Max(book => book.Isbn.Length);
            int maxCopiesLength = BooksInLibrary.Values.Max(book => book.CopiesOfTheBook.ToString().Length);

            int[] minLengths = { 4, 6, 4, 6 };
            int[] maxLengths = { maxNameLength, maxAuthorLength, maxIsbnLength, maxCopiesLength };

            Console.Clear();
            Console.WriteLine(_tableCreator.GenerateTableHeader(minLengths, maxLengths, "Name", "Author", "ISBN", "Copies"));
            Console.WriteLine(_tableCreator.GenerateHorizontalLine(minLengths, maxLengths));

            DisplayBooksCollection(BooksInLibrary.Values, minLengths, maxLengths);
            DisplayBooksCollection(DuplicateBooks, minLengths, maxLengths);

            Console.WriteLine("Press any key to return to the menu");
            Console.ReadLine();
        }
        public void DisplayRentedBooks()
        {
            if (RentedBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No books are currently rented. Press any key to return to the menu.");
                Console.ReadLine();
                return;
            }

            int maxNameLength = RentedBooks.Max(book => book.RentedBook.BookName.Length);
            int maxAuthorLength = RentedBooks.Max(book => book.RentedBook.AuthorOfTheBook.Length);
            int maxIsbnLength = RentedBooks.Max(book => book.RentedBook.Isbn.Length);
            int maxRentDateLength = RentedBooks.Max(book => book.RentDate.ToString().Length);
            int maxDueDateLength = RentedBooks.Max(book => book.DueDate.ToString().Length);

            int[] minLengths = { 4, 6, 4, 9, 8, 15 };
            int[] maxLengths = { maxNameLength, maxAuthorLength, maxIsbnLength, maxRentDateLength, maxDueDateLength, 15 };

            Console.Clear();
            Console.WriteLine(_tableCreator.GenerateTableHeader(minLengths, maxLengths, "Name", "Author", "ISBN", "Rent Date", "Due Date", "Passed Due Time"));
            Console.WriteLine(_tableCreator.GenerateHorizontalLine(minLengths, maxLengths));

            foreach (var book in RentedBooks)
            {
                Console.WriteLine(_tableCreator.GenerateTableRow(minLengths, maxLengths, book.RentedBook.BookName, book.RentedBook.AuthorOfTheBook, book.RentedBook.Isbn, book.RentDate.ToString(), book.DueDate.ToString(), book.PassedDueTime() ? "Yes" : "No"));
                Console.WriteLine(_tableCreator.GenerateHorizontalLine(minLengths, maxLengths));
            }

            Console.WriteLine("Press any key to return to the menu");
            Console.ReadLine();
        }


        public void SearchBook()
        {
            if(BooksInLibrary.Count == 0 && DuplicateBooks.Count == 0 && RentedBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No books found in the library. Press any key to return to the menu.");
                Console.ReadLine();
                return;
            }

            bool terminate = false;

            (List<Book> foundBooks, terminate) = _bookSearcher.FindBook(BooksInLibrary.Values);

            if (terminate)
                return;

            Book book = null;
            int selection;

            if (foundBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No book found with the provided information. Press any key to return to the menu.");
                Console.ReadLine();
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Please select a book from the list");
                for (int i = 0; i < foundBooks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}-BookName:{foundBooks[i].BookName} ISBN:{foundBooks[i].Isbn}");
                }

                ConsoleKeyInfo bookSelectKey = Console.ReadKey();
                if (char.IsDigit(bookSelectKey.KeyChar))
                {
                    if (int.Parse(bookSelectKey.KeyChar.ToString()) <= foundBooks.Count)
                    {
                        book = foundBooks[int.Parse(bookSelectKey.KeyChar.ToString()) - 1];
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong Input, press any key to return to the menu");
                        Console.ReadLine();
                        return;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Input, press any key to return to the menu");
                    Console.ReadLine();
                    return;
                }
            }

            Console.Clear();
            Console.WriteLine($"Selected Book: {book.BookName} ISBN: {book.Isbn}\nPlease select the action you want to perform:");
            Console.WriteLine("1. Update Book Information");
            Console.WriteLine("2. Rent Book");
            Console.WriteLine("3. Return Book");
            Console.WriteLine("4. Return to Menu");

            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
            {
                ChangeBookInformations(book);
            }
            else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2)
            {
                RentBook(false, book);
            }
            else if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.NumPad3)
            {
                ReturnBook(false, book);
            }
            else if (key.Key == ConsoleKey.D4 || key.Key == ConsoleKey.NumPad4)
            {
                return;
            }
        }
        public void ChangeBookInformations(Book book)
        {
            bool terminate = false;

            Console.Clear();
            Console.WriteLine($"Selected Book: {book.BookName} ISBN: {book.Isbn}\nPlease select the action you want to perform:");
            Console.WriteLine("1. Remove book from the library");
            Console.WriteLine("2. Remove one or more copies from the library:");
            Console.WriteLine("3. Add one or more copies to the library:");
            Console.WriteLine("4. Change book name:");
            Console.WriteLine("5. Change author name:");
            Console.WriteLine("6. Change ISBN number:");
            Console.WriteLine("7. Return to Menu");

            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
            {
                BooksInLibrary.Remove(book.Isbn);
            }
            else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2)
            {
                Console.Clear();
                Console.WriteLine("Please enter how many copies you want to remove:");

                (int removeAmount, terminate) = _validateInputs.GetValidInput(_validateInputs.IsInteger, _validateInputs.ConverToInteger, "Invalid Input. Enter the number of copies to remove or press enter to return to the menu:");

                if (terminate)
                    return;

                if (removeAmount <= book.CopiesOfTheBook)
                {
                    book.CopiesOfTheBook -= removeAmount;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("The number of copies you're trying to remove exceeds the available copies. Press any key to return to the menu.");
                    Console.ReadLine();
                }
            }
            else if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.NumPad3)
            {
                Console.Clear();
                Console.WriteLine("Please enter how many copies you want to add:");

                (int addAmount, terminate) = _validateInputs.GetValidInput(_validateInputs.IsInteger, _validateInputs.ConverToInteger, "Invalid Input. Enter the number of copies to add or press enter to return to the menu:");

                if (terminate)
                    return;

                book.CopiesOfTheBook += addAmount;
            }
            else if (key.Key == ConsoleKey.D4 || key.Key == ConsoleKey.NumPad4)
            {
                Console.Clear();
                Console.WriteLine("Please enter the new name for the book:");

                (string newBookName, terminate) = _validateInputs.GetValidInput(_validateInputs.NotNullOrWhiteSpace, _validateInputs.ReturnString, "Invalid input. Enter new book name again or press enter to return to the menu:");

                if (terminate)
                    return;

                book.BookName = newBookName;
            }
            else if (key.Key == ConsoleKey.D5 || key.Key == ConsoleKey.NumPad5)
            {
                Console.Clear();
                Console.WriteLine("Please enter the new author's name:");

                (string newAuthorName, terminate) = _validateInputs.GetValidInput(_validateInputs.NotNullOrWhiteSpace, _validateInputs.ReturnString, "Invalid input. Enter new author again or press enter to return to the menu:");

                if (terminate)
                    return;

                book.AuthorOfTheBook = newAuthorName;
            }
            else if (key.Key == ConsoleKey.D6 || key.Key == ConsoleKey.NumPad6)
            {
                Console.Clear();
                Console.WriteLine("Please enter the new ISBN for the book:");

                (string newISBN, terminate) = _validateInputs.GetValidInput(_validateInputs.IsIntegerDashesAndWhiteSpacesRemoved, _validateInputs.ReturnString, "Invalid input. Enter new ISBN again or press enter to return to the menu:");

                if (terminate)
                    return;

                book.Isbn = newISBN;
            }
            else if (key.Key == ConsoleKey.D7 || key.Key == ConsoleKey.NumPad7)
            {
                return;
            }
        }
        public void RentBook(bool needToSearch, Book book)
        {
            if (BooksInLibrary.Count == 0 && DuplicateBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No books found in the library. Press any key to return to the menu.");
                Console.ReadLine();
                return;
            }

            Book bookToRent = book;
            DateTime dateTime;

            bool terminate = false;

            if (needToSearch)
            {
                (List<Book> foundBooks, terminate) = _bookSearcher.FindBook(BooksInLibrary.Values);

                if (terminate)
                    return;

                if (foundBooks.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Couldn't find the book. Press any key to return to the menu");
                    Console.ReadLine();
                    return;
                }

                Console.Clear();
                Console.WriteLine("Please select a book from the list");

                for (int i = 0; i < foundBooks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}-BookName:{foundBooks[i].BookName} ISBN:{foundBooks[i].Isbn}");
                }

                ConsoleKeyInfo bookSelectKey = Console.ReadKey();

                if (char.IsDigit(bookSelectKey.KeyChar))
                {
                    if (int.Parse(bookSelectKey.KeyChar.ToString()) <= foundBooks.Count)
                    {
                        bookToRent = foundBooks[int.Parse(bookSelectKey.KeyChar.ToString()) - 1];
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Press any key to return to the menu");
                        Console.ReadLine();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Press any key to return to the menu");
                    Console.ReadLine();
                    return;
                }
            }

            Console.Clear();
            Console.WriteLine("Enter a date and time in the format 'yyyy-MM-dd HH:mm' or press any key to set the current time and date:");

            string userInput = Console.ReadLine();

            while (!_validateInputs.IsDateTime(userInput))
            {
                if (!_validateInputs.NotNullOrWhiteSpace(userInput))
                    break;

                Console.WriteLine("Invalid input. Try again or press enter to set the current time and date:");
                userInput = Console.ReadLine();
            }

            if (_validateInputs.NotNullOrWhiteSpace(userInput))
            {
                dateTime = DateTime.Parse(userInput);
            }
            else
            {
                dateTime = DateTime.Now;
            }

            Console.Clear();
            Console.WriteLine("Enter a due time in days:");

            (int addedDays, terminate) = _validateInputs.GetValidInput(_validateInputs.IsInteger, _validateInputs.ConverToInteger, "Invalid input. Enter due time in days again or press enter to return to the menu:");

            if (terminate)
                return;

            DateTime dueTime = dateTime.AddDays(addedDays);

            if (bookToRent.CopiesOfTheBook == 0)
            {
                BooksInLibrary.Remove(bookToRent.Isbn);
            }
            else
            {
                bookToRent.CopiesOfTheBook--;
            }

            Rent rentedBook = new Rent(bookToRent, dateTime, dueTime);
            RentedBooks.Add(rentedBook);
        }
        public void ReturnBook(bool needToSearch, Book returnedBook)
        {
            if (RentedBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("There is no rented books. Press any key to return to the menu");
                Console.ReadLine();
                return;
            }

            bool terminate = false;
            string isbn;

            if (needToSearch)
            {
                Console.Clear();
                Console.WriteLine("Please enter the ISBN of the book you want to return:");
                (isbn, terminate) = _validateInputs.GetValidInput(_validateInputs.IsIntegerDashesAndWhiteSpacesRemoved, _validateInputs.ReturnString, "Invalid input. Enter the ISBN again or press enter to return to the menu:");

                if (terminate)
                    return;
            }
            else
            {
                isbn = returnedBook.Isbn;
            }

            foreach (var rentedBook in RentedBooks)
            {
                if (rentedBook.RentedBook.Isbn == isbn)
                {
                    if (BooksInLibrary.TryGetValue(rentedBook.RentedBook.Isbn, out Book book))
                    {
                        book.CopiesOfTheBook++;
                    }
                    else
                    {
                        BooksInLibrary.Add(rentedBook.RentedBook.Isbn, rentedBook.RentedBook);
                    }
                    RentedBooks.Remove(rentedBook);
                    Console.Clear();
                    Console.WriteLine("Book return completed. Press any key to return to the menu.");
                    Console.ReadLine();
                    return;
                }
            }

            Console.Clear();
            Console.WriteLine("Book doesn't exist, or there is no rented book with this ISBN. Press any key to return to the menu.");
            Console.ReadLine();
        }
        private (Book, bool) CreateNewBook()
        {
            bool terminate = false;
            
            Console.Clear();
            Console.WriteLine("Please enter the name of the book:");
            (string bookName, terminate) = _validateInputs.GetValidInput(_validateInputs.NotNullOrWhiteSpace, _validateInputs.ReturnString, "Invalid input. Please enter the name of the book again or press enter to return to the menu");

            if (terminate)
                return (null, terminate);

            Console.Clear();
            Console.WriteLine("Please enter the name of the author:");
            (string author, terminate) = _validateInputs.GetValidInput(_validateInputs.NotNullOrWhiteSpace, _validateInputs.ReturnString, "Invalid input. Please enter the author's name again or press enter to return to the menu");

            if (terminate)
                return (null, terminate);
            
            Console.Clear();
            Console.WriteLine("Please enter the ISBN of the book:");
            (string isbn, terminate) = _validateInputs.GetValidInput(_validateInputs.IsIntegerDashesAndWhiteSpacesRemoved, _validateInputs.ReturnString, "Invalid input. Please enter the ISBN of the book again or press enter to return to the menu");

            if (terminate)
                return (null, terminate);

            Console.Clear();
            Console.WriteLine("Please enter the copy number of the book:");
            (int copyNumber, terminate) = _validateInputs.GetValidInput(_validateInputs.IsIntegerDashesAndWhiteSpacesRemoved, _validateInputs.ConvertToIntegerDashesAndWhiteSpacesRemoved, "Invalid input. Please enter the copy number of the book again or press enter to return to the menu");

            if (terminate)
                return (null, terminate);


            return (new Book(bookName, author, isbn, copyNumber), false);
        }
        private void DisplayBooksCollection(IEnumerable<Book> books, int[] minLengths, int[] maxLengths)
        {
            foreach (var book in books)
            {
                Console.WriteLine(_tableCreator.GenerateTableRow(minLengths, maxLengths, book.BookName, book.AuthorOfTheBook, book.Isbn, book.CopiesOfTheBook.ToString()));
                Console.WriteLine(_tableCreator.GenerateHorizontalLine(minLengths, maxLengths));
            }
        }
        private void AddDuplicateBook(Book duplicateBook)
        {
            DuplicateBooks.Add(duplicateBook);
        }
        private static void DisplayDuplicateBook(Book duplicateBook)
        {
            Console.WriteLine("\nDuplicate Book Information:");
            Console.WriteLine($"Book Name: {duplicateBook.BookName}");
            Console.WriteLine($"Author: {duplicateBook.AuthorOfTheBook}");
            Console.WriteLine($"ISBN: {duplicateBook.Isbn}");
        }
    }
}

