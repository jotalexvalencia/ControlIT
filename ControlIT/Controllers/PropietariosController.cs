using ControlIT.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ControlIT.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PropietariosController : ApiController
    {
        private SqlConnection conn;
        private SqlCommand com;

        private Repositorio repo;

        public PropietariosController(Repositorio repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        private void connection()        {
            
            string constr = ConfigurationManager.ConnectionStrings["ad"].ToString();
            conn = new SqlConnection(constr);
        }

        //GET: api/Propietarios
        public IHttpActionResult Get()
        {
            var propietarios = repo.ObtenerPropietarios();
            return Ok(propietarios);
        }

        //GET: api/Propietarios/5
        public IHttpActionResult Get(int id)
        {
            var prop = repo.ObtenerPropietario(id);
            if (prop == null)
            {
                return NotFound();
            }
            return Ok(prop);
        }

        //POST: /api/Propietarios
        public IHttpActionResult Post(Propietario prop)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repo.CrearPropietario(prop);

            return CreatedAtRoute("DefaultApi", new { id = prop.Id }, prop); 
        }

        //PUT: /api/Propietarios/id
        public IHttpActionResult Put(int id, Propietario prop)
        {
            if (id <= 0)
                return BadRequest("El id no es válido.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbProp = repo.ObtenerPropietario(id);
            if (dbProp == null)
                return NotFound();
            
            repo.ActualizarPropietario(prop);

            return Ok();
        }

        //DELETE: /api/Propietarios/id
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("El id no es válido.");

            var prop = repo.ObtenerPropietario(id);

            if (prop == null)
                return NotFound();

            if (prop.Vehiculos.Any())
                return Conflict();

            repo.EliminarPropietario(id);

            return Ok();
        }
    }
}
