
using Entities;
using FakeStore.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FakeStore.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Registrar(
            string nombreUsuario,
            string correo,
            string contrasena)
        {
            bool existe = await _context.Usuarios.AnyAsync(u =>
                u.NombreUsuario == nombreUsuario ||
                u.Correo == correo);

            if (existe)
                throw new Exception("El usuario o correo ya existe.");

            string hash = HashPassword(contrasena);

            var usuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                Correo = correo,
                PasswordHash = hash,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return "Usuario registrado correctamente.";
        }

        public async Task<string> IniciarSesion(
            string nombreUsuario,
            string contrasena)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.NombreUsuario == nombreUsuario && u.Activo);

            if (usuario == null ||
                usuario.PasswordHash != HashPassword(contrasena))
            {
                throw new Exception("Usuario o contraseña incorrectos.");
            }

            string clave = _configuration["KeyJWT"]
                ?? throw new InvalidOperationException(
                    "Falta configurar KeyJWT.");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(clave));

            var credenciales = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Correo)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string HashPassword(string contrasena)
        {
            using var sha256 = SHA256.Create();

            byte[] hash = sha256.ComputeHash(
                Encoding.UTF8.GetBytes(contrasena));

            return Convert.ToBase64String(hash);
        }
    }
}
