using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class Asistire : Iniciativa
    {
        public DateTime FechaEvento { get; set; }
        public string Lugar { get; set; }
        public DateTime FechaLimiteConfirmacion { get; set; }
        public List<Opcion> Opciones { get; set; }
        public Asistire(Usuario owner)
        : base(owner)
        {
            Opciones = new List<Opcion>();
        }
        public static Asistire NuevaInstancia(Usuario owner)
        {
            var instancia = new Asistire(owner);
            instancia.Opciones.Add(new Opcion() { Descripcion = "Asistiré" });
            instancia.Opciones.Add(new Opcion() { Descripcion = "No asistiré" });
            instancia.Opciones.Capacity = 2;
            return instancia;
        }
        public override string GetIniciativaType()
        {
            return "Asistiré";
        }
        public override bool UserHasVoted(int userId)
        {
            foreach (var item in Opciones)
            {
                if (item.Votantes.FindIndex(x=>x.ID == userId) != -1)
                {
                    return true;
                }
            }
            return false;
        }
        public override List<Opcion> OptionsVoted(int userId)
        {
            var opciones = new List<Opcion>();
            foreach (var item in Opciones)
            {
                if (item.Votantes.FindIndex(x => x.ID == userId) != -1)
                {
                    opciones.Add(item);
                }
            }
            return opciones;
        }
    }
}
