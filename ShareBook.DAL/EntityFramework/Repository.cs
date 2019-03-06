using ShareBook.Common;
using ShareBook.Core.DataAccess;
using ShareBook.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.DAL.EntityFramework
{
    public class Repository<T>:RepositoryBase, IDataAccess<T> where T:class
    {
        private DbSet<T> _objectSet;
     
        public Repository()
        {
           
                _objectSet = context.Set<T>();
            
           
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            if (obj is MyEntitiesBase)
            {
                MyEntitiesBase o = obj as MyEntitiesBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedBy = App.common.getCurrentUsername(); //TODO: Buraya bakılacak
            }


            _objectSet.Add(obj);
            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntitiesBase)
            {
                MyEntitiesBase o = obj as MyEntitiesBase;
   
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = App.common.getCurrentUsername(); //TODO: Buraya bakılacak
            }
            return Save();
        }

        public int Delete(T obj)
        {
            if (obj is MyEntitiesBase)
            {
                MyEntitiesBase o = obj as MyEntitiesBase;
               
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = App.common.getCurrentUsername(); //TODO: Buraya bakılacak modified on created on incelenecek
            }

            _objectSet.Remove(obj);
            return Save();
        }


        public int Save()
        {
         
                return context.SaveChanges();
            
            
        }

        public T Find(Expression<Func<T,bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }




    }
}
