using System.Data.SqlClient;
using Dapper;

namespace TPMOGUE.Models
{
    public static class BD
    {
        private static string _connectionString = @"Server=localhost;DataBase=DBRedsocial;Trusted_Connection=True;";

        public static bool ExisteUsuario(string nombreUsuario)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario";

                Usuario usuario = db.QueryFirstOrDefault<Usuario>(
                    sql,
                    new { NombreUsuario = nombreUsuario }
                );

                return usuario != null;
            }
        }

        public static void RegistrarUsuario(Usuario usuario)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Usuarios (NombreUsuario, Contraseña, Nombre, Apellido) VALUES (@NombreUsuario, @Contraseña, @Nombre, @Apellido)";

                db.Execute(sql, usuario);
            }
        }

        public static Usuario Login(string nombreUsuario, string contraseña)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Contraseña = @Contraseña";

                return db.QueryFirstOrDefault<Usuario>(
                    sql,
                    new
                    {
                        NombreUsuario = nombreUsuario,
                        Contraseña = contraseña
                    }
                );
            }
        }

        public static Usuario ObtenerUsuario(int idUsuario)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Usuarios WHERE Id = @Id";

                return db.QueryFirstOrDefault<Usuario>(
                    sql,
                    new { Id = idUsuario }
                );
            }
        }

        public static void CrearPublicacion(Publicacion publicacion)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Publicaciones (IdUsuario, Titulo, Descripcion, Imagen, FechaPublicacion) VALUES (@IdUsuario, @Titulo, @Descripcion, @Imagen, GETDATE())";

                db.Execute(sql, publicacion);
            }
        }

        public static List<Publicacion> ObtenerPublicaciones(int desde)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Publicaciones ORDER BY FechaPublicacion DESC OFFSET @Desde ROWS FETCH NEXT 10 ROWS ONLY";

                return db.Query<Publicacion>(
                    sql,
                    new { Desde = desde }
                ).ToList();
            }
        }

        public static bool ExistePublicacion(int idPublicacion)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Publicaciones WHERE Id = @Id";

                Publicacion publicacion = db.QueryFirstOrDefault<Publicacion>(
                    sql,
                    new { Id = idPublicacion }
                );

                return publicacion != null;
            }
        }

        public static bool UsuarioDioMeGusta(int idPublicacion, int idUsuario)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM PublicacionesMeGusta WHERE IdPublicación = @IdPublicacion AND IdUsuario = @IdUsuario";

                PublicacionMeGusta meGusta = db.QueryFirstOrDefault<PublicacionMeGusta>(
                    sql,
                    new
                    {
                        IdPublicacion = idPublicacion,
                        IdUsuario = idUsuario
                    }
                );

                return meGusta != null;
            }
        }

        public static void DarMeGusta(int idPublicacion, int idUsuario)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO PublicacionesMeGusta (IdPublicación, IdUsuario) VALUES (@IdPublicacion, @IdUsuario)";

                db.Execute(
                    sql,
                    new
                    {
                        IdPublicacion = idPublicacion,
                        IdUsuario = idUsuario
                    }
                );
            }
        }

        public static void QuitarMeGusta(int idPublicacion, int idUsuario)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM PublicacionesMeGusta WHERE IdPublicación = @IdPublicacion AND IdUsuario = @IdUsuario";

                db.Execute(
                    sql,
                    new
                    {
                        IdPublicacion = idPublicacion,
                        IdUsuario = idUsuario
                    }
                );
            }
        }

        public static int CantidadMeGusta(int idPublicacion)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(*) FROM PublicacionesMeGusta WHERE IdPublicación = @IdPublicacion";

                return db.QueryFirstOrDefault<int>(
                    sql,
                    new { IdPublicacion = idPublicacion }
                );
            }
        }

        public static List<Comentario> ObtenerComentarios(int idPublicacion)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Comentarios WHERE IdPublicacion = @IdPublicacion ORDER BY FechaComentario";

                return db.Query<Comentario>(
                    sql,
                    new { IdPublicacion = idPublicacion }
                ).ToList();
            }
        }

        public static Comentario AgregarComentario(int idPublicacion, int idUsuario, string texto)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Comentarios (IdPublicacion, IdUsuarioComenta, Texto, FechaComentario) OUTPUT INSERTED.* VALUES (@IdPublicacion, @IdUsuario, @Texto, GETDATE())";

                return db.QueryFirstOrDefault<Comentario>(
                    sql,
                    new
                    {
                        IdPublicacion = idPublicacion,
                        IdUsuario = idUsuario,
                        Texto = texto
                    }
                );
            }
        }
    }
}