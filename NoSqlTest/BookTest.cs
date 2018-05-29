using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NoSqlTest
{
    [TestClass]
    public class Tasks
    {
        private IMongoCollection<Book> _books;

        [TestInitialize]
        public void Setup()
        {
            var client = new MongoClient();
            var db = client.GetDatabase("Test");
            _books = db.GetCollection<Book>("Books");
            Initialize();
        }

        private void Initialize()
        {
            if (_books.Count(FilterDefinition<Book>.Empty) == 0)
            {
                _books.InsertMany(new[]
                {
                    new Book
                    {
                        Name = "Hobbit",
                        Author = "Tolkien",
                        Count = 5,
                        Genre = new[] { "fantasy" },
                        Year = 2014
                    },
                    new Book
                    {
                        Name = "Lord of the rings",
                        Author = "Tolkien",
                        Count = 3,
                        Genre = new[] { "fantasy" },
                        Year = 2015
                    },
                    new Book
                    {
                        Name = "Kolobok",
                        Count = 10,
                        Genre = new[] { "kids" },
                        Year = 2000
                    },
                    new Book
                    {
                        Name = "Repka",
                        Count = 11,
                        Genre = new[] { "kids" },
                        Year = 2000
                    },
                    new Book
                    {
                        Name = "Dyadya Stiopa",
                        Author = "Mihalkov",
                        Count = 1,
                        Genre = new[] { "kids" },
                        Year = 2001
                    }
                });
            }
        }

        [TestMethod]
        public void BooksWithCountMoreThanOne()
        {
            var booksWithCountMoreThanOne = _books.Find(b => b.Count > 1).ToList();
            Assert.AreEqual(4, booksWithCountMoreThanOne.Count);
        }

        [TestMethod]
        public void BooksWithMinAndMaxCount()
        {
            var MinCount = _books.Find(b => b.Count > 0).SortBy(b => b.Count).FirstOrDefault();
            Assert.AreEqual("Dyadya Stiopa", MinCount.Name);

            var MaxCount = _books.Find(b => b.Count > 0).SortByDescending(b => b.Count).FirstOrDefault();
            Assert.AreEqual("Repka", MaxCount.Name);
        }

        [TestMethod]
        public void AuthorList()
        {
            var authors = _books.Distinct<string>("Author", new BsonDocument()).ToList();
            Assert.AreEqual(3, authors.Count);
        }

        [TestMethod]
        public void BooksWithoutAuthor()
        {
            var booksWithoutAuthor = _books.Find(b => b.Author == null);
            Assert.AreEqual(2, booksWithoutAuthor.Count());
        }


        [TestMethod]
        public void RemoveBooksWithCountLess_3()
        {
            _books.DeleteMany(Builders<Book>.Filter.Lt(b => b.Count, 3));
            Assert.AreEqual(4, _books.Find(b => b.Count > 1).Count());
        }

        [TestMethod]
        public void RemoveAllBooks()
        {
            _books.DeleteMany(book => true);
            Assert.AreEqual(0, _books.Count(book=>true));
        }
    }
}
