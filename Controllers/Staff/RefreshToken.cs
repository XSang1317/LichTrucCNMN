using Microsoft.Identity.Client;

namespace LichTruc.Controllers.Staff
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; }  = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
