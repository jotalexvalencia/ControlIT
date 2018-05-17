using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlIT.Models
{
    public class CantidadVehiculos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string descripcion { get; set; }
        public float latitud { get; set; }
        public float longitud { get; set; }
        public int pid { get; set; }
        public string sitio { get; set; }
    }
}