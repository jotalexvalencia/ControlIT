using ControlIT.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ControlIT.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PropietarioController : ApiController
    {
        private SqlConnection conn;
        private SqlCommand com;

        private void connection()        {
            
            string constr = ConfigurationManager.ConnectionStrings["ad"].ToString();
            conn = new SqlConnection(constr);
        }

        //GET: api/Propietario
        [System.Web.Http.HttpGet()]
        public IHttpActionResult GetPropietario()
        {
            IHttpActionResult ret = null;

            connection();
            conn.Open();

            var propietarios = new List<Propietario>();
            com = new SqlCommand("SELECT * FROM propietario", conn);
            using (var registro = com.ExecuteReader())
            {

                while (registro.Read())
                {
                    // Usuario
                    var propietario = new Propietario
                    {
                        id = Convert.ToInt32(registro["id"]),
                        nombre = registro["nombre"].ToString(),
                        apellido = registro["apellido"].ToString()
                    };

                    // Agregamos el usuario a la lista 
                    propietarios.Add(propietario);
                }

                if (propietarios.Count > 0)
                {
                    conn.Close();
                    ret = Ok(propietarios);
                }
                else
                {
                    conn.Close();
                    ret = NotFound();
                }

                return ret;
            }
        }

        //POST: /api/Propietario
        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostPropietario([FromBody]Propietario item)
        {
            if (!String.IsNullOrEmpty(item.nombre) && !String.IsNullOrEmpty(item.apellido))
            {
                connection();
                conn.Open();
                string sqlText = "Insert into propietario values(@nombre,@apellido)";
                com = new SqlCommand(sqlText, conn);

                com.Parameters.AddWithValue("@nombre", item.nombre.ToUpper().Trim());
                com.Parameters.AddWithValue("@apellido", item.apellido.ToUpper().Trim());

                int i = com.ExecuteNonQuery();
                conn.Close();
                var response = Request.CreateResponse<Propietario>(HttpStatusCode.Created, item);
                string uri = Url.Link("DefaultApi", new { item });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                conn.Close();
                var respuesta = Request.CreateResponse("Error");
                return respuesta;
            }
        }

        //PUT: /api/Propietario/id
        [System.Web.Http.HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Propietario item)
        {
            connection();
            conn.Open();
            string sqlText = $"update propietario set nombre=@nombre,apellido=@apellido where id={id}";
            com = new SqlCommand(sqlText, conn);
            com.Parameters.AddWithValue("@nombre", item.nombre.ToUpper().Trim());
            com.Parameters.AddWithValue("@apellido", item.apellido.ToUpper().Trim());
            int i = com.ExecuteNonQuery();
            conn.Close();
            var respuesta = Request.CreateResponse("Propietario actualizado");
            return respuesta;
        }

        //DELETE: /api/Propietario/id
        [System.Web.Http.HttpDelete]
        public HttpResponseMessage Delete(int id, [FromBody]Propietario item)
        {
            HttpResponseMessage respuesta = null;

            if (id > 0)
            {
                int cantVehiculos;

                connection();
                conn.Open();

                string sqlText = $"SELECT COUNT (vehiculos.propietario) as cantidad  FROM vehiculos " +
                    $"inner join propietario on vehiculos.propietario = propietario.id " +
                    $"where vehiculos.propietario = {id}";

                com = new SqlCommand(sqlText, conn);

                using (var registro = com.ExecuteReader())
                {
                    if (registro.Read())
                    {
                        cantVehiculos = int.Parse(registro["cantidad"].ToString());

                        if (cantVehiculos < 1)
                        {
                            registro.Close();
                            string sqlText1 = $"delete from propietario where id={id}";
                            com = new SqlCommand(sqlText1, conn);
                            int i = com.ExecuteNonQuery();
                            conn.Close();
                            respuesta = Request.CreateResponse("Propietario eliminado");
                            return respuesta;
                        }
                        else
                        {
                            conn.Close();
                            respuesta = Request.CreateResponse($"El propietario posee {cantVehiculos} vehículo(s)");
                            return respuesta;
                        }
                    }
                    else
                    {
                        conn.Close();
                        respuesta = Request.CreateResponse("Error");
                        return respuesta;
                    }
                }
            }
            else
            {
                conn.Close();
                respuesta = Request.CreateResponse("Propietario no existe");
                return respuesta;
            }
        }
    }
}
