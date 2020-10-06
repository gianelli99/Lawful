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
        }
        public DoDont NuevaInstancia(Usuario owner)
        {
            var instancia = new DoDont(owner);
            instancia.Opciones.Add(new Opcion() { Descripcion = "Si" });
            instancia.Opciones.Add(new Opcion() { Descripcion = "No" });
            instancia.Opciones.Capacity = 2;
            return instancia;
        }
    }
}
