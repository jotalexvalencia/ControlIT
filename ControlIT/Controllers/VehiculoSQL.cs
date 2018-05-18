using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ControlIT.Models;

namespace ControlIT.Controllers
{
   
    public class VehiculoSQL : IVehicle
    { 
        
        public string constr { get; set; }

        public List<CantidadVehiculos> GetCantVehiculos()
        {
            throw new NotImplementedException();
        }

        public void setConexion(string constr)
        {   constr = ConfigurationManager.ConnectionStrings["ad"].ToString();
            this.constr = constr;
            
            
        }
    }
}