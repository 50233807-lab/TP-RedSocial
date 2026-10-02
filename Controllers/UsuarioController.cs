using Microsoft.AspNetCore.Mvc;
using TPMOGUE.Models;

namespace TPMOGUE.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            if (BD.ExisteUsuario(usuario.NombreUsuario))
            {
                ViewBag.Error = "El nombre de usuario ya existe";
                return View();
            }

            BD.RegistrarUsuario(usuario);

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string nombreUsuario, string contraseña)
        {
            Usuario usuario = BD.Login(nombreUsuario, contraseña);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }

            HttpContext.Session.SetInt32("IdUsuario", usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);

            return RedirectToAction("Index", "Publicacion");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}