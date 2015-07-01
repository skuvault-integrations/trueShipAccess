namespace TrueShipAccess.Models
{
    public class OrderBackupResource
    {
        public class OrderBackupResponse
        {
            public string alias_id { get; set; }
            public string Backup { get; set; }
            public int Id { get; set; }
            public string numerical_id { get; set; }
            public string primary_id { get; set; }
            public string resource_uri { get; set; }
        }

        public class OrderBackup
        {
            public OrderBackupResponse meta { get; set; }
        }
    }
}