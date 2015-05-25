using System;
using System.Collections.Generic;

namespace TrueShip.Models
{
    #region order backup resource
    public class OrderBackupResource
    {
        public class Meta
        {
            public string alias_id { get; set; }
            public string backup { get; set; }
            public int id { get; set; }
            public string numerical_id { get; set; }
            public string primary_id { get; set; }
            public string resource_uri { get; set; }
        }

        public class OrderBackup
        {
            public Meta meta { get; set; }
        }

    }
    #endregion

    #region remaining orders resource
    public class RemainingOrdersResource
    {
        public int remaining_orders { get; set; }
        public string resource_uri { get; set; }
    }
    #endregion

    #region companies resource
    public class CompaniesResource
    {
        public class Meta
        {
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total_count { get; set; }
        }

        public class Object
        {
            public string company_id { get; set; }
            public string created_at { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string resource_uri { get; set; }
            public string updated_at { get; set; }
        }

        public class Companies : IEnumerable<Object>
        {
            public Meta meta { get; set; }
            public List<Object> objects { get; set; }

            public Companies()
            {
                this.objects = new List<Object>();
            }

            public IEnumerator<Object> GetEnumerator()
            {
                return objects.GetEnumerator();
            }
            
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

    }
    #endregion
    
    #region accounts resource
    public class AccountsResource
    {
        public class Account
        {
            public string avatar { get; set; }
            public string email { get; set; }
            public string first_name { get; set; }
            public int id { get; set; }
            public string last_name { get; set; }
            public string name { get; set; }
            public string resource_uri { get; set; }
        }
    }
    #endregion

    #region order resource
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
    #endregion

    #region orders list resource
    public class OrdersResource : IEnumerable<OrderResource>
    {
        public List<OrderResource> OrderList;

        public OrdersResource()
        {
            this.OrderList = new List<OrderResource>();
        }

        public IEnumerator<OrderResource> GetEnumerator()
        {
            return OrderList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    #endregion
    
    #region location update
    public class PickLocation
    {
        public string pick_location { get; set; }
    }
    #endregion

}
