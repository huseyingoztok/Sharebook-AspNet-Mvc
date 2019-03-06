using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace ShareBook.WebApp.Helpers
{
    public class FileHelper
    {
        public string SaveImage(HttpPostedFileBase file, string path, string url)
        {
            FileInfo info = new FileInfo(file.FileName);
            var filename = Guid.NewGuid().ToString() + info.Extension;
            path = Path.Combine(path, filename);
            file.SaveAs(path);
            WebImage img = new WebImage(file.InputStream);

            img.Save(path);

            return url + "/" + filename;
        }
    }
}