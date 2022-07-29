namespace OutlookSyncApi.Models
{
    public class GraphConfiguration
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string Scopes { get; set; }
    }
}

