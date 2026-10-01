using Domain.Common;
using System;

namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public DateTime? Revoked { get; set; }
        // Identity user id
        public string UserId { get; set; } = string.Empty;
    }
}
