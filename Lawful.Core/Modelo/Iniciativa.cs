using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public abstract class Iniciativa
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string IconName { get; set; }
        public bool EveryoneCanEdit { get; set; }
        public Tema Tema { get; set; }
        public List<Comentario> Comentarios { get; set; }

        public readonly Usuario Owner;
        public Iniciativa(Usuario owner)
        {
            Owner = owner;
            Comentarios = new List<Comentario>();
        }
        public abstract string[,] GetSpecificInsert();
    }
}
