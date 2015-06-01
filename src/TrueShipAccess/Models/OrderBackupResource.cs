namespace TrueShipAccess.Models
{
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
}