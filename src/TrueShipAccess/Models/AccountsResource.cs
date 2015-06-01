namespace TrueShipAccess.Models
{
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
}
