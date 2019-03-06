using ShareBook.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.DAL.EntityFramework
{
    public class myInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
       
        protected override void Seed(DatabaseContext context)
        {
            
        }

        
        
    }
}
