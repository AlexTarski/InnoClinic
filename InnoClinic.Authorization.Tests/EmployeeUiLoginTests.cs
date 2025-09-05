using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using SeleniumExtras.WaitHelpers;

namespace InnoClinic.Authorization.Tests
{
    [TestFixture]
    public class EmployeeUiLoginTests
    {
        //TODO: Move strings to localization files
        private const string _loginSuccessPageName = "login-success";
        private const string _activeDoctorProfileEmail = "elena.volkova@example.com";
        private const string _inactiveDoctorProfileEmail = "sergey.ivanov@example.com";
        private const string _receptionistProfileEmail = "olga.smirnova@clinic.com";
        private const string _patientProfileEmail = "maxim.petrov@patientmail.com";
        private const string _nonExistingProfileEmail = "123@example.com";
        private const string _baseUrl = "https://localhost:4300";
        private const string _emailFieldName = "Email";
        private const string _passwordFieldName = "Password";
        private const string _validationErrorsField = "//div[@class='validation-summary-errors validation-summary-errors']";
        private const string _messageHeaderField = "//div[@class='message-header']";
        private const string _submitButtonId = "submit-button";
        private const string _password = "123456";
        private const string _passwordForPatient = "Aa123456!";
        private const string _validationErrorsFieldMessage = "Either an email or a password is incorrect";
        private const string _messageHeaderFieldMessage = "Invalid Profile Type";
        private ChromeDriver _driver;
        private StringBuilder _verificationErrors;
        private WebDriverWait _wait;

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _verificationErrors = new StringBuilder();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        }

        [TearDown]
        public void CleanUp()
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
            await Login(_activeDoctorProfileEmail, _password);
            _wait.Until(d => d.Url.Contains(_loginSuccessPageName));

            Assert.That(_driver.Url, Does.Contain(_loginSuccessPageName));
        }

        [Test]
        public async Task LoginAsDoctorInactiveProfileTest()
        {
            await Login(_inactiveDoctorProfileEmail, _password);
            var messageOnThePage = _wait.Until(
               ExpectedConditions.ElementIsVisible(By.XPath(_validationErrorsField))).Text;

            Assert.That(messageOnThePage, Is.EqualTo(_validationErrorsFieldMessage));
        }

        [Test]
        public async Task LoginAsReceptionistTest()
        {
            await Login(_receptionistProfileEmail, _password);
            _wait.Until(d => d.Url.Contains(_loginSuccessPageName));

            Assert.That(_driver.Url, Does.Contain(_loginSuccessPageName));
        }

        [Test]
        public async Task LoginAsPatientProfileTest()
        {
            await Login(_patientProfileEmail, _passwordForPatient);
            var messageOnThePage = _wait.Until(
                ExpectedConditions.ElementIsVisible(By.XPath(_messageHeaderField))).Text;

            Assert.That(messageOnThePage, Is.EqualTo(_messageHeaderFieldMessage));
        }

        [Test]
        public async Task LoginAsNonexistentProfileTest()
        {
            await Login(_nonExistingProfileEmail, _password);
            var messageOnThePage = _wait.Until(
                ExpectedConditions.ElementIsVisible(By.XPath(_validationErrorsField))).Text;

            Assert.That(messageOnThePage, Is.EqualTo(_validationErrorsFieldMessage));
        }

        private async Task EnterValueToField(string fieldName, string value)
        {
            var field = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(fieldName)));

            field.Click();
            await Task.Delay(300);
            field.Clear();
            await Task.Delay(300);
            field.SendKeys(value);
        }

        private async Task Login(string email, string password)
        {
            await _driver.Navigate().GoToUrlAsync(_baseUrl);
            await EnterValueToField(_emailFieldName, email);
            await EnterValueToField(_passwordFieldName, password);
            await Task.Delay(300);
            _driver.FindElement(By.Id(_submitButtonId)).Click();
        }
    }
}