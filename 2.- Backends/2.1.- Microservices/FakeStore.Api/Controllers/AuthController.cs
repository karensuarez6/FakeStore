
using FakeStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        public class RegistroRequest
        {
            public string NombreUsuario { get; set; } = string.Empty;
            public string Correo { get; set; } = string.Empty;
            public string Contrasena { get; set; } = string.Empty;
        }

        public class LoginRequest
        {
            public string NombreUsuario { get; set; } = string.Empty;
            public string Contrasena { get; set; } = string.Empty;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar(
            [FromBody] RegistroRequest request)
        {
            try
            {
                var resultado = await _authService.Registrar(
                    request.NombreUsuario,
                    request.Correo,
                    request.Contrasena);

                return Ok(new { mensaje = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> IniciarSesion(
            [FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.IniciarSesion(
                    request.NombreUsuario,
                    request.Contrasena);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
        }
    }
}
