using System.Collections.Generic;

namespace TrueShipAccess.Models
{
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
}