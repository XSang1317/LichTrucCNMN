using Newtonsoft.Json;
using LichTruc.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LichTruc.Controllers.Authentication
{
    public class TokenViewModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int Status { get; set; }


        //public IEnumerable<Department> Departments { get; set; }

        public int DepartmentId { get; set; }
    }
}
