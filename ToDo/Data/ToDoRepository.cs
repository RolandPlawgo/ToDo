using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using ToDo.Helpers;
using ToDo.Models;

namespace ToDo.Data
{
    public class ToDoRepository : IToDoRepository, IDisposable
    {
        private bool disposed;
        private readonly ApplicationDbContext _dbContext;
        
        public ToDoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ToDoModel>> GetAllToDosAsync()
        {
            return await _dbContext.ToDos.ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<ToDoModel?> GetToDoByIdAsync(int id)
        {
            return await _dbContext.ToDos.FindAsync(id);
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<ToDoModel>> GetToDosByDateAsync(DateTime minDate, DateTime maxDate)
        {
            minDate = minDate.SetKindUtc();
            maxDate = maxDate.SetKindUtc();
            return await _dbContext.ToDos.Where(x => x.DateAndTimeOfExpiry > minDate && x.DateAndTimeOfExpiry < maxDate).ToListAsync();
        }

        public async Task InsertToDoAsync(ToDoModel toDo)
        {
            toDo.DateAndTimeOfExpiry = toDo.DateAndTimeOfExpiry.SetKindUtc();
            await _dbContext.AddAsync(toDo);
        }

        public void UpdateToDo(ToDoModel toDo)
        {
            _dbContext.Update(toDo);
        }

        public async Task DeleteToDoAsync(int id)
        {
            ToDoModel? toDo = await _dbContext.ToDos.FindAsync(id);
            if (toDo is null)
                throw new KeyNotFoundException();
            _dbContext.ToDos.Remove(toDo);
        }

        /// <inheritdoc/>
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
