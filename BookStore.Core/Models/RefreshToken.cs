namespace BookStore.Core.Models
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime CreateOn { get; set; } = DateTime.UtcNow;
        public DateTime ExpireOn { get; set; }
        public DateTime? RevokeOn { get; set; }
        public bool IsExpire => DateTime.UtcNow >= ExpireOn;
        public bool IsActive => RevokeOn == null && !IsExpire;
    }
}
