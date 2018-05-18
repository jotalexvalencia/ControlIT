﻿using ControlIT.Models;
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
    public class VehiculoController : ApiController
    {
        private SqlConnection conn;
        private SqlCommand com;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["ad"].ToString();
            conn = new SqlConnection(constr);
        }

        //POST: /api/Vehiculo
        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostVehiculo([FromBody]Vehiculo item)
        {
            HttpResponseMessage respuesta = null;

            if (item.descripcion != string.Empty && item.longitud != 0 && item.latitud != 0
                && item.propietario > 0 && item.sitio != string.Empty)
            {
                int cantVehiculos;

                connection();
                conn.Open();

                string sqlText = $"SELECT COUNT (vehiculos.propietario) as cantidad  FROM vehiculos " +
                    $"inner join propietario on vehiculos.propietario = propietario.id " +
                    $"where vehiculos.propietario = {item.propietario}";

                com = new SqlCommand(sqlText, conn);

                using (var registro = com.ExecuteReader())
                {
                    if (registro.Read())
                    {
                        cantVehiculos = int.Parse(registro["cantidad"].ToString());

                        if (cantVehiculos < 3)
                        {
                            registro.Close();
                            string sqlText1 = "Insert into vehiculos values(@descripcion,@longitud,@latitud,@propietario,@sitio)";
                            com = new SqlCommand(sqlText1, conn);

                            com.Parameters.AddWithValue("@descripcion", item.descripcion);
                            com.Parameters.AddWithValue("@longitud", item.longitud);
                            com.Parameters.AddWithValue("@latitud", item.latitud);
                            com.Parameters.AddWithValue("@propietario", item.propietario);
                            com.Parameters.AddWithValue("@sitio", item.sitio);
                            int i = com.ExecuteNonQuery();
                            conn.Close();
                            var response = Request.CreateResponse<Vehiculo>(HttpStatusCode.Created, item);
                            string uri = Url.Link("DefaultApi", new { id = item.id });
                            response.Headers.Location = new Uri(uri);
                            respuesta = Request.CreateResponse("Vehiculo ingresado con éxito");
                            return respuesta;
                        }
                        else
                        {
                            conn.Close();
                            respuesta = Request.CreateResponse($"El propietario alcanzó {cantVehiculos}, que es la Maxima cantidad de vehiculos");
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
                respuesta = Request.CreateResponse("Debe ingresar la información, gracias");
                return respuesta;
            }
        }

        //GET: api/vehiculo
        [System.Web.Http.HttpGet()]
        public IHttpActionResult GetCantVehiculos()
        {
            IHttpActionResult ret = null;

            connection();
            conn.Open();

            List<CantidadVehiculos> cantidadVehiculos = new List<CantidadVehiculos>();

            com = new SqlCommand("SELECT propietario.nombre as nombre,propietario.apellido as apellido," +
               "vehiculos.descripcion as descripcion, vehiculos.latitud as latitud,vehiculos.longitud as longitud," +
               " vehiculos.sitio as sitio, vehiculos.id as id,propietario.id as pid FROM vehiculos " +
               "INNER JOIN propietario ON vehiculos.propietario = propietario.id order by propietario.id", conn);
            using (var dr = com.ExecuteReader())
            {

                while (dr.Read())
                {
                    // Usuario
                    var cantidadVehiculo = new CantidadVehiculos
                    {
                        id = (int)Convert.ToInt32(dr["id"]),
                        nombre = dr["nombre"].ToString(),
                        apellido = dr["apellido"].ToString(),
                        descripcion = dr["descripcion"].ToString(),
                        latitud = (float)Convert.ToDouble(dr["latitud"]),
                        longitud = (float)Convert.ToDouble(dr["longitud"]),
                        pid = (int)Convert.ToInt32(dr["pid"]),
                        sitio = dr["sitio"].ToString()
                    };


                    // Agregamos el usuario a la lista genreica
                    cantidadVehiculos.Add(cantidadVehiculo);


                }

                if (cantidadVehiculos.Count > 0)
                {
                    conn.Close();
                    ret = Ok(cantidadVehiculos);
                }


                return ret;
            }
        }

        //PUT: /api/vehiculo/id
        [System.Web.Http.HttpPut]
        public HttpResponseMessage PutVehiculo(int id, [FromBody]Vehiculo item)
        {
            HttpResponseMessage respuesta = null;

            if (!String.IsNullOrEmpty(item.descripcion) && !String.IsNullOrEmpty(item.latitud.ToString()) &&
                !String.IsNullOrEmpty(item.longitud.ToString()) && !String.IsNullOrEmpty(item.propietario.ToString()) && id > 0)
            {
                int cantVehiculos;

                connection();

                conn.Open();
                string sqlText = $"SELECT COUNT (vehiculos.propietario) as cantidad  FROM vehiculos " +
                    $"inner join propietario on vehiculos.propietario = propietario.id " +
                    $"where vehiculos.propietario = {item.propietario}";

                com = new SqlCommand(sqlText, conn);

                using (var registro = com.ExecuteReader())
                {
                    if (registro.Read())
                    {
                        cantVehiculos = int.Parse(registro["cantidad"].ToString());

                        if (cantVehiculos < 4)
                        {
                            registro.Close();
                            string sqlText1 = $"update vehiculos set descripcion=@descripcion," +
                                $"latitud=@latitud," +
                                $"longitud=@longitud," +
                                $"sitio=@sitio," +
                                $"propietario=@propietario where id={id}";
                            com = new SqlCommand(sqlText1, conn);

                            com.Parameters.AddWithValue("@descripcion", item.descripcion);
                            com.Parameters.AddWithValue("@latitud", item.latitud);
                            com.Parameters.AddWithValue("@longitud", item.longitud);
                            com.Parameters.AddWithValue("@propietario", item.propietario);
                            com.Parameters.AddWithValue("@sitio", item.sitio);

                            int i = com.ExecuteNonQuery();
                            conn.Close();
                            var response = Request.CreateResponse<Vehiculo>(HttpStatusCode.Created, item);
                            string uri = Url.Link("DefaultApi", new { id = item.id });
                            response.Headers.Location = new Uri(uri);
                            respuesta = Request.CreateResponse("Vehiculo Actualizado con éxito");
                            return respuesta;
                        }
                        else
                        {
                            conn.Close();
                            respuesta = Request.CreateResponse($"El propietario alcanzó {cantVehiculos}, que es la Maxima cantidad de vehiculos");
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
                respuesta = Request.CreateResponse("Debe ingresar la información, gracias");
                return respuesta;
            }
        }

        //Get: /api/vehiculo/id
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetVehiculoPropietario(int id, [FromBody]Vehiculo item)
        {
            IHttpActionResult ret = null;

            if (id > 0)
            {

                connection();
                conn.Open();

                var vehiculos = new List<Vehiculo>();
                com = new SqlCommand($"select vehiculos.latitud, vehiculos.longitud, vehiculos.sitio " +
                   $"from vehiculos where propietario ={id} ", conn);

                using (var registro = com.ExecuteReader())
                {
                    while (registro.Read())
                    {
                        // Usuario
                        var vehiculo = new Vehiculo
                        {
                            latitud = (float)Convert.ToDouble(registro["latitud"]),
                            longitud = (float)Convert.ToDouble(registro["longitud"]),
                            sitio = registro["sitio"].ToString()
                        };

                        // Agregamos el usuario a la lista 
                        vehiculos.Add(vehiculo);
                    }

                    if (vehiculos.Count > 0)
                    {
                        conn.Close();
                        ret = Ok(vehiculos);


                    }
                    else
                    {
                        conn.Close();
                        ret = BadRequest("El usuario no posee vehículos aún");
                    }


                    return ret;

                }
            }
            else
            {
                conn.Close();
                ret = BadRequest("El usuario no existe");
                return ret;
            }
        }


    }
}