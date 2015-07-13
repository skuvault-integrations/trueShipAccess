using FluentAssertions;
using NUnit.Framework;

namespace TrueShipAccessTests.Companies
{
	[TestFixture]
	public class CompanyTests : TestBase
	{
		[Test]
		public void GetCompanies()
		{
			//------------ Arrange
			var service = this._factory.CreateService(this.Credentials);

			//------------ Act
			var companies = service.GetCompanies(0);
			companies.Wait();

			//------------ Assert
			companies.Result.Should().NotBeNullOrEmpty();
		}
	}
}