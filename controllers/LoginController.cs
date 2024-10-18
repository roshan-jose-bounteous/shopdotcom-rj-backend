
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
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
