using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Users.Application.Repositories;
using Users.Core.Entities;
using Users.Infrastructure.Persistence;

namespace Users.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        internal ApplicationContext context;
        internal DbSet<TEntity> dbSet;

        public RepositoryBase(ApplicationContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query,
            params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters).ToList();
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

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
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

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {

            if (entityToUpdate == null)
            {
                throw new ArgumentException("entity");
            }
            if (context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                HandleDetached(entityToUpdate);
            }
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual bool IsInserted(TEntity entity)
        {
            return dbSet.Find(entity.Id) != null;
        }

        private bool HandleDetached(TEntity entity)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var entitySet = objectContext.CreateObjectSet<TEntity>();
            var entityKey = objectContext.CreateEntityKey(entitySet.EntitySet.Name, entity);
            object foundSet;
            bool exists = objectContext.TryGetObjectByKey(entityKey, out foundSet);
            if (exists)
            {
                objectContext.Detach(foundSet);
            }
            return exists;
        }
    }
}
