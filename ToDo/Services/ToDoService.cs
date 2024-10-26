using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ToDo.Data;
using ToDo.Helpers;
using ToDo.Models;

namespace ToDo.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _repository;
        public ToDoService(IToDoRepository repository) 
        {
            _repository = repository;
        }
        public async Task<List<ToDoModel>> GetAllTodos()
        {
            var toDos = await _repository.GetAllToDosAsync();
            return toDos.ToList();
        }
        public async Task<ToDoModel?> GetTodo(int id)
        {
            return await _repository.GetToDoByIdAsync(id);
        }
        /// <inheritdoc/>
        public async Task<List<ToDoModel>> GetIncomingToDosForCurrentDay()
        {
            var toDos = await _repository.GetToDosByDateAsync(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
            return toDos.ToList();
        }
        public async Task<List<ToDoModel>> GetIncomingToDosForNextDay()
        {
            var toDos = await _repository.GetToDosByDateAsync(DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2));
            return toDos.ToList();
        }
        public async Task<List<ToDoModel>> GetToDosForCurrentWeek()
        {
            var toDos = await _repository.GetToDosByDateAsync(DateHelper.StartOfWeek(DateTime.Now.Date), DateHelper.EndOfWeek(DateTime.Now.Date));
            return toDos.ToList();
        }
        public async Task CreateToDo(ToDoModel model)
        {
            await _repository.InsertToDoAsync(model);
            await _repository.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task UpdateToDo(ToDoModel model)
        {
            _repository.UpdateToDo(model);
            await _repository.SaveAsync();
        }
        public async Task DeleteToDo(int id)
        {
            await _repository.DeleteToDoAsync(id);
            await _repository.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task SetToDoPercentComplete(int id, int percentComplete)
        {
            if (percentComplete < 0 || percentComplete > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percentComplete));
            }

            var toDo = await _repository.GetToDoByIdAsync(id);
            if (toDo is null)
            {
                throw new KeyNotFoundException();
            }
            toDo!.PercentComplete = percentComplete;
            _repository.UpdateToDo(toDo);
            await _repository.SaveAsync();
        }
        /// <inheritdoc/>
        public async Task CompleteToDo(int id)
        {
            var toDo = await _repository.GetToDoByIdAsync(id);
            if (toDo is null)
            {
                throw new KeyNotFoundException();
            }
            toDo!.PercentComplete = 100;
            _repository.UpdateToDo(toDo);
            await _repository.SaveAsync();
        }
    }
}
