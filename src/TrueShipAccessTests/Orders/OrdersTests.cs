using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TrueShipAccess.Models;

namespace TrueShipAccessTests.Orders
{
	public class TestConfig
	{
		public string AccessToken { get; set; }
		public int CompanyId { get; set; }
	}

	public class ExistingOrderIds
	{
		public static readonly List< string > BoxIds = new List< string >
		{
			"/api/v1/items/7747246/",
			"/api/v1/items/7747245/",
			"/api/v1/items/7747244/",
			"/api/v1/items/5203283/",
			"/api/v1/items/5203282/",
			"/api/v1/items/5203281/",
			"/api/v1/items/5203221/",
			"/api/v1/items/5203220/",
			"/api/v1/items/5203219/"
		};

		public static readonly List< string > OrderIds = new List< string > { "TRUE000001", "TRUE000002", "TRUE000003" };
	}

	[ TestFixture ]
	public class OrdersTests : TestBase
	{
		[ Test ]
		public void CanGetBoxes()
		{
			//------------ Arrange
			var service = this._factory.CreateService( this.Credentials );
			var ctSource = new CancellationTokenSource();

			//------------ Act
			var boxes = service.GetBoxes( 10, 0, ctSource.Token );
			boxes.Wait();

			//------------ Assert
			boxes.Result.Should().NotBeEmpty();
		}

		[ Test ]
		public async Task CanUpdateOrderPickLocation()
		{
			//------------ Arrange
			var service = this._factory.CreateService( this.Credentials );

			//------------ Act
			var wasUpdated = await service.UpdateOrderItemPickLocations( new List< KeyValuePair< string, PickLocation > >
			{
				new KeyValuePair< string, PickLocation >( ExistingOrderIds.BoxIds.First(), new PickLocation
				{
					Location = "Somwhere11111"
				} )
			} );

			//------------ Assert
			wasUpdated.Should().BeTrue();
		}

		[ Test ]
		public void GetOrders()
		{
			//------------ Arrange
			var service = this._factory.CreateService( this.Credentials );
			var ctSource = new CancellationTokenSource();

			//------------ Act
			var orders = service.GetOrdersAsync( DateTime.MinValue, DateTime.MaxValue, ctSource.Token );
			orders.Wait();

			//------------ Assert
			orders.Should().NotBeNull();
			orders.Result.Should().NotBeEmpty();
			orders.Result.Should().HaveCount( 3 );
			orders.Result.Select( x => x.PrimaryId ).ShouldAllBeEquivalentTo( ExistingOrderIds.OrderIds );

			var syncDate = orders.Result.Last().UpdatedAt;

			var filteredOrders = service.GetOrdersAsync( syncDate, DateTime.MaxValue, ctSource.Token );
			filteredOrders.Wait();

			//------------ Assert
			filteredOrders.Should().NotBeNull();
			filteredOrders.Result.Should().HaveCount( orders.Result.Count( x => x.UpdatedAt > syncDate ) );
		}

		[ Test ]
		public void GetRemainingOrders()
		{
			//------------ Arrange
			var service = this._factory.CreateService( this.Credentials );
			var ctSource = new CancellationTokenSource();

			//------------ Act
			var orders = service.GetRemainingOrders( ctSource.Token );
			orders.Wait();
			//------------ Assert
			orders.Result.remaining_orders.Should().BeGreaterThan( 0 );
		}
	}
}