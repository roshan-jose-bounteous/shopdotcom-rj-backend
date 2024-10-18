// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using shopdotcobackend.Services;
// using Supabase;

// namespace shopdotcobackend.controllers
// { [ApiController]
//     [Route("api/[controller]")]
//     public class LoginController : ControllerBase
//     {
//         private readonly SupabaseService _authService;

//         public LoginController(SupabaseService authService)
//         {
//             _authService = authService;
//         }

//         [HttpPost("login")]
//         public async Task<IActionResult> Login([FromBody] LoginRequest request)
//         {
//             if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
//             {
//                 return BadRequest("Email and password are required.");
//             }

//                         var response = await _authService.Auth.SignInWithPassword(request.Email, request.Password);
//             // return response;

//             // var session = await _authService.LoginAsync(request.Email, request.Password);
//             // if (session == null)
//             // {
//             //     return Unauthorized("Invalid email or password.");
//             // }
//             return Ok(response);
//             // return Ok(new { access_token = session.AccessToken, refresh_token = session.RefreshToken });
//         }
//     }

//     public class LoginRequest
//     {
//         public string Email { get; set; }
//         public string Password { get; set; }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using Supabase;
using Supabase.Gotrue;
using System.Threading.Tasks;

namespace shopdotcobackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;

        // public LoginController(IConfiguration configuration)
        // {
        //     var url = configuration["Supabase:Url"];
        //     var key = configuration["Supabase:ApiKey"];
        //     var options = new SupabaseOptions { AutoRefreshToken = true };
        //     _supabaseClient = new Supabase.Client(url, key, options);
        //     _supabaseClient.InitializeAsync().Wait();
        // }

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

            // Sign in with email and password
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
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
