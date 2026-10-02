using Microsoft.AspNetCore.Mvc;
using TPMOGUE.Models;

namespace TPMOGUE.Controllers
{
    public class PublicacionController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            List<Publicacion> publicaciones = BD.ObtenerPublicaciones(0);

            return View(publicaciones);
        }

        public IActionResult Crear()
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Crear(string titulo, string descripcion, IFormFile imagen)
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            int idUsuario = HttpContext.Session.GetInt32("IdUsuario").GetValueOrDefault();

            string nombreImagen = imagen.FileName;

            string ruta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "img",
                nombreImagen
            );

            using (FileStream archivo = new FileStream(ruta, FileMode.Create))
            {
                imagen.CopyTo(archivo);
            }

            Publicacion publicacion = new Publicacion();

            publicacion.IdUsuario = idUsuario;
            publicacion.Titulo = titulo;
            publicacion.Descripcion = descripcion;
            publicacion.Imagen = nombreImagen;

            BD.CrearPublicacion(publicacion);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult MeGusta(int idPublicacion)
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
            {
                return Json(new { error = true });
            }

            int idUsuario = HttpContext.Session.GetInt32("IdUsuario").GetValueOrDefault();

            if (BD.ExistePublicacion(idPublicacion) == false)
            {
                return Json(new { error = true });
            }

            if (BD.UsuarioDioMeGusta(idPublicacion, idUsuario))
            {
                BD.QuitarMeGusta(idPublicacion, idUsuario);
            }
            else
            {
                BD.DarMeGusta(idPublicacion, idUsuario);
            }

            int cantidad = BD.CantidadMeGusta(idPublicacion);

            bool tieneMeGusta = BD.UsuarioDioMeGusta(idPublicacion, idUsuario);

            return Json(new
            {
                error = false,
                cantidad = cantidad,
                tieneMeGusta = tieneMeGusta
            });
        }

        [HttpPost]
        public IActionResult Comentar(int idPublicacion, string texto)
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
            {
                return Json(new { error = true });
            }

            int idUsuario = HttpContext.Session.GetInt32("IdUsuario").GetValueOrDefault();

            if (BD.ExistePublicacion(idPublicacion) == false)
            {
                return Json(new { error = true });
            }

            if (texto == null || texto.Trim() == "")
            {
                return Json(new { error = true });
            }

            Comentario comentario = BD.AgregarComentario(
                idPublicacion,
                idUsuario,
                texto
            );

            Usuario usuario = BD.ObtenerUsuario(idUsuario);

            return Json(new
            {
                error = false,
                nombreUsuario = usuario.NombreUsuario,
                texto = comentario.Texto,
                fecha = comentario.FechaComentario.ToString("dd/MM/yyyy HH:mm")
            });
        }

        [HttpGet]
        public IActionResult ObtenerMas(int desde)
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
            {
                return Json(new { error = true });
            }

            List<Publicacion> publicaciones = BD.ObtenerPublicaciones(desde);

            return Json(publicaciones);
        }
    }
}