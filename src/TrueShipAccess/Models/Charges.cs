using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class Charges
	{
		[ DataMember( Name = "actual_ship_cost" ) ]
		public string ActualShipCost{ get; set; }

		[ DataMember( Name = "declared_value" ) ]
		public string DeclaredValue{ get; set; }

		[ DataMember( Name = "insured_value" ) ]
		public string InsuredValue{ get; set; }
		
		[ DataMember( Name = "calculated_grand_total" ) ]
		public string CalculatedGrandTotal{ get; set; }
		
		[ DataMember( Name = "calculated_shipping" ) ]
		public string CalculatedShipping{ get; set; }
		
		[ DataMember( Name = "calculated_tax" ) ]
		public string CalculatedTax{ get; set; }
		
		[ DataMember( Name = "calculated_total" ) ]
		public string CalculatedTotal{ get; set; }
		
		[ DataMember( Name = "grand_total" ) ]
		public string GrandTotal{ get; set; }
		
		[ DataMember( Name = "grand_total_source" ) ]
		public string GrandTotalSource{ get; set; }
		
		[ DataMember( Name = "imported_grand_total" ) ]
		public string ImportedGrandTotal{ get; set; }
		
		[ DataMember( Name = "imported_shipping" ) ]
		public string ImportedShipping{ get; set; }
		
		[ DataMember( Name = "imported_tax" ) ]
		public string ImportedTax{ get; set; }
		
		[ DataMember( Name = "imported_total" ) ]
		public string ImportedTotal{ get; set; }

		public string Shipping{ get; set; }
	
		[ DataMember( Name = "shipping_source" ) ]
		public string ShippingSource{ get; set; }
		
		public string Tax{ get; set; }
		[ DataMember( Name = "tax_source" ) ]
		public string TaxSource{ get; set; }
		
		public string Total{ get; set; }
		
		[ DataMember( Name = "total_source" ) ]
		public string TotalSource{ get; set; }
	}
}