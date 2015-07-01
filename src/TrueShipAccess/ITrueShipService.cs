using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public interface ITrueShipService
	{
		Task<IEnumerable<TrueShipOrderResource>> GetOrders(DateTime lastOrderSync);

		Task<IEnumerable<TrueShipOrderResource>> GetOrdersAsync(DateTime dateFrom, DateTime dateTo);

		RemainingOrdersResource GetRemainingOrders();

		TrueShipOrderResource GetOrderById(string id);

		Task<List<Item>> GetUnshippedOrderItemsAfterDateTime(int id, string datefilter, DateTime lastsync);

		Task<bool> UpdateOrderItemPickLocations(IEnumerable<KeyValuePair<string, PickLocation>> orderitemlist);

		Task<IEnumerable<Box>> GetBoxes(int limit, int offset, int orderId = -1);

		Task<IEnumerable<CompanyResponse.Company>> GetCompanies(int offset);
	}
}