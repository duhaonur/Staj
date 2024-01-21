namespace Library
{
    [Serializable]
    internal class Book
    {
        public string BookName;
        public string AuthorOfTheBook;
        public string Isbn;
        public int CopiesOfTheBook;

        public Book(string bookName, string author, string isbn, int copyNumber)
        {
            BookName = bookName;
            AuthorOfTheBook = author;
            Isbn = isbn;
            CopiesOfTheBook = copyNumber;
        }
    }
}
