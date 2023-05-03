using Avionera.Services;

namespace Avionera.UnitTests
{
    internal class CitizenNumberValidatorTests
    {
        private CitizenNumberValidator _validatorService;

        [SetUp]
        public void Setup()
        {
            _validatorService = new CitizenNumberValidator();
        }

        [Test]
        public void IsValid_CitizenNumber_ReturnsTrue() 
        {
            var result = _validatorService.IsValid("8402130471");
            Assert.IsTrue(result);
        }
        [Test]
        public void IsValid_CitizenNumber_ReturnsFalse()
        {
            var result = _validatorService.IsValid("1204350471");
            Assert.IsFalse(result);
        }
    }
}