using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    /// <summary>
    /// For login to API, convert to json
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// annv3 15/08/2022 created
    /// </Modified>
    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
