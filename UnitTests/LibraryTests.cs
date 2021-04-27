using Library.Controllers;
using Library.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Library.Tests
{
    [TestFixture]
    public class LibraryTests
    {
        private const string SAMPLE_TEXT = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
                Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate 
                velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        public LibraryTests()
        {
        }

        [Test]
        public void MostCommonWordsTest()
        {
            var controller = new BooksController();

            var result = controller.Getbooks() as List<BookDTO>;
            var testBooks = new List<BookDTO>();
            testBooks.Add(new BookDTO { Id = 1,Title= "A Tale Of Two Cities.txt",Path = ""});
            testBooks.Add(new BookDTO { Id = 2, Title = "Moby Dick.txt", Path = "" });
            testBooks.Add(new BookDTO { Id = 3, Title = "Moby Dick.txt", Path = "" });
            Assert.AreEqual(testBooks.Count, result.Count);
          
        }

        [Test]
        public void SearchTest()
        {
             Assert.Fail();
        }
    }
}