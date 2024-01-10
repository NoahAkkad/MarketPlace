using System.Collections.Generic;
using System.Security.Claims;
using EntityFrameworkCore.Testing.Moq;
using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace MyApp.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ApplicationDbContext mockContext;
        private ProductController controller;

        [SetUp]
        public void Setup()
        {
            mockContext = Create.MockedDbContextFor<ApplicationDbContext>();
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "TestingـCategory" }
            };
            mockContext.Set<Category>().AddRange(categories);

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 50, CategoryId = 1, Category = categories[0] }
            };
            mockContext.Set<Product>().AddRange(products);

            mockContext.SaveChanges();

            mockContext.ChangeTracker.Clear();
            controller = new ProductController(mockContext);
        }

        [Test]
        public void Index_ReturnsViewResultWithListOfProducts()
        {
            var result = controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<List<Product>>());
        }

        [Test]
        public void Create_ValidModel_RedirectsToIndex()
        {
            var result = controller.Create();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Edit_ValidModel_RedirectsToIndex()
        {
            var product = new Product { Id = 2, Name = "Test_Product", Price = 10, CategoryId = 1 };
            mockContext.Set<Product>().Add(product);
            mockContext.SaveChanges();

            var result = controller.Edit(product.Id) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsInstanceOf<Product>(result.Model);
            Assert.That((result.Model as Product).Id, Is.EqualTo(2));
        }

        [Test]
        public void Delete_ValidModel_RedirectsToIndex()
        {
            var product = new Product { Id = 3, Name = "Test555" };
            mockContext.Set<Product>().Add(product);
            mockContext.SaveChanges();

            var result = controller.Delete(3) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsInstanceOf<Product>(result.Model);
            Assert.That((result.Model as Product).Id, Is.EqualTo(3));
        }

        [Test]
        public void Create_InvalidModel_ReturnsViewResult()
        {
            controller.ModelState.AddModelError("Error", "Model error");
            var invalidProduct = new Product { Id = 5, Name = null };

            var result = controller.Create(invalidProduct) as ViewResult;

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

        [Test]
        public void Create_ActionIsUnauthorized()
        {
            SetupAuthorization(false, ""); // No Admin role claim; user is not an admin

            var result = controller.Create() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Edit_ActionIsUnauthorized()
        {
            SetupAuthorization(false, "username"); // No Admin role claim; user is not an admin

            var result = controller.Edit(1) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Delete_ActionIsUnauthorized()
        {
            SetupAuthorization(false, "username"); // No Admin role claim; user is not an admin

            var result = controller.Delete(1) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        private void SetupAuthorization(bool isAdmin, string userName = "username")
        {
            var httpContext = new DefaultHttpContext();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
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
