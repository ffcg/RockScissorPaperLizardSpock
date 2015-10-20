using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RSPS.Model;

namespace RSPS.Mobile.Tests
{
// ReSharper disable once InconsistentNaming
    public class ConfigRepository_test
    {
        [Test]
        public void ConfigRepository_GetConfig_should_return_default_config_when_no_config_is_stored()
        {
            var localStorageMock = new Mock<ILocalStorage>();
            localStorageMock.Setup(mock => mock.LoadFile(@"config.json")).Returns(() => null);
            var localStorage = localStorageMock.Object;

            var configRepository = new ConfigRepository(localStorage);

            var config = configRepository.GetConfig();

            config.Name.Should().Be("Jan Banan");
            config.LoginAttempts.Should().Be(0);

            localStorageMock.VerifyAll();
        }
    }
}
