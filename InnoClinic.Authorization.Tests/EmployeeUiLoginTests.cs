using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

using SeleniumExtras.WaitHelpers;

namespace InnoClinic.Authorization.Tests
{
    [TestFixture]
    public class EmployeeUiLoginTests
    {
        private FirefoxDriver _driver;
        private StringBuilder _verificationErrors;
        private readonly string _baseUrl = "https://localhost:4300";
        private readonly string _emailFieldName = "Email";
        private readonly string _passwordFieldName = "Password";
        private readonly string _validationErrorsFieldName = "validation-summary-errors validation-summary-errors";
        private readonly string _messageHeaderFieldName = "message-header";
        private readonly string _submitButtonName = "submit";

        [SetUp]
        public void SetupTest()
        {
            _driver = new FirefoxDriver();
            _verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }

            Assert.That(_verificationErrors.ToString(), Is.EqualTo(""));
        }

        [Test]
        public async Task LoginAsDoctorActiveProfileTest()
        {
            await Login("elena.volkova@example.com", "123456");
            await Task.Delay(2000);

            Assert.That(_driver.Url, Is.EqualTo($"{_baseUrl}/login-success"));
        }

        [Test]
        public async Task LoginAsDoctorInactiveProfileTest()
        {
            await Login("sergey.ivanov@example.com", "123456");
            await Task.Delay(2000);
            var messageOnThePage = _driver.FindElement(
                By.XPath($"//div[@class='{_validationErrorsFieldName}']")).Text;

            Assert.That(messageOnThePage, Is.EqualTo("Either an email or a password is incorrect"));
        }

        [Test]
        public async Task LoginAsReceptionistTest()
        {
            await Login("olga.smirnova@clinic.com", "123456");
            await Task.Delay(2000);

            Assert.That(_driver.Url, Is.EqualTo($"{_baseUrl}/login-success"));

        }

        [Test]
        public async Task LoginAsPatientProfileTest()
        {
            await Login("maxim.petrov@patientmail.com", "Aa123456!");
            await Task.Delay(2000);
            var messageOnThePage = _driver.FindElement(
                By.XPath($"//div[@class='{_messageHeaderFieldName}']")).Text;

            Assert.That(messageOnThePage, Is.EqualTo($"Invalid Profile Type"));
        }

        [Test]
        public async Task LoginAsNonexistentProfileTest()
        {
            await Login("123@example.com", "123456");
            await Task.Delay(2000);
            var messageOnThePage = _driver.FindElement(
                By.XPath($"//div[@class='{_validationErrorsFieldName}']")).Text;

            Assert.That(messageOnThePage, Is.EqualTo("Either an email or a password is incorrect"));
        }

        private async Task EnterValueToField(string fieldName, string value)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var field = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(fieldName)));

            field.Click();
            await Task.Delay(300);
            field.Clear();
            await Task.Delay(300);
            field.SendKeys(value);
        }

        private async Task Login(string email, string password)
        {
            await _driver.Navigate().GoToUrlAsync(_baseUrl);
            await Task.Delay(1000);
            await EnterValueToField(_emailFieldName, email);
            await EnterValueToField(_passwordFieldName, password);
            await Task.Delay(300);
            _driver.FindElement(By.XPath($"//button[@type='{_submitButtonName}']")).Click();
        }
    }
}