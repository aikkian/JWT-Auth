using jwt_auth.Logics;
using jwt_auth.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace jwt_auth.Controllers
{
    [RoutePrefix("Authentication")]
    public class AuthenticationController : ApiController
    {
        [Route("UserLogin")]
        [HttpPost]
        public IHttpActionResult UserLogin(User user)
        {
            // This is a simple login access, you can change it by implementing your login access using EF or Ado.Net
            if (user.Username == "suelynn" && user.Password == "abc123")
            {
                // From the username get the user's role from DB using EF or Ado.Net, in jwt we can assign multiple roles.
                // This is a common practice, especially when a user has multiple roles within an application.
                string[] role = { "Senior Software Engineer", "Product Owner" };

                UserToken userToken = new UserToken()
                {
                    Status = Status.Success,
                    Message = "User is valid",
                    Role = role,
                    Token = TokenManager.GenerateToken(user.Username, role),
                    Username = user.Username,
                };

                return Ok(userToken);
            }
            else
            {
                UserToken userToken = new UserToken()
                {
                    Status = Status.InvalidUser,
                    Message = "User is invalid",
                };

                return Content(HttpStatusCode.Unauthorized, userToken);
            }
        }

        [Route("Validate")]
        [HttpPost]
        public IHttpActionResult Validate()
        {
            try
            {
                IEnumerable<string> headerValues = new List<string>();
                Request.Headers.TryGetValues("Authorization", out headerValues);

                if (headerValues != null)
                {
                    string token = headerValues.FirstOrDefault();

                    if (token.StartsWith("Bearer"))
                    {
                        token = token.Substring("Bearer ".Length).Trim();
                        var tokenInfo = TokenManager.ValidateToken(token);

                        if (tokenInfo != null)
                        {
                            return Ok(new UserToken
                            {
                                Status = Status.Success,
                                Message = "Token is valid",
                                Token = token,
                                Username = tokenInfo.Item1,
                                Role = tokenInfo.Item2
                            });
                        }
                        else
                        {
                            throw new ArgumentException("Invalid Token");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error, it could be token expired or invalid
                Debug.WriteLine(ex.ToString());
            }

            UserToken userToken = new UserToken()
            {
                Status = Status.InvalidToken,
                Message = "Token is invalid",
            };

            return Content(HttpStatusCode.Unauthorized, userToken);
        }
    }
}
