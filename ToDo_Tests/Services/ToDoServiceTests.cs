using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services;
using Xunit;

namespace ToDo_Tests.Services
{
    public class ToDoServiceTests
    {
        private readonly Mock<IToDoRepository> _mockRepository;
        private readonly ToDoService _toDoService;

        public ToDoServiceTests()
        {
            _mockRepository = new Mock<IToDoRepository>();
            _toDoService = new ToDoService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllTodos_ReturnsListOfToDoModels()
        {
            // Arrange
            var expectedTodos = new List<ToDoModel>
            {
                new ToDoModel { Id = 1, Title = "Task 1", Description = "Desc 1", PercentComplete = 0, DateAndTimeOfExpiry = DateTime.Now },
                new ToDoModel { Id = 2, Title = "Task 2", Description = "Desc 2", PercentComplete = 50, DateAndTimeOfExpiry = DateTime.Now.AddDays(1) }
            };
            _mockRepository.Setup(repo => repo.GetAllToDosAsync()).ReturnsAsync(expectedTodos);

            // Act
            var result = await _toDoService.GetAllTodos();

            // Assert
            Assert.Equal(expectedTodos, result);
        }

        [Fact]
        public async Task GetTodo_ReturnsToDoModel_WhenToDoExists()
        {
            // Arrange
            var expectedTodo = new ToDoModel { Id = 1, Title = "Task", Description = "Desc", PercentComplete = 0, DateAndTimeOfExpiry = DateTime.Now };
            _mockRepository.Setup(repo => repo.GetToDoByIdAsync(1)).ReturnsAsync(expectedTodo);

            // Act
            var result = await _toDoService.GetTodo(1);

            // Assert
            Assert.Equal(expectedTodo, result);
        }

        [Fact]
        public async Task GetTodo_ReturnsNull_WhenToDoNotFound()
        {
            // Arrange
            int id = 1;
            _mockRepository.Setup(repo => repo.GetToDoByIdAsync(id)).ReturnsAsync((ToDoModel?)null);

            // Act
            var result = await _toDoService.GetTodo(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetIncomingToDosForCurrentDay_ReturnsToDosForToday()
        {
            // Arrange
            var today = DateTime.Now.Date;
            var expectedTodos = new List<ToDoModel>
            {
                new ToDoModel { Id = 1, Title = "Task 1", DateAndTimeOfExpiry = today.AddHours(10) },
                new ToDoModel { Id = 2, Title = "Task 2", DateAndTimeOfExpiry = today.AddHours(12) }
            };
            _mockRepository.Setup(repo => repo.GetToDosByDateAsync(today, today.AddDays(1))).ReturnsAsync(expectedTodos);

            // Act
            var result = await _toDoService.GetIncomingToDosForCurrentDay();

            // Assert
            Assert.Equal(expectedTodos, result);
        }

        [Fact]
        public async Task CreateToDo_CallsInsertAndSaveMethods()
        {
            // Arrange
            var newToDo = new ToDoModel { Id = 1, Title = "New Task", Description = "New Desc", PercentComplete = 0, DateAndTimeOfExpiry = DateTime.Now };
            _mockRepository.Setup(repo => repo.InsertToDoAsync(newToDo)).Returns(Task.CompletedTask);

            // Act
            await _toDoService.CreateToDo(newToDo);

            // Assert
            _mockRepository.Verify(repo => repo.InsertToDoAsync(newToDo), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateToDo_CallsUpdateAndSaveMethods()
        {
            // Arrange
            var updatedToDo = new ToDoModel { Id = 1, Title = "Updated Task", Description = "Updated Desc", PercentComplete = 50, DateAndTimeOfExpiry = DateTime.Now };
            _mockRepository.Setup(repo => repo.UpdateToDo(updatedToDo)).Verifiable();
            _mockRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _toDoService.UpdateToDo(updatedToDo);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateToDo(updatedToDo), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteToDo_CallsDeleteAndSaveMethods()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeleteToDoAsync(1)).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _toDoService.DeleteToDo(1);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteToDoAsync(1), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 50)]
        [InlineData(1, 100)]
        public async Task SetToDoPercentComplete_UpdatesPercentComplete_WhenValid(int id, int percentComplete)
        {
            // Arrange
            var toDo = new ToDoModel { Id = id, Title = "Task", PercentComplete = 0 };
            _mockRepository.Setup(repo => repo.GetToDoByIdAsync(id)).ReturnsAsync(toDo);
            _mockRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _toDoService.SetToDoPercentComplete(id, percentComplete);

            // Assert
            Assert.Equal(percentComplete, toDo.PercentComplete);
            _mockRepository.Verify(repo => repo.UpdateToDo(toDo), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Theory]
        [InlineData(1, -10)]
        [InlineData(1, 150)]
        public async Task SetToDoPercentComplete_ThrowsArgumentOutOfRangeException_WhenPercentInvalid(int id, int percentComplete)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _toDoService.SetToDoPercentComplete(id, percentComplete));
        }

        [Fact]
        public async Task SetToDoPercentComplete_ThrowsKeyNotFoundException_WhenToDoNotFound()
        {
            // Arrange
            int id = 1;
            _mockRepository.Setup(repo => repo.GetToDoByIdAsync(id)).ReturnsAsync((ToDoModel?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _toDoService.SetToDoPercentComplete(id, 50));
        }

        [Fact]
        public async Task CompleteToDo_SetsPercentCompleteTo100_WhenToDoExists()
        {
            // Arrange
            int id = 1;
            var toDo = new ToDoModel { Id = id, Title = "Task", PercentComplete = 0 };
            _mockRepository.Setup(repo => repo.GetToDoByIdAsync(id)).ReturnsAsync(toDo);
            _mockRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _toDoService.CompleteToDo(id);

            // Assert
            Assert.Equal(100, toDo.PercentComplete);
            _mockRepository.Verify(repo => repo.UpdateToDo(toDo), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CompleteToDo_ThrowsKeyNotFoundException_WhenToDoNotFound()
        {
            // Arrange
            int id = 1;
            _mockRepository.Setup(repo => repo.GetToDoByIdAsync(id)).ReturnsAsync((ToDoModel?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _toDoService.CompleteToDo(id));
        }
    }
}
