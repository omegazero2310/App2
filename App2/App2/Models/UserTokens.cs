namespace App2.Models
{
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