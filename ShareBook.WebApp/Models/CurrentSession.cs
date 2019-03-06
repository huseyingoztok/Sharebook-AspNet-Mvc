using ShareBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareBook.WebApp.Models
{
    public class CurrentSession
    {
        

        public static ShareBookUser shareBookUser 
        {
            get
            {
                return Get<ShareBookUser>("loginOk");
            }
           
        }

        public static ShareBookUser shareBookUserAdmin
        {
            get
            {
                return Get<ShareBookUser>("LoginAdmin");
            }

        }

        public static void Set<T>(string key,T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }


        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                return (T)HttpContext.Current.Session[key];
            }

            return default(T); //Verdiğimiz türün default değerini dönderir (class ise null döner, int ise 0 döner...)
        }


        public static void RemoveSession(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                HttpContext.Current.Session.Remove(key);
            }

        }

        public static void ClearAllSessions()
        {
            HttpContext.Current.Session.Clear();
        }


    }
}