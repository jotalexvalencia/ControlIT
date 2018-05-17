using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlIT.Models
{
    public class Vehiculo
    {   [Key]
        public int id { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
        public float longitud { get; set; }
        [Required]
        public float latitud { get; set; }
        [Required]
        public int propietario { get; set; }
        [Required]
        public string sitio { get; set; }
    }
}