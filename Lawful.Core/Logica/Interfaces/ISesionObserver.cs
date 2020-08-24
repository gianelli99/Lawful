using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Logica.Interfaces
{
    public interface ISesionObserver
    {
        void Actualizar(bool isFirst);
    }
}
