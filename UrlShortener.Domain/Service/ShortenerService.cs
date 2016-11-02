using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrlShortener.Domain.Model;

namespace UrlShortener.Domain.Service
{
    public class ShortenerService : IShortenerService
    {
        readonly IRepository repository;

        public ShortenerService(IRepository repository)
        {
            this.repository = repository;
        }


        public string CreateUrlIdentifier(string url)
        {
            var urlEntity = (from u in repository.Urls
                             where u.Value == url
                             select u
                                ).FirstOrDefault();

            if (urlEntity == null)
            {
                int Id = repository.Urls.Count() + 1;
                urlEntity = new Url() {Id =Id,  Value = url, HitCount = 1 };
                if (CheckDomain(url))
                    repository.InsertUrl(urlEntity);
                else
                    throw new Exception("Not a valid Domain");
            }

            return IdEncoder.Encode(urlEntity.Id);
        }

        private Boolean CheckDomain(string strurl)
        {
            if (!string.IsNullOrEmpty(strurl) && strurl.Contains("mydeal.com.au"))            
                return true;
            else            
                return false;
        }

        public string ResolveUrlIdentifier(string urlShort)
        {
            var id = IdEncoder.Decode(urlShort);
            var urlEntity = (from u in repository.Urls
                             where u.Id == id
                             select u
                    ).FirstOrDefault();

            if (urlEntity == null)
            {
                throw new Exception("Id not found");
            }
            return urlEntity.Value;
        }


        public void UpdateUrlStatistics(string encodedId, System.Collections.Specialized.NameValueCollection nameValueCollection)
        {
            repository.IncreaseUrlHitCount(IdEncoder.Decode(encodedId));   
        }
    }
}
