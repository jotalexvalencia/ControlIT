//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Policy;
//using System.Web;
//using System.Web.Http;
//using ControlIT.Models;
//using System.Net;
//using System.Web.Http.Cors;

//namespace ControlIT.Controllers
//{
//    [EnableCors(origins: "*", headers: "*", methods: "*")]
//    public class VehiculoSQL : IVehicle
//    {
//        private string constr;
//        private SqlConnection conn;
//        private SqlCommand com;


//        public void setConexion(string constr)
//        {
//            this.constr = constr;
//        }

//        public IHttpActionResult GetCantVehiculos()
//        {
//            throw new NotImplementedException();
//        }

//        public IHttpActionResult GetVehiculoPropietario(int id, [FromBody] Vehiculo item)
//        {
//            throw new NotImplementedException();
//        }

//        public string PostVehiculo([FromBody] Vehiculo item)
//        {
//            string respuesta = null;

//            if (item.descripcion != string.Empty && item.longitud != 0 && item.latitud != 0
//                && item.propietario > 0 && item.sitio != string.Empty)
//            {
//                int cantVehiculos;
                
//                conn.Open();

//                string sqlText = $"SELECT COUNT (vehiculos.propietario) as cantidad  FROM vehiculos " +
//                    $"inner join propietario on vehiculos.propietario = propietario.id " +
//                    $"where vehiculos.propietario = {item.propietario}";

//                com = new SqlCommand(sqlText, conn);

//                using (var registro = com.ExecuteReader())
//                {
//                    if (registro.Read())
//                    {
//                        cantVehiculos = int.Parse(registro["cantidad"].ToString());

//                        if (cantVehiculos < 3)
//                        {
//                            registro.Close();
//                            string sqlText1 = "Insert into vehiculos values(@descripcion,@longitud,@latitud," +
//                                "@propietario,@sitio)";
//                            com = new SqlCommand(sqlText1, conn);

//                            com.Parameters.AddWithValue("@descripcion", item.descripcion);
//                            com.Parameters.AddWithValue("@longitud", item.longitud);
//                            com.Parameters.AddWithValue("@latitud", item.latitud);
//                            com.Parameters.AddWithValue("@propietario", item.propietario);
//                            com.Parameters.AddWithValue("@sitio", item.sitio);
//                            int i = com.ExecuteNonQuery();
//                            conn.Close();
//                            //return item;
//                            //string uri = Url.Link("DefaultApi", new { id = item.id });
//                            //response.Headers.Location = new Uri(uri);
//                            respuesta = Request.CreateResponse("Vehiculo ingresado con éxito");
//                            return respuesta;
//                        }
//                        else
//                        {
//                            conn.Close();
//                            respuesta = Request.CreateResponse($"El propietario alcanzó {cantVehiculos}," +
//                                $" " +
//                                $"que es la Maxima cantidad de vehiculos");
//                            return respuesta;
//                        }
//                    }
//                    else
//                    {
//                        conn.Close();
//                        respuesta = Request.CreateResponse("Error");
//                        return respuesta;
//                    }
//                }
//            }
//            else
//            {
//                conn.Close();
//                respuesta = Request.CreateResponse("Debe ingresar la información, gracias");
//                return respuesta;
//            }
//        }

//        public HttpResponseMessage PutVehiculo(int id, [FromBody] Vehiculo item)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}