using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace TrueShipAccessTests.Companies
{
	[ TestFixture ]
	public class CompanyTests : TestBase
	{
		[ Test ]
		public void GetCompanies()
		{
			//------------ Arrange
			var service = this._factory.CreateCommonService( this.Config );
			var ctSource = new CancellationTokenSource();

			//------------ Act
			var companies = service.GetCompanies( 0, ctSource.Token );
			companies.Wait();

			//------------ Assert
			companies.Result.Should().NotBeNullOrEmpty();
		}
	}
}