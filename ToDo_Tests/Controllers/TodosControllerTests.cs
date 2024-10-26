using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Controllers;
using ToDo.Models;
using ToDo.Services;
using Xunit;

namespace ToDo_Tests.Controllers
{
    public class TodosControllerTests
    {
        private readonly Mock<IToDoService> _mockToDoService;
        private readonly Mock<ILogger<TodosController>> _mockLogger;
        private readonly TodosController _controller;

        public TodosControllerTests()
        {
            _mockToDoService = new Mock<IToDoService>();
            _mockLogger = new Mock<ILogger<TodosController>>();
            _controller = new TodosController(_mockToDoService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfToDoModels()
        {
            // Arrange
            var todos = new List<ToDoModel> { new ToDoModel { Id = 1, Title = "Task 1", Description = "Description 1", PercentComplete = 50, DateAndTimeOfExpiry = DateTime.Now } };
            _mockToDoService.Setup(service => service.GetAllTodos()).ReturnsAsync(todos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(todos, okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsBadRequest_OnException()
        {
            // Arrange
            _mockToDoService.Setup(service => service.GetAllTodos()).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetSpecificTodo_ReturnsOkResult_WithToDoModel()
        {
            // Arrange
            var todo = new ToDoModel { Id = 1, Title = $"Task", Description = $"Task", PercentComplete = 75, DateAndTimeOfExpiry = DateTime.Now };
            _mockToDoService.Setup(service => service.GetTodo(1)).ReturnsAsync(todo);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(todo, okResult.Value);
        }

        [Fact]
        public async Task GetSpecificTodo_ReturnsNotFound_WhenTodoNotFound()
        {
            // Arrange
            _mockToDoService.Setup(service => service.GetTodo(1)).ReturnsAsync((ToDoModel?)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetSpecificTodo_ReturnsBadRequest_OnException()
        {
            // Arrange
            _mockToDoService.Setup(service => service.GetTodo(1)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Post_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            var todo = new ToDoModel { Title = "New Task", Description = "New Description", PercentComplete = 0, DateAndTimeOfExpiry = DateTime.Now.AddDays(1) };
            _mockToDoService.Setup(service => service.CreateToDo(todo)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(todo);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_OnException()
        {
            // Arrange
            var todo = new ToDoModel { Title = "New Task", Description = "New Description", PercentComplete = 0, DateAndTimeOfExpiry = DateTime.Now.AddDays(1) };
            _mockToDoService.Setup(service => service.CreateToDo(todo)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Post(todo);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            var todo = new ToDoModel { Id = 1, Title = "Updated Task", Description = "Updated Description", PercentComplete = 100, DateAndTimeOfExpiry = DateTime.Now.AddHours(2) };
            _mockToDoService.Setup(service => service.UpdateToDo(todo)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(todo);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_OnException()
        {
            // Arrange
            var todo = new ToDoModel { Id = 1, Title = "Updated Task", Description = "Updated Description", PercentComplete = 100, DateAndTimeOfExpiry = DateTime.Now.AddHours(2) };
            _mockToDoService.Setup(service => service.UpdateToDo(todo)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Put(todo);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Complete_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            _mockToDoService.Setup(service => service.CompleteToDo(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Complete(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Complete_ReturnsBadRequest_OnException()
        {
            // Arrange
            _mockToDoService.Setup(service => service.CompleteToDo(1)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Complete(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SetPercentComplete_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            _mockToDoService.Setup(service => service.SetToDoPercentComplete(1, 50)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SetPercentComplete(1, 50);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SetPercentComplete_ReturnsBadRequest_OnException()
        {
            // Arrange
            _mockToDoService.Setup(service => service.SetToDoPercentComplete(1, 50)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.SetPercentComplete(1, 50);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            _mockToDoService.Setup(service => service.DeleteToDo(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_OnException()
        {
            // Arrange
            _mockToDoService.Setup(service => service.DeleteToDo(1)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
