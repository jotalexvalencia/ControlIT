using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlIT.Models
{
    public class Vehiculo
    {   [Key]
        public int Id { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public double Longitud { get; set; }
        [Required]
        public double Latitud { get; set; }
        [Required]
        public int Propietario { get; set; }
        [Required]
        public string Sitio { get; set; }
    }
}