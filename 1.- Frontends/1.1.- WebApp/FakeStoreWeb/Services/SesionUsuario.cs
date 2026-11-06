
namespace FakeStoreWeb.Services
{
    public class SesionUsuario
    {
        private string? _token;

        public bool EstaAutenticado =>
            !string.IsNullOrWhiteSpace(_token);

        public void IniciarSesion(string token)
        {
            _token = token;
        }

        public string? ObtenerToken()
        {
            return _token;
        }

        public void CerrarSesion()
        {
            _token = null;
        }
    }
}
