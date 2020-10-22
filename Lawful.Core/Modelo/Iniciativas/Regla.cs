using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class Regla : Iniciativa
    {
        public int Relevancia { get; set; }
        public List<Opcion> Opciones { get; set; }
        public Regla(Usuario owner)
        : base(owner)
        {
            Opciones = new List<Opcion>();
            Relevancia = 1;
        }
        public static Regla NuevaInstancia(Usuario owner)
        {
            var instancia = new Regla(owner);
            instancia.Opciones.Add(new Opcion() { Descripcion = "Acepto" });
            instancia.Opciones.Add(new Opcion() { Descripcion = "No acepto" });
            instancia.Opciones.Capacity = 2;
            instancia.Relevancia = 1;
            return instancia;
        }
        public override string GetIniciativaType()
        {
            return "Regla";
        }
    }
}
