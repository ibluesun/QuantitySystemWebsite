using Qs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuantitySystem.org
{
    public class QsWebStorageProvider : IQsStorageProvider
    {
        static Dictionary<Guid, Dictionary<string, object>> QsDictionaries = new Dictionary<Guid, Dictionary<string, object>>();

        const string QsCookieKey = "ServerQs";

        public HttpCookie QsCookie
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[QsCookieKey] == null)
                {
                    // new browser make a new coockie with the key
                    var c = new HttpCookie(QsCookieKey, Guid.NewGuid().ToString());
                    c.Expires = DateTime.Today.AddDays(1);
                    c.HttpOnly = true;

                    HttpContext.Current.Response.Cookies.Add(c);
                    return c;
                }

                return HttpContext.Current.Request.Cookies[QsCookieKey];
            }
        }

        public Dictionary<string, object> BrowserDictionary
        {
            get
            {
                var g = new Guid(QsCookie.Value);

                // now we have the guid number 

                // we want to get the values from the memory for this guid number

                Dictionary<string, object> bDictionary;

                if (!QsDictionaries.TryGetValue(g, out bDictionary))
                {
                    bDictionary = new Dictionary<string, object>();
                    QsDictionaries.Add(g, bDictionary);
                }
                return bDictionary;
            }
        }

        public void Clear()
        {
            BrowserDictionary.Clear();
        }

        public bool DeleteValue(string variable)
        {
            return BrowserDictionary.Remove(variable);
        }

        public IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            return BrowserDictionary.AsEnumerable();
        }

        public IEnumerable<string> GetKeys()
        {
            return BrowserDictionary.Keys.AsEnumerable();
        }

        public object GetValue(string variable)
        {
            return BrowserDictionary[variable];
        }

        public IEnumerable<object> GetValues()
        {
            return BrowserDictionary.Values.AsEnumerable();
        }

        public bool HasValue(string variable)
        {
            return BrowserDictionary.ContainsKey(variable);
        }

        public void SetValue(string variable, object value)
        {
            BrowserDictionary[variable] = value;
        }

        public bool TryGetValue(string variable, out object q)
        {
            return BrowserDictionary.TryGetValue(variable, out q);
        }

        public void Dispose()
        {
            
        }
    }
}