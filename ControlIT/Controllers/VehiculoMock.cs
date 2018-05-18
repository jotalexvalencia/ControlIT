using ControlIT.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ControlIT.Controllers
{
    public class VehiculoMock : IVehicle
    {
        public List<CantidadVehiculos> cantidadVehiculos;

        public VehiculoMock()
        {
            cantidadVehiculos = new List<CantidadVehiculos>();
            CantidadVehiculos cantidadVehiculo = new CantidadVehiculos();
            cantidadVehiculo.id = 3026;
            cantidadVehiculo.nombre = "JORGE ALEXANDER";
            cantidadVehiculo.apellido = "VALENCIA VALENCIA";
            cantidadVehiculo.descripcion = "kia picanto 2018 placa 78yut";
            cantidadVehiculo.latitud = 3.4516468F;
            cantidadVehiculo.longitud = -76.53198F;
            cantidadVehiculo.pid = 1046;
            cantidadVehiculo.sitio = "CALI";
            cantidadVehiculos.Add(cantidadVehiculo);
            CantidadVehiculos cantidadVehiculo2 = new CantidadVehiculos();
            cantidadVehiculo2.id = 3027;
            cantidadVehiculo2.nombre = "JORGE ALEXANDER";
            cantidadVehiculo2.apellido = "VALENCIA VALENCIA";
            cantidadVehiculo2.descripcion = "KIA RIO";
            cantidadVehiculo2.latitud = 4.80871725F;
            cantidadVehiculo2.longitud = -75.6906F;
            cantidadVehiculo2.pid = 1046;
            cantidadVehiculo2.sitio = "PEREIRA";
            cantidadVehiculos.Add(cantidadVehiculo2);

            

        }

        public List<CantidadVehiculos> GetCantVehiculos()
        {
            return cantidadVehiculos;
        }

        List<CantidadVehiculos> IVehicle.GetCantVehiculos()
        {
            throw new System.NotImplementedException();
        }

        public void setConexion(string constr)
        {
            
        }
    }
}


