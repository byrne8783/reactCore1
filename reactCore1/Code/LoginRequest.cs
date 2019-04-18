using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactCore1.Web
{

    public class LoginRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public LoginResponse(ApplicationUser aU) { UserId = aU.UserName;Name = aU.Name; }
        public readonly string UserId;
        public string Name { get; private set; }
    }
    public class ResponseData<T> where T:class
    {
        public ResponseData(string id, T data)
        {
            Id = id ?? "";
            Data = data;
        }
        public readonly string Id;
        public readonly T Data;
    }
}
