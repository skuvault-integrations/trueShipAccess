using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public interface ITrueShipCommonService
	{
		/// <summary>
		///     Expected format (JSON or XML).
		/// </summary>
		string Format { get; set; }

		/// <summary>
		///     The number of entries to limit the returned data to.
		/// </summary>
		int Limit { get; set; }

		/// <summary>
		///     Retrieve a list of boxes.
		/// </summary>
		/// <param name="limit">The number of entries to limit the returned data to</param>
		/// <param name="offset">The entry to start the returned data with</param>
		/// <param name="ct"></param>
		/// <param name="orderId">Order ID for which you want retrieve boxes</param>
		/// <returns></returns>
		Task< IEnumerable< TrueShipBox > > GetBoxes( int limit, int offset, CancellationToken ct, int? orderId = null );

		/// <summary>
		///     Retrieve a list of companies.
		/// </summary>
		/// <param name="offset">The entry to start the returned data with.</param>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task< IEnumerable< Company > > GetCompanies( int offset, CancellationToken ct );

		/// <summary>
		///     Retrieve a list of box items.
		/// </summary>
		/// <param name="limit">The number of entries to limit the returned data to.</param>
		/// <param name="offset">The entry to start the returned data with.</param>
		/// <param name="ct"></param>
		/// <param name="boxId">Box ID for which you want retrieve items.</param>
		/// <returns></returns>
		Task< IEnumerable< TrueShipItem > > GetItems( int limit, int offset, CancellationToken ct, int? boxId = null );

		/// <summary>
		///     Retrieve a list of all items.
		/// </summary>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task< IEnumerable< TrueShipItem > > GetItems( CancellationToken ct );

		/// <summary>
		///     Retrieve a single order by ID.
		/// </summary>
		/// <param name="orderId"></param>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task< OrderResource.TrueShipOrder > GetOrder( string orderId, CancellationToken ct );

		/// <summary>
		///     Retrieve a list of orders by last updated time
		/// </summary>
		/// <param name="lastSync"></param>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrders( DateTime lastSync, CancellationToken ct );

		Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo, CancellationToken ct );

		/// <summary>
		///     Retrieve the number of remaining orders for the company with the specified ID
		/// </summary>
		/// <param name="ct"></param>
		/// <param name="companyId"></param>
		/// <returns></returns>
		Task< RemainingOrdersResource > GetRemainingOrders( CancellationToken ct, int? companyId = null );

		Task< List< TrueShipItem > > GetUnshippedOrderItemsAfterDateTime( int id, string datefilter, DateTime lastsync, CancellationToken ct );

		/// <summary>
		///     Update pick_location for order items by item id
		/// </summary>
		/// <param name="orderitemlist"></param>
		/// <param name="ct"></param> 
		/// <returns></returns>
		Task< bool > UpdateOrderItemPickLocations( IEnumerable< KeyValuePair< string, PickLocation > > orderitemlist, CancellationToken ct );

		IEnumerable< OrderResource.TrueShipOrder > GetOrders( DateTime dateFrom, DateTime dateTo );
	}
}