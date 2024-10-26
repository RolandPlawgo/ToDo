using ToDo.Models;

namespace ToDo.Services
{
    public interface IToDoService
    {
        public Task<List<ToDoModel>> GetAllTodos();
        public Task<ToDoModel?> GetTodo(int id);
        /// <summary>
        /// Gets incoming ToDo's for the current day
        /// </summary>
        /// <returns>A list of all ToDo's for the current day</returns>
        public Task<List<ToDoModel>> GetIncomingToDosForCurrentDay();

        /// <summary>
        /// Gets incoming ToDo's for the next day
        /// </summary>
        /// <returns>A list of all ToDo's for the next day</returns>
        public Task<List<ToDoModel>> GetIncomingToDosForNextDay();

        /// <summary>
        /// Gets incoming ToDo's for the current week
        /// </summary>
        /// <returns>A list of all ToDo's for the current week</returns>
        public Task<List<ToDoModel>> GetToDosForCurrentWeek();
        public Task CreateToDo(ToDoModel model);
        /// <summary>
        /// Updates the Todo based on the <paramref name="model"/> parameter. The Todo to update is found based on the Id of the provided Todo.
        /// </summary>
        /// <param name="model">The model to be updated. The Id property should not be changed.</param>
        public Task UpdateToDo(ToDoModel model);
        /// <summary>
        /// Sets the percent complete to the value of the specified <paramref name="percentComplete"/> parameter
        /// </summary>
        /// <param name="id">The id of the todo to be updated</param>
        /// <param name="percentComplete"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="percentComplete"/> parameter is grater than 0 or smaller than 100</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the todo can not be found</exception>
        public Task SetToDoPercentComplete(int id, int percentComplete);
        public Task DeleteToDo(int id);
        /// <summary>
        /// Sets the PercentComplete value of the todo to 100
        /// </summary>
        /// <param name="id">The id of the todo to be updated</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Thrown when the todo can not be found</exception>
        public Task CompleteToDo(int id);
    }
}