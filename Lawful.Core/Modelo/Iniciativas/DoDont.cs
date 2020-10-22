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
        public static DoDont NuevaInstancia(Usuario owner)
        {
            var instancia = new DoDont(owner);
            instancia.Opciones.Add(new Opcion() { Descripcion = "Si" });
            instancia.Opciones.Add(new Opcion() { Descripcion = "No" });
            instancia.Opciones.Capacity = 2;
            return instancia;
        }
        public override string GetIniciativaType()
        {
            if (Tipo == "Do")
            {
                return "Do";
            }
            else if (Tipo == "Don't")
            {
                return "Don't";
            }
            else
            {
                return "Do or Don't";
            }
        }
    }
}
