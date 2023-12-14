namespace EFdigitalLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }

        public Book( string name, string author, string genre, DateTime releaseDate)
        {
            Name = name;
            Author = author;
            Genre = genre;
            ReleaseDate = releaseDate;
            
        }
    }
}
