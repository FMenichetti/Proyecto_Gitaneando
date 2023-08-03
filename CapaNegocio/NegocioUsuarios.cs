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
        DatosUsuario datos = new DatosUsuario();

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
                string clave = NegocioRecursos.GenerarClave();

                string asunto = "Creacion de cuenta";
                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);

                    bool respuesta = NegocioRecursos.EnviarCorreo(user.Email, asunto, mensajeCorreo);

                if (respuesta)
                {
                    user.Pass = NegocioRecursos.ConvertirSHA256(clave);
                    return datos.RegistrarUsuarioConSP(user, out mensaje);
                }
                else
                {

                mensaje = "No se pudo enviar el correo";
                return 0;
                }

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

        public bool CambiarClave(int idUsuario, string nuevaClave, out string mensaje)
        {
            return datos.CambiarClave(idUsuario, nuevaClave, out mensaje);
        }
        public bool RestablecerClave(int idUsuario, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaClave = NegocioRecursos.GenerarClave();
            bool resultado = datos.RestablecerClave(idUsuario, NegocioRecursos.ConvertirSHA256(nuevaClave),out mensaje);
        

            if (resultado)
            {
                string asunto = "Contraseña restablecida";
                string mensajeCorreo = "<h3>Su contraseña fue restablecida correctamente</h3></br><p>Su  nueva contraseña para acceder es: !clave! , al ingresar al sistema se le va a pedir cambiarla.</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaClave);

                bool respuesta = NegocioRecursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return false;
                }

            }
            else
            {
                mensaje = "No se pudo restablecer la contraseña";
                return false;
            }

        }



    }
}
