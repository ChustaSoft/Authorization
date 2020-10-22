using ChustaSoft.Tools.Authorization.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Authentication;

namespace ChustaSoft.Tools.Authorization.UnitTest.TestServices
{
    [TestClass]
    public class UserValidationUnitTest
    {

        [TestMethod]
        public void Given_UserWithPhone_When_GetLoginType_Then_LoginTypePhoneRetrived()
        {
            var validation = new UserValidation { Phone = "666999666", ConfirmationToken = "XXXXXX" };

            var typeRetrived = validation.GetLoginType();

            Assert.AreEqual(typeRetrived, LoginType.PHONE);
        }

        [TestMethod]
        public void Given_EmailAndToken_When_GetLoginType_Then_LoginTypeMailRetrived()
        {
            var credentials = new UserValidation { Email = "test@email.com", ConfirmationToken = "XXXXXX" };

            var typeRetrived = credentials.GetLoginType();

            Assert.AreEqual(typeRetrived, LoginType.MAIL);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void Given_ValidationWithoutToken_When_GetLoginType_Then_ExceptionThrown()
        {
            var credentials = new UserValidation { Email = "test@mail.com" };

            credentials.GetLoginType();
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void Given_WrongUser_When_GetLoginType_Then_ExceptionThrown()
        {
            var credentials = new UserValidation();

            credentials.GetLoginType();
        }

    }
}
