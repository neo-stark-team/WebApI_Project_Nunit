using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Controllers;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

using System.Collections.Generic;
using System.Linq;

namespace dotnetapp.Tests
{
    public class ExpenseTrackerApiControllerTests
    {
        private ExpenseTrackerApiDbContext _context;
        private ExpenseTrackerController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerApiDbContext>()
                .UseInMemoryDatabase(databaseName: "ExpenseTrackerApiTest")
                .Options;

            _context = new ExpenseTrackerApiDbContext(options);

            _controller = new ExpenseTrackerController(_context);

            _context.ExpenseTrackerApis.AddRange(new List<ExpenseTrackerApi>
            {
                new ExpenseTrackerApi { Id = 1, Expense_Date = new DateTime(2023, 4, 20), Amount = 100, Category = "Goods", Description = "Exported charge", PaymentMethod = "PaymentMethod A" },
                new ExpenseTrackerApi { Id = 2, Expense_Date = new DateTime(2023, 5, 1), Amount = 500, Category = "Lunch", Description = "had lunch at KFC", PaymentMethod = "PaymentMethod B" },
                new ExpenseTrackerApi { Id = 3, Expense_Date = new DateTime(2023, 5, 1), Amount = 10000, Category = "Equipment", Description = "Bought Equipments", PaymentMethod = "PaymentMethod C" }
            });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task GetExpenseTrackerApis_ReturnsAllEvents()
        {
            var result = await _controller.GetExpenseTrackerApis();

            Assert.IsInstanceOf(typeof(List<ExpenseTrackerApi>), result.Value);
            Assert.AreEqual(3, result.Value.Count());
        }

    
        [Test]
        public async Task GetExpenseTrackerApi_InvalidId_ReturnsNotFound()
        {
            int eventId = 100;

            var result = await _controller.GetExpenseTrackerApi(eventId);

            Assert.IsInstanceOf(typeof(NotFoundResult), result.Result);
            
        }

        [Test]
        public async Task GetExpenseTrackerApi_ReturnsSingleEvent()
        {
            int eventId = 2;

            var result = await _controller.GetExpenseTrackerApi(eventId);

            Assert.IsInstanceOf(typeof(ExpenseTrackerApi), result.Value);
            Assert.AreEqual(eventId, result.Value.Id);
        }

        [Test]
        public async Task DeleteExpenseTrackerApi_DeletesExistingEvent()
        {
            int eventId = 1;

            var result = await _controller.DeleteExpenseTrackerApi(eventId);

            Assert.IsInstanceOf(typeof(NoContentResult), result);
            var eventFromDb = _context.ExpenseTrackerApis.Find(eventId);
            Assert.IsNull(eventFromDb);
        }

         [Test]
        public async Task PostExpenseTrackerApi_WithValidData_ReturnsCreatedAtActionResult()
        {
            var testExpenseTrackerApi = new ExpenseTrackerApi {Id = 4 ,Expense_Date = new DateTime(2023, 7, 1), Amount = 5000, Category = "Laptop", Description = "Bought laptop", PaymentMethod = "PaymentMethod D" };

            var result = await _controller.PostExpenseTrackerApi(testExpenseTrackerApi);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        }

        [Test]
        public async Task PostExpenseTrackerApi_WithValidData_ReturnsCreatedExpenseTrackerApi()
        {
            var testExpenseTrackerApi = new ExpenseTrackerApi { Id = 5 ,Expense_Date = new DateTime(2023, 7, 1), Amount = 750, Category = "Travel", Description = "office work", PaymentMethod = "PaymentMethod D"  };

            var result = await _controller.PostExpenseTrackerApi(testExpenseTrackerApi);

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);

            var createdExpenseTrackerApi = createdResult.Value as ExpenseTrackerApi;
            Assert.IsNotNull(createdExpenseTrackerApi);
            Assert.AreEqual(testExpenseTrackerApi.Expense_Date, createdExpenseTrackerApi.Expense_Date);
            Assert.AreEqual(testExpenseTrackerApi.Amount, createdExpenseTrackerApi.Amount);
            Assert.AreEqual(testExpenseTrackerApi.Category, createdExpenseTrackerApi.Category);
            Assert.AreEqual(testExpenseTrackerApi.Description, createdExpenseTrackerApi.Description);
        }  
        
        [Test]
        public async Task PutExpenseTrackerApi_Should_Update_ExpenseTrackerApi()
        {
            // Arrange
            var controller = new ExpenseTrackerController(_context);
            var expenseTrackerApi = new ExpenseTrackerApi { 
                        Id = 10,
                        Expense_Date = new DateTime(2023, 04, 13),
                        Amount = 30,
                        Category = "Groceries",
                        Description = "Bought some fruits",
                        PaymentMethod = "Credit Card"
                    };
            await controller.PostExpenseTrackerApi(expenseTrackerApi);

            expenseTrackerApi.PaymentMethod = "Gpay";
            expenseTrackerApi.Amount = 200;

            var result = await controller.PutExpenseTrackerApi(expenseTrackerApi.Id, expenseTrackerApi);

            Assert.That(result, Is.InstanceOf<NoContentResult>());

            var updatedExpenseTrackerApi = await _context.ExpenseTrackerApis.FindAsync(expenseTrackerApi.Id);
            Assert.That(updatedExpenseTrackerApi, Is.Not.Null);
            Assert.That(updatedExpenseTrackerApi.PaymentMethod, Is.EqualTo(expenseTrackerApi.PaymentMethod));
            Assert.That(updatedExpenseTrackerApi.Amount, Is.EqualTo(expenseTrackerApi.Amount));
        }

        
        [Test]
        public async Task PutExpenseTrackerApi_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var expenseTrackerApi = new ExpenseTrackerApi
            {
                Id = 10,
                Expense_Date = new DateTime(2023, 04, 13),
                Amount = 30,
                Category = "Groceries",
                Description = "Bought some fruits, vegetables, and bread",
                PaymentMethod = "Credit Card"
            };
            _context.ExpenseTrackerApis.Add(expenseTrackerApi);
            await _context.SaveChangesAsync();

            var result = await _controller.PutExpenseTrackerApi(2, expenseTrackerApi);

            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
    }
}
