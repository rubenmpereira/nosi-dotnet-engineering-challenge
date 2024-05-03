using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using NOS.Engineering.Challenge.Models;
using System.Collections.Concurrent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NOS.Engineering.Challenge.Database
{
    public class EntityFrameworkDatabase<TOut, TIn> : IDatabase<TOut, TIn> where TOut : class
    {
        private readonly IDbContextFactory<ContentDbContext> _contextFactory;
        private readonly IMapper<TOut?, TIn> _mapper;

        public EntityFrameworkDatabase(IMapper<TOut?, TIn> mapper, IDbContextFactory<ContentDbContext> contextFactory)
        {
            _mapper = mapper;
            _contextFactory = contextFactory;
        }

        public async Task<TOut?> Create(TIn item)
        {
            var id = Guid.NewGuid();
            var createdItem = _mapper.Map(id, item);

            if (createdItem is not null)
            {
                using var _context = _contextFactory.CreateDbContext();
                await _context.AddAsync(createdItem);
                await _context.SaveChangesAsync();
            }

            return createdItem;
        }

        public async Task<TOut?> Read(Guid id)
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.FindAsync<TOut>(id);
        }

        public async Task<IEnumerable<TOut?>> ReadAll()
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Set<TOut>().ToListAsync()!;
        }

        public async Task<TOut?> Update(Guid id, TIn item)
        {
            using var _context = _contextFactory.CreateDbContext();

            var dbItem = await _context.FindAsync<TOut>(id);
            if (dbItem is null)
            {
                return default;
            }
            var updatedItem = _mapper.Patch(dbItem, item);

            if (updatedItem is not null)
            {
                _context.SetModified(dbItem,updatedItem);
                await _context.SaveChangesAsync();
            }
            return updatedItem;
        }

        public async Task<Guid> Delete(Guid id)
        {
            using var _context = _contextFactory.CreateDbContext();
            var entity = await _context.FindAsync<TOut>(id);

            if (entity is null)
                return Guid.Empty;

            _context.Attach<TOut>(entity);
            _context.Remove<TOut>(entity);
            await _context.SaveChangesAsync();

            return id;
        }

    }
}
