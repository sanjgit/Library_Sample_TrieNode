using Library.DataLayer;
using Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Web;

namespace Library.Repositories
{
    public static class ServiceRepository
    {
        
        public static int Num_Of_Common_Words = 10;
        public static void SeedData()
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (!memoryCache.Contains("ListOfBooks") || !memoryCache.Contains("FileTrieNodes"))
            {


                string[] fileArray = Directory.GetFiles(HttpContext.Current.Server.MapPath(("~/Resources")));
                List<BookDTO> lstDto = new List<BookDTO>();

                CacheItemPolicy policy = new CacheItemPolicy();
                int bookId = 0;
                foreach (var txtFile in fileArray)
                {
                    BookDTO bookObj = new BookDTO();
                    bookObj.Title = Path.GetFileNameWithoutExtension(txtFile) + Path.GetExtension(txtFile);
                    bookObj.Id = ++bookId;
                    bookObj.Path = txtFile;

                    lstDto.Add(bookObj);
                }
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(fileArray));
                memoryCache.Add("ListOfBooks", lstDto, DateTimeOffset.UtcNow.AddMinutes(10));

                List<string> searchedList = new List<string>();
                Dictionary<DataReader, Thread> readers = new Dictionary<DataReader, Thread>();
                if (fileArray.Length > 0)
                {
                    foreach (BookDTO eachTextFile in lstDto)
                    {
                        TrieNode eachRoot = new TrieNode(null, '?');
                        DataReader new_reader = new DataReader(eachTextFile.Path, eachTextFile.Id, ref eachRoot);
                        Thread new_thread = new Thread(new_reader.ThreadRun);
                        readers.Add(new_reader, new_thread);
                        new_thread.Start();
                    }
                }
                //string path = lstDto.FirstOrDefault(o => o.Id == 2).ToString();

                foreach (Thread t in readers.Values) t.Join();

                // store the trie node content into the cache with reader objects
                memoryCache.Add("FileTrieNodes", readers, DateTimeOffset.UtcNow.AddMinutes(10));
            }
           
        }
    }
}