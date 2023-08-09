using CapaDatos;
using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NegocioCliente
    {
        DatosCliente datos = new DatosCliente();

        public int RegistrarClienteConSP(Cliente cliente, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Nombre))
                mensaje = "El campo de nombre no puede estar vacio";
            else if (string.IsNullOrEmpty(cliente.Apellido) || string.IsNullOrWhiteSpace(cliente.Apellido))
                mensaje = "El campo de apellido no puede estar vacio";
            else if (string.IsNullOrEmpty(cliente.Email) || string.IsNullOrWhiteSpace(cliente.Email))
                mensaje = "El campo de email no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {
                cliente.Pass = NegocioRecursos.ConvertirSHA256(cliente.Pass);
                return datos.RegistrarClienteConSP(cliente, out mensaje);
            }
            else
            {
                return 0;
            }

        }
        public List<Cliente> Listar()
        {
            return datos.listar();
        }

        public bool CambiarClave(int idCliente, string nuevaClave, out string mensaje)
        {
            return datos.CambiarClave(idCliente, nuevaClave, out mensaje);
        }
        public bool RestablecerClave(int idCliente, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaClave = NegocioRecursos.GenerarClave();
            bool resultado = datos.RestablecerClave(idCliente, NegocioRecursos.ConvertirSHA256(nuevaClave), out mensaje);


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
