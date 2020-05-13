using FluentAssertions;
using Netco.Logging;
using NUnit.Framework;
using System.Threading;

namespace TrueShipAccessTests
{
	[ TestFixture ]
	public class OrganizationsTests : TestBase
	{
		[ Test ]
		public void GetActiveOrganizationsAsync()
		{
			var result = _commonService.GetActiveOrganizationsAsync( CancellationToken.None, Mark.Blank() ).Result;

			result.Should().NotBeEmpty();
		}
	}
}
