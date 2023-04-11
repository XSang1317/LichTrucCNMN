using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Controllers.Authentication
{
    public class RefreshTokenViewModel
    {
        [Required, JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
