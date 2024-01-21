namespace Library
{
    internal class Program
    {

        static void Main(string[] args)
        {
            LibraryManager Library = new LibraryManager();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Please select the action you want to perform:");
                Console.WriteLine("1: List All Books");
                Console.WriteLine("2: List Rented Books");
                Console.WriteLine("3: Add New Book");
                Console.WriteLine("4: Search for a Book by Name, Author or ISBN");
                Console.WriteLine("5: Rent a Book");
                Console.WriteLine("6: Return a Book");
                Console.WriteLine("7: Remove Duplicate Books from Library");
                Console.WriteLine("Press Escape to exit the application.");

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.D1)
                {
                    Library.DisplayBooksInLibrary();
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    Library.DisplayRentedBooks();
                }
                else if (key.Key == ConsoleKey.D3)
                {
                    Library.AddNewBook();
                }
                else if (key.Key == ConsoleKey.D4)
                {
                    Library.SearchBook();
                }
                else if (key.Key == ConsoleKey.D5)
                {
                    Library.RentBook(true, null);
                }
                else if (key.Key == ConsoleKey.D6)
                {
                    Library.ReturnBook(true, null);
                }
                else if (key.Key == ConsoleKey.D7)
                {
                    Library.RemoveDuplicateBooks();
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Library.SaveData();
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please try again.");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}