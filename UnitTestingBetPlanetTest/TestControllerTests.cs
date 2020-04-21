using BetPlanetTest;
using BetPlanetTest.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace UnitTestingBetPlanetTest
{
    [TestClass]
    public class TestControllerTests
    {

        private static TestController controller;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var services = new ServiceCollection();
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext = new DefaultHttpContext
            {
                RequestServices = services.BuildServiceProvider()
            };

            controller = new TestController(new PostgresDispatcherMock(), httpContextAccessorMock);
        }

        [TestInitialize()]
        public void Prepare()
        {

        }

        [TestCleanup()]
        public void Cleanup()
        {
            
        }

        // ���������� 1 ��� Id. �������� OkObjectResult � �������� Users � Id = 1
        [TestMethod]
        public void UsersGet_Ok_Test()
        {
            int id = 1;
            var actionResult = controller.UsersGet(id);
            var result = actionResult.Result as OkObjectResult;
            
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            Users user = result.Value as Users;
            
            Assert.IsInstanceOfType(user, typeof(Users));
            Assert.AreEqual(user.Id, id);
            
        }

        // ���������� �������� < 0. �������� NotFoundResult
        [TestMethod]
        public void UsersGet_NotFound_Test()
        {
            int id = -1;
            var actionResult = controller.UsersGet(id);
            var result = actionResult.Result as NotFoundResult;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        // ���������� "test name1" ��� name. �������� OkObjectResult � �������� Users � name = "test name1"
        [TestMethod]
        public void UsersNameGet_Ok_Test()
        {
            string name = "test name1";
            var actionResult = controller.UsersNameGet(name);
            var result = actionResult.Result as OkObjectResult;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            Users user = result.Value as Users;

            Assert.IsInstanceOfType(user, typeof(Users));
            Assert.AreEqual(user.Name, name);
        }

        // ���������� "fake name" ��� name. �������� NotFoundResult
        [TestMethod]
        public void UsersNameGet_NotFound_Test()
        {
            string name = "fake name";
            var actionResult = controller.UsersNameGet(name);
            var result = actionResult.Result as NotFoundResult;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
