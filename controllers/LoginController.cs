
using Microsoft.AspNetCore.Mvc;
using Supabase;
using Supabase.Gotrue;
using System.Threading.Tasks;
using Supabase.Gotrue.Exceptions;

namespace shopdotcobackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;



        public LoginController(Supabase.Client SupabaseClient)
        {
            _supabaseClient = SupabaseClient;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var response = await _supabaseClient.Auth.SignInWithPassword(request.Email, request.Password);

            if (response == null || string.IsNullOrEmpty(response.AccessToken))
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                access_token = response.AccessToken,
                refresh_token = response.RefreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request)
        {

            Console.WriteLine($"Email Request: {Newtonsoft.Json.JsonConvert.SerializeObject(request.Email)}");
            Console.WriteLine($"Password Request: {Newtonsoft.Json.JsonConvert.SerializeObject(request.Password)}");

            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            // var exists = await _supabaseClient.Auth.SignInWithPassword(request.Email, request.Password);

            // if (exists != null || exists.User != null)
            // {
            //     return Conflict("This email is already registered.");
            // }

        //     var options = new Supabase.Gotrue.SignUpOptions
        //     {
        //         Data = new Dictionary<string, object>
        // {
        //     { "display_name", request.DisplayName }
        // }
        //     };

            // var response = await _supabaseClient.Auth.SignUp(Supabase.Gotrue.Constants.SignUpType.Email, request.Email, request.Password, options);

            // if (response == null || string.IsNullOrEmpty(response.AccessToken))
            // {
            //     return Unauthorized("Invalid email or password.");
            // }

            // return Ok(new { message = "User registered successfully", display_name = request.DisplayName });


            var response = await _supabaseClient.Auth.SignUp(request.Email, request.Password);

            if (response == null || string.IsNullOrEmpty(response.AccessToken))
            {
                return Unauthorized("Invalid email or password.");
            }

            // // return Ok(new 
            // // { 
            // //     access_token = response.AccessToken, 
            // //     refresh_token = response.RefreshToken 
            // // });
            return Ok(new { message = "User registered successfully" });
        }





    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
