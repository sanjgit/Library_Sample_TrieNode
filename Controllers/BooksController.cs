﻿using Library.DataLayer;
using Library.Models;
using Library.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Library.Controllers
{
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        List<BookDTO> lstDto;
        /*
		The following GET methods are expected on the api/books controller:

			1. GET api/books
				Returns a list of Ids & Titles for all the books in the Resources folder
				Titles should be the name of the filename, minus the extension.
				The Id should be unique.

			2. GET api/books/{Id}
				Returns a list of the most common 10 words (min 5 letters) and how many times they occur in the specified book.
				When parsing, whitespace, linefeeds and punctiation should be ignored, and words matched case-insensitively (e.g. "the" and "The" and "THE" will be returned as "The"=3)
				Words should be returned in capital case (e.g. Word), and the list should be sorted in decreasing incidence.

			3. GET api/books/{Id}?query={query}
				Returns a list of all words which start with the specified string (min 3 letters) and the number of times they occur within the specified book.
				Case matching should be insensitive.

		The logic for these calls should largely be encapsulated in other classes. This should make it easier to Unit Test those classes
		to validate the expected behaviour.
		*/
        // GET api/Books
        public IEnumerable<BookDTO> Getbooks()
		{
			BooksRepository br = new BooksRepository();
			return br.GetBooks();
		}
		// GET api/Books/id
		public IEnumerable<MostCommonWords> Getbooks(int id)
		{
			BooksRepository br = new BooksRepository();
			IEnumerable<MostCommonWords> top10CommonWords = br.GetTopCommonWords(id).TopCommonWords;

			
			return top10CommonWords;
		}

		//GET api/Books/id?query=""
		public IEnumerable<string> Getbooks(int id, string query)
        {
			BooksRepository br = new BooksRepository();
			return br.searchByString(id, query);

		}
	}
}
