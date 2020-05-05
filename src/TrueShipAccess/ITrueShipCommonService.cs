using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrueShipAccess.Models;
using Netco.Logging;

namespace TrueShipAccess
{
	public interface ITrueShipCommonService
	{
		Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo, CancellationToken ct, Mark mark );
		IEnumerable< OrderResource.TrueShipOrder > GetOrders( DateTime dateFrom, DateTime dateTo, Mark mark );
		Task< bool > UpdateOrderItemPickLocations( IEnumerable< ItemLocationUpdateModel > orderitemlist, CancellationToken ct, Mark mark );
		Task< IEnumerable< OrderResource.TrueShipOrder > > GetUnshippedOrdersAsync( DateTime dateTo, CancellationToken ct, Mark mark );
	}
}