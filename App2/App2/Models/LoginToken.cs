using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    public class LoginToken
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Validaty { get; set; }
        public object RefreshToken { get; set; }
        public string Id { get; set; }
        public object EmailId { get; set; }
        public string GuidId { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
