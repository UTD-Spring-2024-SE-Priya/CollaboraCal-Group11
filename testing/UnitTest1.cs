using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollaboraCal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.Sqlite;
using CollaboraCal.JsonRequests;

namespace CollaboraCal.Testing
{
    [TestClass]
    public class UnitTest1
    {

        private const string DatabaseMigrationError = "Database has not been migrated correctly on this machine. Database migration must be applied on each machine before the program can be tested.";

        [TestMethod]
        public void TestCreateUser()
        {
            try
            {
                Assert.AreEqual("Email field is blank",
                    Application.Accounts.CreateUser("", "Bob", "Hello444", "Hello444"));
                Assert.AreEqual("Password field is blank",
                    Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "", "Hello444"));
                Assert.AreEqual("Confirm Password field is blank",
                    Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", ""));
                Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444");
                Assert.AreEqual("Email already exists",
                    Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444"));
                Assert.AreEqual("Couldn't verify email address",
                    Application.Accounts.CreateUser("hehehehgkhaao", "Bob", "Hello444", "Hello444"));
                Assert.AreEqual("Passwords do not match each other",
                    Application.Accounts.CreateUser("cag200009@utdallas.edu", "Bob", "Hello444", "Hello333"));
                Assert.AreEqual("Password must be at least 8 characters long with at least one uppercase letter, lowercase letter, and number",
                    Application.Accounts.CreateUser("cag200009@utdalas.edu", "Bob", "Hello55", "Hello55"));
                Assert.AreEqual("Email and Password are Valid",
                    Application.Accounts.CreateUser("cag200009@utdallas.edu", "Bob", "Hello555", "Hello555"));

                // Successful test case emails:
                //      mc1174471@gmail.com
                //      cag200009@utdallas.edu

                Application.Database.RemoveUserByEmail("mc1174471@gmail.com");
                Application.Database.RemoveUserByEmail("cag200009@utdallas.edu");

            } catch(SqliteException sqliteException)
            {
                Console.WriteLine($"SQLite Error: {sqliteException.SqliteErrorCode}");
                Assert.Inconclusive(DatabaseMigrationError);
            }
        }

        [TestMethod]
        public void TestLogin()
        {
            string email = "mytestemail@test.com";
            string username = "TestUserName1";
            string password = "MyPassword1234";

            try
            {
                Application.Accounts.CreateUser(email, username, password, password);
                string? result = Application.Sessions.Login(email, password);

                Assert.IsNotNull(result);

                Application.Database.RemoveUserByEmail(email);
            }
            catch (SqliteException sqliteException)
            {
                Console.WriteLine($"SQLite Error: {sqliteException.SqliteErrorCode}");
                Assert.Inconclusive(DatabaseMigrationError);
            }

        }

        [TestMethod]
        public void TestCreateCalendar()
        {
            try
            {
                NewCalendarData data;

                Assert.IsNull(Application.CalendarManager.CreateCalendar("mc1174471@gmail.com", data = new("Birthdays", "This calendar holds birthdays")));

                Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444");

                Assert.IsNull(Application.CalendarManager.CreateCalendar("mc1174471@gmail.com", data = new("", "This calendar holds birthdays.")));
                Assert.IsNull(Application.CalendarManager.CreateCalendar("mc1174471@gmail.com", data = new("Birthdays", "")));
                Assert.IsNull(Application.CalendarManager.CreateCalendar("", data = new("Birthdays", "This calendar holds birthdays")));
                Assert.IsNotNull(Application.CalendarManager.CreateCalendar("mc1174471@gmail.com", data = new("Birthdays", "This calendar holds birthdays")));

                Application.Database.RemoveUserByEmail("mc1174471@gmail.com");

            }
            catch (SqliteException sqliteException)
            {
                Console.WriteLine($"SQLite Error: {sqliteException.SqliteErrorCode}");
                Assert.Inconclusive(DatabaseMigrationError);
            }
        }

        [TestMethod]
        public void TestCreateEvent() 
        {
            try
            {
                NewEventData data;
                DateTime timeStart = DateTime.Now;
                DateTime timeEnd = DateTime.Now;
                Assert.AreEqual(false, Application.CalendarManager.CreateEvent("mc1174471@gmail.com", 
                    data = new("Trash", "Trash on Fridays at 7 pm", "Home", timeStart, timeEnd, 0)));
                
                Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444");

                Assert.AreEqual(false, Application.CalendarManager.CreateEvent("mc1174471@gmail.com",
                    data = new("Trash", "Trash on Fridays at 7 pm", "Home", timeStart, timeEnd, 0)));

                Application.CalendarManager.CreateCalendar("mc1174471@gmail.com", new("Tasks", "This calendar holds home tasks"));

                User? user = Application.Database.GetLightUserFromEmail("mc1174471@gmail.com");
                Assert.AreEqual(false, Application.CalendarManager.CreateEvent("", 
                    data = new("Trash", "Trash on Fridays at 7 pm", "Home", timeStart, timeEnd, user.Calendars[0].ID)));
                Assert.AreEqual(false, Application.CalendarManager.CreateEvent("mc1174471@gmail.com",
                    data = new("Trash", "Trash on Fridays at 7 pm", "Home", timeStart, timeEnd, user.Calendars[0].ID + 1)));
                Assert.AreEqual(true, Application.CalendarManager.CreateEvent("mc1174471@gmail.com",
                    data = new("Trash", "Trash on Fridays at 7 pm", "Home", timeStart, timeEnd, user.Calendars[0].ID)));

                Application.Database.RemoveUserByEmail("mc1174471@gmail.com");
            }
            catch (SqliteException sqliteException)
            {
                Console.WriteLine($"SQLite Error: {sqliteException.SqliteErrorCode}");
                Assert.Inconclusive(DatabaseMigrationError);
            }
        }

        [TestMethod]
        public void TestResetPassword()
        { 
            try
            {
                Assert.AreEqual(false, Application.Accounts.ResetPassword("mc1174471@gmail.com", "Goodbye444", "Goodbye444"));
                
                Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444");

                Assert.AreEqual(false, Application.Accounts.ResetPassword("", "Hello444", "Hello444"));
                Assert.AreEqual(false, Application.Accounts.ResetPassword("mc1174471@gmail.com", "", "Goodbye444"));
                Assert.AreEqual(false, Application.Accounts.ResetPassword("mc1174471@gmail.com", "Goodbye444", ""));
                Assert.AreEqual(false, Application.Accounts.ResetPassword("mc1174471@gmail.com", "Goodbye444", "Goodbye443"));
                Assert.AreEqual(false, Application.Accounts.ResetPassword("mc1174471@gmail.com", "Goodbye", "Goodbye"));
                Assert.AreEqual(true, Application.Accounts.ResetPassword("mc1174471@gmail.com", "Goodbye444", "Goodbye444"));

                Application.Database.RemoveUserByEmail("mc1174471@gmail.com");
            }
            catch (SqliteException sqliteException)
            {
                Console.WriteLine($"SQLite Error: {sqliteException.SqliteErrorCode}");
                Assert.Inconclusive(DatabaseMigrationError);
            }
        }
    }
}