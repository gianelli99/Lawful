using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Tema Tema { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public readonly Usuario Owner;
        public DateTime FechaCierre { get; set; }
        public Iniciativa(Usuario owner)
        {
            Owner = owner;
            Comentarios = new List<Comentario>();
        }
        public virtual string GetIniciativaType()
        {
            return this.GetType().Name;
        }
        public virtual bool UserHasVoted(int userId)
        {
            return false;
        }
        public virtual List<Opcion> OptionsVoted(int userId)
        {
            return null;
        }
        public string GetState()
        {
            if (FechaCierre.Date < DateTime.Now.Date)
            {
                return "Cerrada";
            }
            else
            {
                return "Abierta";
            }
        }
        public bool isOpen()
        {
            if (FechaCierre.Date < DateTime.Now.Date)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
