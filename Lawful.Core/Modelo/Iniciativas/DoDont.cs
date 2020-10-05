using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class DoDont : Iniciativa
    {
        public string Tipo { get; set; }
        public List<Opcion> Opciones { get; set; }
        public DoDont(Usuario owner)
        :base(owner)
        {
            Opciones = new List<Opcion>();
            Opciones.Add(new Opcion() { Descripcion = "Si"});
            Opciones.Add(new Opcion() { Descripcion = "No"});
            Opciones.Capacity = 2;
        }
    }
}
