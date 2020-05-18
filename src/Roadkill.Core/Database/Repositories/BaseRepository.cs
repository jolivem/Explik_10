using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
//using ContosoUniversity.Models;
using System.Linq.Expressions;
using Roadkill.Core.Database.Repositories.Entities;


namespace Roadkill.Core.Database.Repositories
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        internal Entities.Entities context;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(Entities.Entities context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet; ;
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void DeleteAll()
        {
            Delete(dbSet);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == System.Data.Entity.EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filterExpression) 
        {
            return dbSet.FirstOrDefault(filterExpression);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;
        }
    }
}

