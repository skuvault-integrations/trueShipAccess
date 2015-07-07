using System.Collections.Generic;

namespace TrueShipAccess.Models
{
	/// <summary>
	/// Box Item Resource
	/// https://www.readycloud.com/static/api-doc/apireference.html#box-item-resource
	/// </summary>
	public class ItemsResource
	{
		public class Response
		{
			public Meta Meta { get; set; }
			public List<Item> Objects { get; set; }
		}
	}
}