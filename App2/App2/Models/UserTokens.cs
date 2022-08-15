namespace App2.Models
{
    /// <summary>Class to convert login respone from web api json to instance object C#</summary>
    /// <Modified>
    /// Name Date Comments
    /// annv3 15/08/2022 created
    /// </Modified>
    public class UserTokens
    {
        public string Token
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public System.TimeSpan Validaty
        {
            get;
            set;
        }
        public string RefreshToken
        {
            get;
            set;
        }
        public System.Guid Id
        {
            get;
            set;
        }
        public string EmailId
        {
            get;
            set;
        }
        public System.Guid GuidId
        {
            get;
            set;
        }
        public System.DateTime ExpiredTime
        {
            get;
            set;
        }
    }
}