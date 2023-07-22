using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaDominio;


namespace CapaNegocio
{
    public class NegocioUsuarios
    {
        AccesoDatos datos = new AccesoDatos();
        
        public List<Usuario> Listar()
        {
            return datos.listar();
        }
        public int RegistrarUsuarioConSP(Usuario user, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(user.Nombre) || string.IsNullOrWhiteSpace(user.Nombre))
                mensaje = "El campo de nombre no puede estar vacio";
            else if (string.IsNullOrEmpty(user.Apellido) || string.IsNullOrWhiteSpace(user.Apellido))
                mensaje = "El campo de nombre no puede estar vacio";
            else if (string.IsNullOrEmpty(user.Email) || string.IsNullOrWhiteSpace(user.Email))
                mensaje = "El campo de nombre no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {

            return datos.RegistrarUsuarioConSP(user, out mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarUsuarioConSp(Usuario user, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(user.Nombre) || string.IsNullOrWhiteSpace(user.Nombre))
                mensaje = "El campo de nombre no puede estar vacio";
            else if (string.IsNullOrEmpty(user.Apellido) || string.IsNullOrWhiteSpace(user.Apellido))
                mensaje = "El campo de nombre no puede estar vacio";
            else if (string.IsNullOrEmpty(user.Email) || string.IsNullOrWhiteSpace(user.Email))
                mensaje = "El campo de nombre no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {

                return datos.EditarUsuarioConSp(user, out mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int id, out string mensaje)
        {
            return datos.Eliminar(id, out mensaje);
        }
    }
}
