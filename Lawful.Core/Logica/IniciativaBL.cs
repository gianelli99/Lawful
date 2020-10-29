using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Datos.Strategies;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Logica
{
    public class IniciativaBL
    {
        private Datos.Interfaces.IIniciativaDAO iniciativaDAO;
        public IniciativaBL()
        {
            iniciativaDAO = new Datos.DAO.IniciativaDAO_SqlServer();
        }
        public List<string[]> ListarTipos()
        {
            try
            {
                return iniciativaDAO.ListarTipos();
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void InsertarComentario(int iniciativaID, Comentario comentario)
        {
            try
            {
                iniciativaDAO.InsertarComentario(iniciativaID, comentario);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void InsertarVoto(int userId, List<Opcion> opciones)
        {
            try
            {
                iniciativaDAO.InsertarVoto(userId, opciones);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }

        public void Insertar(Modelo.Iniciativa iniciativa)
        {
            try
            {
                switch (iniciativa.GetType().Name)
                {
                    case "Asistire":
                        iniciativaDAO.SetStrategy(new AsistireStrategy());
                        break;
                    case "DoDont":
                        if (((DoDont)iniciativa).Tipo == "Do")
                        {
                            iniciativaDAO.SetStrategy(new DoDontStrategy(2, "Like"));
                        }
                        else
                        {
                            iniciativaDAO.SetStrategy(new DoDontStrategy(3, "Dislike"));
                        }
                        
                        break;
                    case "FAQ":
                        iniciativaDAO.SetStrategy(new FAQStrategy());
                        break;
                    case "PropuestaGenerica":
                        iniciativaDAO.SetStrategy(new PropuestaGenericaStrategy());
                        break;
                    case "Regla":
                        iniciativaDAO.SetStrategy(new ReglaStrategy());
                        break;
                    case "Votacion":
                        iniciativaDAO.SetStrategy(new VotacionStrategy());
                        break;
                    case "VotacionMultiple":
                        iniciativaDAO.SetStrategy(new VotacionMultipleStrategy());
                        break;
                    default:
                        break;
                }
                iniciativaDAO.Insertar(iniciativa);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Modificar(Modelo.Iniciativa iniciativa)
        {
            try
            {
                iniciativaDAO.Modificar(iniciativa);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Eliminar(int id)
        {
            try
            {
                iniciativaDAO.Eliminar(id);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public Modelo.Iniciativa Consultar(int id)
        {
            try
            {
                return iniciativaDAO.Consultar(id);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void SeleccionarRespuestaCorrecta(int iniciativaID, int comentarioID)
        {
            try
            {
                iniciativaDAO.SeleccionarRespuestaCorrecta(iniciativaID, comentarioID);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
    }
}
