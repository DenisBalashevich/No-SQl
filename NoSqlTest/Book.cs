using MongoDB.Bson;

namespace DAL
{
    public class Book
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }

        public string Author { get; set; }

        public int Count { get; set; }

        public int Year { get; set; }

        public string[] Genre { get; set; }
    }
}
