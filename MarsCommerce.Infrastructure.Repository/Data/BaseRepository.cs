using MarsCommerce.Core.Interfaces;
using MarsCommerce.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MarsCommerce.Infrastructure.Repository.Data
{
    public class BaseRepository<TBaseEntity> : IRepository<TBaseEntity> where TBaseEntity : BaseEntity
    {
        private AppDbContext _dbContext;
        public BaseRepository(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> filter)
        {
            return await _dbContext.Set<TBaseEntity>().Where(filter).FirstOrDefaultAsync();
        }


        public async Task<TBaseEntity?> GetAsync(int? id)
        {
            return await _dbContext.Set<TBaseEntity>().FindAsync(id);
        }
        public async Task<List<TBaseEntity>> GetAllByAsync(Expression<Func<TBaseEntity, bool>> filter)
        {
            return await _dbContext.Set<TBaseEntity>().Where(filter).ToListAsync();
        }
        public async Task<TBaseEntity> AddAsync(TBaseEntity entity)
        {
            await _dbContext.Set<TBaseEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TBaseEntity>> AddRangeAsync(List<TBaseEntity> entities)
        {
            await _dbContext.Set<TBaseEntity>().AddRangeAsync(entities);
            await  _dbContext.SaveChangesAsync();
            return entities;
        }

        public async Task<TBaseEntity> DeleteAsync(TBaseEntity entity)
        {
            _dbContext.Set<TBaseEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
       
        public async Task<TBaseEntity> UpdateAsync(TBaseEntity entity)
        {
            _dbContext.Set<TBaseEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TBaseEntity>> UpdateRangeAsync(List<TBaseEntity> entities)
        {
            _dbContext.Set<TBaseEntity>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
            return entities;
        }

        public async Task<TBaseEntity> DeleteCartAsync(int id)
        {
            TBaseEntity baseEntity=  await _dbContext.Set<TBaseEntity>().FindAsync(id);

            _dbContext.Set<TBaseEntity>().Remove(baseEntity);
            await _dbContext.SaveChangesAsync();
            return baseEntity;
        }

    }
}