namespace Library
{
    [Serializable]
    internal class Rent
    {
        public Book RentedBook { get; private set; }
        public DateTime RentDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public Rent(Book rentedBook, DateTime rentDate, DateTime dueDate)
        {
            RentedBook = rentedBook;
            RentDate = rentDate;
            DueDate = dueDate;
        }

        public bool PassedDueTime()
        {
            return DateTime.Now > DueDate;
        }
    }
}
