using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    interface IIniciativaDAO
    {
        IQueryStrategy Strategy { get; set; }
        void SetStrategy(IQueryStrategy strategy);
        List<Modelo.Iniciativa> ListarPorTema(int temaId);
        List<Modelo.Iniciativa> ListarPorUsuario(int userId);
        void Insertar(Iniciativa iniciativa);
        void Eliminar(int id);
        void Modificar(Iniciativa iniciativa);
        Modelo.Iniciativa Consultar(int id);
        List<string[]> ListarTipos();
        void InsertarComentario(int iniciativaID, Comentario comentario);
        void InsertarVoto(int userID, List<Opcion> opciones);
    }
}
