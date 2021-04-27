using Library.DataLayer;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Library.Repositories
{
    public class BooksRepository
    {
        MemoryCache memoryCache = MemoryCache.Default;

        public List<BookDTO> GetBooks()
        {
            if (!memoryCache.Contains("ListOfBooks")){
                ServiceRepository.SeedData();
            }
            List<BookDTO>  lstTextBooks = (List<BookDTO>)memoryCache.GetCacheItem("ListOfBooks").Value;

            return lstTextBooks;
        }
        public BookTextResult GetTopCommonWords(int fileId)
        {
            List<BookTextResult> lstBookTextResult = new List<BookTextResult>();
            //check if filenodes in cache
            if (!memoryCache.Contains("FileTrieNodes"))
            {
                ServiceRepository.SeedData();
            }
            if (memoryCache.Contains("BookTextCounts"))
            {
                lstBookTextResult = (List<BookTextResult>)memoryCache.GetCacheItem("BookTextCounts").Value;
                foreach (BookTextResult BTR in lstBookTextResult)
                {
                    if (BTR.id == fileId)
                    {
                        return BTR;
                    }
                }
            }
            Dictionary<DataReader, Thread> readers  =(Dictionary < DataReader, Thread >) memoryCache.GetCacheItem("FileTrieNodes").Value;
            TrieNode root = new TrieNode(null, '?');
            memoryCache.Remove("BookTextCounts");
            foreach (DataReader dr in readers.Keys)
            {
                dr.GetType();
                if (dr.m_pathId==fileId)
                {
                    root = dr.m_root;
                }
            }
            List <TrieNode> top_nodes = new List<TrieNode>();
            for (int i = 0; i < ServiceRepository.Num_Of_Common_Words; i++)
            {
                top_nodes.Add(root);
            }
            int distinct_word_count = 0;
            int total_word_count = 0;
            List<string> lst_distinct_words = new List<string>();
            root.GetTopCounts(ref top_nodes, ref distinct_word_count, ref total_word_count, ref lst_distinct_words);
            top_nodes.Reverse();
            List<MostCommonWords> lstMostCommonWords = new List<MostCommonWords>();
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            foreach (TrieNode node in top_nodes)
            {
                MostCommonWords mcw = new MostCommonWords();
                mcw.Word_Name = myTI.ToTitleCase(node.ToString());
                mcw.Count_Of_Occurance = node.m_word_count;
                lstMostCommonWords.Add(mcw);
            }
            lst_distinct_words.Sort();
            
            BookTextResult bookCountResult = new BookTextResult();
            bookCountResult.id = fileId;
            bookCountResult.TopCommonWords = lstMostCommonWords;
            bookCountResult.TotalWordsCount = total_word_count;
            bookCountResult.DistinctWords = lst_distinct_words;
            lstBookTextResult.Add(bookCountResult);
            var cacheItemPolicy = new CacheItemPolicy(){ AbsoluteExpiration = DateTime.Now.AddDays(1)};
            memoryCache.Add("BookTextCounts", lstBookTextResult, cacheItemPolicy);
    
            return bookCountResult;
        }
        public List<string> searchByString(int fileId, string searchQuery)
        {
            List<BookTextResult> lstBookTextResult;
            BookTextResult bookTestRes=new BookTextResult();
            if (memoryCache.Contains("BookTextCounts"))
            {
                lstBookTextResult = (List<BookTextResult>)memoryCache.GetCacheItem("BookTextCounts").Value;
                foreach (BookTextResult BTR in lstBookTextResult)
                {
                    if (BTR.id == fileId)
                    {
                        bookTestRes = BTR;
                    }
                }
            }
            else
            {
                bookTestRes=GetTopCommonWords(fileId);
            }
            var resultList = bookTestRes.DistinctWords.Where(r => r.StartsWith(searchQuery.ToLower())).ToList();
            return resultList;

        }
    }
}