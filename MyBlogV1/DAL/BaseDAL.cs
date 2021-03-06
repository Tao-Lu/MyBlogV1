﻿using IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BaseDAL<T> : IBaseDAL<T> where T: class, new()
    {
        public DbContext dbContext
        {
            get
            {
                return DbContextFactory.CreateDbContext();
            }
        }

        public void AddEntity(T entity)
        {
            // dbContext.Set<T>().Add(entity);
            dbContext.Entry<T>(entity).State = EntityState.Added;
        }

        public void DeleteEntity(T entity)
        {
            // dbContext.Set<T>().Remove(entity);
            dbContext.Entry<T>(entity).State = EntityState.Deleted;
        }

        public void EditEntity(T entity)
        {
            dbContext.Entry<T>(entity).State = EntityState.Modified;
        }

        public IQueryable<T> GetEntities(Expression<Func<T, bool>> whereExpression)
        {
            return dbContext.Set<T>().Where<T>(whereExpression);
        }

        // checks result is null or not
        public T GetEntity(Expression<Func<T, bool>> firstOrDefaultExpression)
        {
            return dbContext.Set<T>().FirstOrDefault<T>(firstOrDefaultExpression);
        }

        public IQueryable<T> GetEntitiesByPageOrdered<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereExpression, Expression<Func<T, s>> orderByExpression, bool isAsc)
        {
            var entitiesByPage = dbContext.Set<T>().Where<T>(whereExpression);
            totalCount = entitiesByPage.Count();

            if (isAsc)
            {
                entitiesByPage = entitiesByPage.OrderBy<T, s>(orderByExpression).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                entitiesByPage = entitiesByPage.OrderByDescending<T, s>(orderByExpression).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }

            return entitiesByPage;
        }
    }
}
