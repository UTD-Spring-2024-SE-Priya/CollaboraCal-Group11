using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollaboraCal;

namespace Phase4TestCases
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateUser()
        {
            AccountController accountController = new AccountController();
            Assert.AreEqual<string>("Email field is blank", 
                accountController.CreateUser("", "Bob", "Hello444", "Hello444"));
            Assert.AreEqual<string>("Password field is blank", 
                accountController.CreateUser("mc1174471@gmail.com", "Bob", "", "Hello444"));
            Assert.AreEqual<string>("Confirm Password field is blank", 
                accountController.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", ""));
            accountController.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444");
            Assert.AreEqual<string>("Email already exists",
                accountController.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444"));
            Assert.AreEqual<string>("Couldn't verify email address",
                accountController.CreateUser("hehehehgkhaao", "Bob", "Hello444", "Hello444"));
            Assert.AreEqual<string>("Passwords do not match each other",
                accountController.CreateUser("cag200009@utdallas.edu", "Bob", "Hello444", "Hello333"));
            Assert.AreEqual<string>("Password must be at least 8 characters long with at least one uppercase letter, lowercase letter, and number",
                accountController.CreateUser("cag200009@utdalas.edu", "Bob", "Hello55", "Hello55"));
            Assert.AreEqual<string>("Email and Password are Valid",
                accountController.CreateUser("cag200009@utdallas.edu", "Bob", "Hello555", "Hello555"));
        }

        [TestMethod]
        public void TestLogin()
        {
            //TODO
        }
    }
}