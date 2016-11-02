using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using UrlShortener.Domain.Model;

namespace UrlShortener.Domain
{
    public class SqlRepository : UrlShortener.Domain.IRepository
    {

        private Table<Url> urlsTable;
        private DataContext dc;

        public SqlRepository(string connectionString)
        {
            dc = new DataContext(connectionString);
            urlsTable = dc.GetTable<Url>();
        }

        public IQueryable<Url> Urls
        {
            get { return urlsTable; }
        }

        public Url InsertUrl(Url url)
        {            
            urlsTable.InsertOnSubmit(url);
            dc.ExecuteCommand("INSERT INTO urls (Id, url, hitcount) values ({0}, {1}, {2})", url.Id, url.Value, url.HitCount);            
            return url;
        }


        public void IncreaseUrlHitCount(int urlId)
        {
            dc.ExecuteCommand("UPDATE urls SET hitcount = hitcount + 1 WHERE id = {0} ", urlId);
        }
    }
}
