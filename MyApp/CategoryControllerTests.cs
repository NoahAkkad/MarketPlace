using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using EntityFrameworkCore.Testing.Moq;
using MarketPlace.Data;
using MarketPlace.Models;

namespace MyApp.Tests
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private ApplicationDbContext mockContext;
        private CategoryController controller;

        [SetUp]
        public void Setup()
        {
            mockContext = Create.MockedDbContextFor<ApplicationDbContext>();
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "testing1" }
            };
            mockContext.Set<Category>().AddRange(categories);
            mockContext.SaveChanges();

            mockContext.ChangeTracker.Clear();
            controller = new CategoryController(mockContext);
        }

        [Test]
        public void Index_ReturnsViewResultWithListOfCategories()
        {
            var result = controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<List<Category>>());
        }

        [Test]
        public void Create_ReturnsViewResultIsValid()
        {
            var result = controller.Create();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Edit_ValidModel_RedirectsToIndex()
        {
            var category = new Category { Id = 2, Name = "testing2" };
            mockContext.Set<Category>().Add(category);
            mockContext.SaveChanges();

            var result = controller.Edit(category.Id) as ViewResult;

            Assert.IsInstanceOf<Category>(result.Model);
            Assert.That((result.Model as Category).Id, Is.EqualTo(2));
        }

        [Test]
        public void Delete_ValidModel_RedirectsToIndex()
        {
            var category = new Category { Id = 3, Name = "testing3" };
            mockContext.Set<Category>().Add(category);
            mockContext.SaveChanges();

            var result = controller.Delete(3) as ViewResult;

            Assert.IsInstanceOf<Category>(result.Model);
            Assert.That((result.Model as Category).Id, Is.EqualTo(3));
        }

        [Test]
        public void Create_InvalidModel_ReturnsViewResult()
        {
            controller.ModelState.AddModelError("Error", "Model error");
            var invalidCategory = new Category { Id = 5, Name = null };

            var result = controller.Create(invalidCategory) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCase(true)] // Authorized
        [TestCase(false)] // Unauthorized
        public void Create_ActionAuthorization(bool isAdmin)
        {
            SetupAuthorization(isAdmin);

            var result = controller.Create() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCase(true)] // Authorized
        [TestCase(false)] // Unauthorized
        public void Edit_ActionAuthorization(bool isAdmin)
        {
            SetupAuthorization(isAdmin);

            var result = controller.Edit(1) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCase(true)] // Authorized
        [TestCase(false)] // Unauthorized
        public void Delete_ActionAuthorization(bool isAdmin)
        {
            SetupAuthorization(isAdmin);

            var result = controller.Delete(1) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCase(true)] // Authorized
        [TestCase(false)] // Unauthorized
        public void Details_ActionAuthorization(bool isAdmin)
        {
            SetupAuthorization(isAdmin);

            var result = controller.Details(1) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        private void SetupAuthorization(bool isAdmin)
        {
            var httpContext = new DefaultHttpContext();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username")
            };
            if (isAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IHttpContextAccessor>(new HttpContextAccessor { HttpContext = httpContext })
                .BuildServiceProvider();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            httpContext.RequestServices = serviceProvider;

            controller.TempData = tempData;
        }
    }
}
