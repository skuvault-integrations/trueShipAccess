using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	/// <summary>
	///     Retrieve a list of orders.
	///     https://www.readycloud.com/static/api-doc/v2/02-apireference-v2-02-orders.html#get
	/// </summary>
	public class OrderResource : TrueShipBaseResponse< OrderResource.TrueShipOrder >
	{
		[ DataContract ]
		public class TrueShipOrder
		{
			[ DataMember( Name = "billing" ) ]
			public OrderBilling Billing { get; set; }

			[ DataMember( Name = "boxes") ]
			public List< TrueShipBox > Boxes { get; set; }

			[ DataMember( Name = "created_at" ) ]
			public string CreatedAtValue { get; set; }
			public DateTime CreatedAt
			{
				get
				{
					return CreatedAtValue.ToUtcDateTime();
				}
			}

			[ DataMember( Name = "ordered_at" ) ]
			public string OrderedAtValue { get; set; }
			public DateTime OrderedAt
			{
				get
				{
					return OrderedAtValue.ToUtcDateTime();
				}
			}

			[ DataMember( Name = "notes" ) ]
			public IEnumerable< OrderNote > Notes { get; set; }
			public string NotesText
			{
				get
				{
					return string.Join( "; ", Notes.Select( x => x.Content ) );
				}
			}

			[ DataMember( Name = "order_number" ) ]
			public string OrderNumber { get; set; }

			[ DataMember( Name = "primary_id" ) ]
			public string PrimaryId { get; set; }

			[ DataMember( Name = "url" ) ]
			public string Url { get; set; }
			public string OrderKey
			{
				get
				{
					if ( Url == null )
						return string.Empty;
					var urlParts = Url.Split( new [] { '/' }, StringSplitOptions.RemoveEmptyEntries );
					return urlParts.LastOrDefault();
				}
			}

			[ DataMember( Name = "shipping") ]
			public OrderShipping Shipping { get; set; }

			[ DataMember( Name = "unique_id" ) ]
			public string UniqueId { get; set; }

			[ DataMember( Name = "updated_at" ) ]
			public string UpdatedAtValue { get; set; }
			public DateTime UpdatedAt
			{
				get
				{
					return UpdatedAtValue.ToUtcDateTime();
				}
			}
		}
	}
}