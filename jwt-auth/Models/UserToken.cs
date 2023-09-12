using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jwt_auth.Models
{
    public enum Status { Success, InvalidToken, InvalidUser }
    public class UserToken
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string[] Role { get; set; }
        public Status Status { get; set; }
        public string Message { get; set; }
    }
}