using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using TrueShipAccess.Models;

namespace TrueShipAccess.Extensions
{
	public static class TrueShipListExtensions
	{
		private static string ListToString( IEnumerable<string> list )
		{
			return string.Join( ",", list );
		}

			public static string MakeString< T >( this List<T> list )
			{
				if ( typeof( T ) == typeof( OrderResource.TrueShipOrder ) )
				{
					return ListToString( ( list as List< OrderResource.TrueShipOrder > ).Select( o => o.PrimaryId ) );
				}
				if ( typeof( T ) == typeof( TrueShipBox ) )
				{
					return ListToString( ( list as List<TrueShipBox> ).Select( b =>
					{
						var removedPrefix = "/api/v1/boxes/";
						int index = b.ResourceUri.IndexOf( removedPrefix, StringComparison.Ordinal );
						string cleanedResources = ( index < 0 )
							? b.ResourceUri
							: b.ResourceUri.Remove( index, removedPrefix.Length );
						return cleanedResources.Length > 0 ? cleanedResources.Remove( cleanedResources.Length - 1 ) : cleanedResources;
					} ) );
				}
				if ( typeof( T ) == typeof( TrueShipItem ) )
				{
					return ListToString( ( list as List< TrueShipItem > ).Select( i => i.ItemId ) );
				}
				if ( typeof( T ) == typeof( ItemLocationUpdateModel ) )
				{
					return ListToString( ( list as List< ItemLocationUpdateModel > ).Select( i => "( {0} -> SKU = {1}, Location = {2} )".FormatWith( i.Resource, i.Sku, i.Location.Location ) ) );
				}
				if ( typeof( T ) == typeof( Company ) )
				{
					return ListToString( ( list as List< Company > ).Select( c => "{0} - {1}".FormatWith( c.CompanyId, c.Name ) ) );
				}
				return list.ToString();
			}
	}
}
