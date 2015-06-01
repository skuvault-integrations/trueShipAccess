using System;
using System.Collections.Generic;

namespace TrueShipAccess.Models
{
    public class OrderResource : IEnumerable<OrderResource.Box>
    {
        public Dictionary<string,string>bill_to_dict { get; set; }
        public class Box
        {
            public Dictionary<string, string> boxes_dict { get; set; }
            public Dictionary<string, string> boxes_charges_dict { get; set; }
            public Dictionary<string, Dictionary<string, string>> boxes_items_dict { get; set; }
        }
        public List<Box> Boxes { get; set; }
        public Dictionary<string, string> charges_dict { get; set; }
        public string created_at { get; set; }
        public string customer_number { get; set; }
        public string future_ship_at { get; set; }
        public string imported_at { get; set; }
        public string message { get; set; }
        public string numerical_id { get; set; }
        public string order_time { get; set; }
        public string po_number { get; set; }
        public string primary_id { get; set; }
        public string printed_at { get; set; }
        public string resource_uri { get; set; }
        public int revision_id { get; set; }
        public string revisions_resource_uri;
        public Dictionary<string, string>ship_from_dict { get; set; }
        public string ship_time { get; set; }
        public Dictionary<string, string>ship_to_dict { get; set; }
        public string ship_type { get; set; }
        public string ship_via { get; set; }
        public string source { get; set; }
        public Dictionary<string, string>sources_dict { get; set; }
        public Boolean status_shipped { get; set; }
        public string terms { get; set; }
        public string unique_id { get; set; }
        public string updated_at { get; set; }
        public string warehouse { get; set; }
        public OrderResource()
        {
            this.bill_to_dict = new Dictionary<string,string>();
            this.Boxes = new List<Box>();
            this.charges_dict = new Dictionary<string, string>();
            this.ship_from_dict = new Dictionary<string, string>();
            this.ship_to_dict = new Dictionary<string, string>();
            this.sources_dict = new Dictionary<string, string>();
        }
        public IEnumerator<OrderResource.Box> GetEnumerator()
        {
            return Boxes.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}