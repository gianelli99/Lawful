using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.ClassParameters
{
    public class CambiarContrasenaParameters
    {
        public int UserID { get; set; }
        public bool NeedCurrentPassword { get; set; }
        public CambiarContrasenaParameters(int id, bool needPassword)
        {
            UserID = id;
            NeedCurrentPassword = needPassword;
        }
    }
}
