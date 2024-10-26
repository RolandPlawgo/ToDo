using ToDo.Models;

namespace ToDo.Data
{
    public interface IToDoRepository : IDisposable
    {
        public Task<IEnumerable<ToDoModel>> GetAllToDosAsync();
        /// <summary>
        /// Gets the Todo with id equal to the specified <paramref name="id"/> parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The todo found or null</returns>
        public Task<ToDoModel?> GetToDoByIdAsync(int id);
        /// <summary>
        /// Retrieves todos that have an expiry date later than the specified <paramref name="minDate"/> and earlier than the <paramref name="maxDate"/> parameter.
        /// </summary>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public Task<IEnumerable<ToDoModel>> GetToDosByDateAsync(DateTime minDate, DateTime maxDate);
        public Task InsertToDoAsync(ToDoModel toDo);
        public void UpdateToDo(ToDoModel toDo);
        public Task DeleteToDoAsync(int id);
        /// <summary>
        /// Has to be called after InsertToDo, UpdateToDo or DeleteToDo methods to save changes to the database.
        /// </summary>
        public Task SaveAsync();
    }
}
