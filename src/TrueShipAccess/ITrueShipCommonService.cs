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
		Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrdersAsync( string organizationKey, DateTime dateFrom, DateTime dateTo, CancellationToken ct, Mark mark );
		IEnumerable< OrderResource.TrueShipOrder > GetOrders( string organizationKey, DateTime dateFrom, DateTime dateTo, Mark mark );
		Task< bool > UpdateOrderItemPickLocations( IEnumerable< ItemLocationUpdateModel > orderitemlist, CancellationToken ct, Mark mark );
		Task< IEnumerable< OrderResource.TrueShipOrder > > GetUnshippedOrdersAsync( string organizationKey, DateTime dateTo, CancellationToken ct, Mark mark );
		Task< IEnumerable< OrganizationResource.TrueShipOrganization > > GetActiveOrganizationsAsync( CancellationToken ct, Mark mark );
	}
}