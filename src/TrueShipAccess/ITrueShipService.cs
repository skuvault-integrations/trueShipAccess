using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public interface ITrueShipService
	{
		/// <summary>
		/// Expected format (JSON or XML).
		/// </summary>
		string Format { get; set; }

		/// <summary>
		/// The number of entries to limit the returned data to.
		/// </summary>
		int Limit { get; set; }

		/// <summary>
		/// Retrieve a list of orders by last updated time
		/// </summary>
		/// <param name="lastSync"></param>
		/// <returns></returns>
		Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrders( DateTime lastSync );

		Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo );

		/// <summary>
		/// Retrieve the number of remaining orders for the company with the specified ID
		/// </summary>
		/// <param name="companyId"></param>
		/// <returns></returns>
		Task< RemainingOrdersResource > GetRemainingOrders( int? companyId = null );

		/// <summary>
		/// Retrieve a single order by ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task< OrderResource.TrueShipOrder > GetOrder( string id );

		Task< List< TrueShipItem > > GetUnshippedOrderItemsAfterDateTime( int id, string datefilter, DateTime lastsync );

		/// <summary>
		/// Update pick_location for order items by item id
		/// </summary>
		/// <param name="orderitemlist"></param>
		/// <returns></returns>
		Task< bool > UpdateOrderItemPickLocations( IEnumerable< KeyValuePair< string, PickLocation > > orderitemlist );

		/// <summary>
		/// Retrieve a list of boxes.
		/// </summary>
		/// <param name="limit">The number of entries to limit the returned data to</param>
		/// <param name="offset">The entry to start the returned data with</param>
		/// <param name="orderId">Order ID for which you want retrieve boxes</param>
		/// <returns></returns>
		Task< IEnumerable< TrueShipBox > > GetBoxes( int limit, int offset, int? orderId = null );

		/// <summary>
		/// Retrieve a list of box items.
		/// </summary>
		/// <param name="limit">The number of entries to limit the returned data to.</param>
		/// <param name="offset">The entry to start the returned data with.</param>
		/// <param name="boxId">Box ID for which you want retrieve items.</param>
		/// <returns></returns>
		Task< IEnumerable< TrueShipItem > > GetItems( int limit, int offset, int? boxId = null );

		/// <summary>
		/// Retrieve a list of all items.
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<TrueShipItem>> GetItems();
		
		/// <summary>
		/// Retrieve a list of companies.
		/// </summary>
		/// <param name="offset">The entry to start the returned data with.</param>
		/// <returns></returns>
		Task< IEnumerable< Company > > GetCompanies( int offset );
	}
}