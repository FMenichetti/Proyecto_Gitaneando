using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaDominio;

namespace CapaNegocio
{
    public class NegocioMarcas
    {


        DatosMarca datos = new DatosMarca();

        public List<Marca> Listar()
        {
            return datos.listar();
        }

        public int RegistrarMarca_sp(Marca marca, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(marca.Descripcion) || string.IsNullOrWhiteSpace(marca.Descripcion))
                mensaje = "El campo de Marca no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {

                return datos.RegistrarMarca_sp(marca, out mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarMarca_Sp(Marca marca, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(marca.Descripcion) || string.IsNullOrWhiteSpace(marca.Descripcion))
                mensaje = "El campo de Marca no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {

                return datos.EditarMarca_Sp(marca, out mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarMarca_sp(int id, out string mensaje)
        {
            return datos.EliminarMarca_sp(id, out mensaje);
        }

        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {
            return datos.ListarMarcaPorCategoria(idCategoria);
        }

    }
}
