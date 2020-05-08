using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Netco.Logging;

namespace TrueShipAccessTests.Orders
{
	[ TestFixture ]
	public class OrdersTests : TestBase
	{
		[ Test ]
		public void GetOrders()
		{
			var orders = _commonService.GetOrdersAsync( base.OrganizationKey, new DateTime( 2020, 3, 30, 0, 0, 1, DateTimeKind.Utc ), 
				new DateTime( 2020, 5, 30, 21, 42, 20, DateTimeKind.Utc ), 
				CancellationToken.None, Mark.Blank() );
			orders.Wait();

			//------------ Assert
			orders.Should().NotBeNull();
			orders.Result.Should().NotBeEmpty();
		}

		[ Test ]
		public async Task GetUnshippedOrdersAsync()
		{
			var orders = await _commonService.GetUnshippedOrdersAsync( base.OrganizationKey, DateTime.UtcNow, CancellationToken.None, Mark.Blank() );

			orders.Should().NotBeEmpty();
		}
	}
}