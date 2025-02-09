using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        EfDbContext _context;

        public GenericRepository(EfDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> Get()
        {
            return _context.Set<TEntity>().AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _context.Set<TEntity>().AsNoTracking().Where(predicate).ToList();
        }

        public void Create(TEntity item)
        {
            _context.Set<TEntity>().Add(item);
        }
        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
        public void Remove(TEntity item)
        {
            _context.Set<TEntity>().Remove(item);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
