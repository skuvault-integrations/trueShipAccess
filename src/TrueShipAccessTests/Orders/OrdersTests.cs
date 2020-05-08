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
			//------------ Arrange
			var service = this._factory.CreateCommonService( this.Config );
			var ctSource = new CancellationTokenSource();

			//------------ Act
			var orders = service.GetOrdersAsync( base.OrganizationKey, new DateTime( 2020, 3, 30, 0, 0, 1, DateTimeKind.Utc ), 
				new DateTime( 2020, 5, 30, 21, 42, 20, DateTimeKind.Utc ), 
				ctSource.Token, Mark.Blank() );
			orders.Wait();

			//------------ Assert
			orders.Should().NotBeNull();
			orders.Result.Should().NotBeEmpty();
		}

		[ Test ]
		public async Task GetUnshippedOrdersAsync()
		{
			var service = this._factory.CreateCommonService( this.Config );

			var orders = await service.GetUnshippedOrdersAsync( base.OrganizationKey, DateTime.UtcNow, CancellationToken.None, Mark.Blank() );

			orders.Should().NotBeEmpty();
		}
	}
}