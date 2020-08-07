using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChustaSoft.Tools.Authorization.UnitTest
{
    [TestClass]
    public class CredentialsBuilderUnitTest
    {

        private static ICredentialsBuilder CredentialsBuilderUnderTest;

        
        [TestInitialize]
        public void TestInitialization()
        {
            CredentialsBuilderUnderTest = new CredentialsBuilder();
        }


        [TestMethod]
        public void Given_UserAndPassword_When_Build_Then_BasicCredentialsRetrived()
        {
            string user = "test", password = "test1234";

            CredentialsBuilderUnderTest.Add(user, password);

            var result = CredentialsBuilderUnderTest.Build();

            Assert.AreEqual(result.First().Credentials.Username, user);
            Assert.AreEqual(result.First().Credentials.Password, password);
            Assert.IsFalse(result.First().Credentials.Parameters.Any());
            Assert.IsFalse(result.First().Roles.Any());
        }

        [TestMethod]
        public void Given_MultipleUserAndPassword_When_Build_Then_BasicCredentialsForBothRetrived()
        {
            string user1 = "test", password1 = "test1234";
            string user2 = "test2", password2 = "test1234";

            CredentialsBuilderUnderTest.Add(user1, password1);
            CredentialsBuilderUnderTest.Add(user2, password2);

            var result = CredentialsBuilderUnderTest.Build();

            Assert.AreEqual(result.Count(), 2);
            Assert.IsNotNull(result.First(x => x.Credentials.Username == user1).Credentials.Username);
            Assert.IsNotNull(result.First(x => x.Credentials.Password == password1).Credentials.Password);
            Assert.IsFalse(result.First(x => x.Credentials.Username == user1).Credentials.Parameters.Any());
            Assert.IsFalse(result.First(x => x.Credentials.Username == user1).Roles.Any());
            Assert.IsNotNull(result.First(x => x.Credentials.Username == user2).Credentials.Username);
            Assert.IsNotNull(result.First(x => x.Credentials.Password == password2).Credentials.Password);
            Assert.IsFalse(result.First(x => x.Credentials.Username == user2).Credentials.Parameters.Any());
            Assert.IsFalse(result.First(x => x.Credentials.Username == user2).Roles.Any());
        }

        [TestMethod]
        public void Given_MultipleUserAndPasswordOneWithRoles_When_Build_Then_BasicCredentialsForOneAndWithRolesForOtherRetrived()
        {
            string user1 = "test", password1 = "test1234";
            string user2 = "test2", password2 = "test12345", role2 = "role";


            CredentialsBuilderUnderTest.Add(user1, password1);
            CredentialsBuilderUnderTest.Add(user2, password2).WithRole(role2);

            var result = CredentialsBuilderUnderTest.Build();

            Assert.AreEqual(result.Count(), 2);
            Assert.IsNotNull(result.First(x => x.Credentials.Username == user1).Credentials.Username);
            Assert.IsNotNull(result.First(x => x.Credentials.Password == password1).Credentials.Password);
            Assert.IsFalse(result.First(x => x.Credentials.Username == user1).Credentials.Parameters.Any());
            Assert.IsFalse(result.First(x => x.Credentials.Username == user1).Roles.Any());
            Assert.IsNotNull(result.First(x => x.Credentials.Username == user2).Credentials.Username);
            Assert.IsNotNull(result.First(x => x.Credentials.Password == password2).Credentials.Password);
            Assert.IsFalse(result.First(x => x.Credentials.Username == user2).Credentials.Parameters.Any());
            Assert.IsTrue(result.First(x => x.Credentials.Username == user2).Roles.Any());
            Assert.AreEqual(result.First(x => x.Credentials.Username == user2).Roles.First(), role2);
        }

    }
}
