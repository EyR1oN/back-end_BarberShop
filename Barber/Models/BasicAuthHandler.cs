using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Security.Claims;
using System.Data;
using MySql.Data.MySqlClient;
using Barber.Calculations;

namespace BasicAuthWebAPI.Helpers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        IConfiguration _configuration;
        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialsByes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialsByes).Split(':');
                string password;

                try
                {
                    string query = @"
                select * from user where username = @username
            ";

                    DataTable table = new DataTable();
                    string sqlDataSource = _configuration.GetConnectionString("OrdersAppCon");
                    MySqlDataReader myReader;
                    using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                    {
                        mycon.Open();
                        using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                        {

                            myCommand.Parameters.AddWithValue("@username", credentials[0]);

                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);

                            myReader.Close();
                            mycon.Close();

                            password = table.Rows[0]["password"].ToString();


                        }
                    }
                    
                    
                }
                catch
                {
                    return AuthenticateResult.Fail("No such user in database");
                }



                if (Password.VerifyPassword(password, credentials[1]))
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, credentials[0])
                    };

                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);

                }
                else
                {
                    return AuthenticateResult.Fail("Invalid Credentaisl ");
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}