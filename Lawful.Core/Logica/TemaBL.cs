﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Logica
{
    public class TemaBL
    {
        private Datos.Interfaces.ITemaDAO temaDAO;
        public TemaBL()
        {
            temaDAO = new Datos.DAO.TemaDAO_SqlServer();
        }
        public void Insertar(Modelo.Tema tema)
        {
            try
            {
                temaDAO.Insertar(tema);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Modificar(Modelo.Tema tema)
        {
            try
            {
                temaDAO.Modificar(tema);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Eliminiar(int id)
        {
            try
            {
                temaDAO.Eliminar(id);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Consultar(int id)
        {
            try
            {
                temaDAO.Consultar(id);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }

    }
}