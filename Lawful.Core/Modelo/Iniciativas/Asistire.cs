﻿using System;
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
    }
}
