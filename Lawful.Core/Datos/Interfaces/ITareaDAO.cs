using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    public interface ITareaDAO
    {
        void Insertar(Tarea tarea, int temaId);
        void Modificar(Tarea tarea);
        void Eliminar(int tareaId);
        List<Tarea> ListarPorTema(int temaId);
        void CambiarEstado(Tarea tarea);
        void InsertarComentario(int tareaID, Comentario comentario);
    }
}
