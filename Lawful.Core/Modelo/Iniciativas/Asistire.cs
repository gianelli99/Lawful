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
        public Asistire NuevaInstancia(Usuario owner)
        {
            var instancia = new Asistire(owner);
            instancia.Opciones.Add(new Opcion() { Descripcion = "Asistiré" });
            instancia.Opciones.Add(new Opcion() { Descripcion = "No asistiré" });
            instancia.Opciones.Capacity = 2;
            return instancia;
        }

        public override string[,] GetSpecificInsert()
        {
            string[,] fields = new string[3,2];
            fields[0,0] = "FechaEvento";
            fields[0, 1] = FechaEvento.ToString();

            fields[1, 0] = "Lugar";
            fields[1, 1] = Lugar;

            fields[2, 0] = "FechaLimiteConfirmacion";
            fields[2, 1] = FechaLimiteConfirmacion.ToString();
            return fields;
        }
    }
}
