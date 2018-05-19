using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControlIT.Controllers
{
    public class PropietarioSql
    {
        string cadcon,nombre,apellido,sql,sql1;
        int id;
        SqlConnection conexion;
        SqlCommand comando;


       public void getConexion(string cadcon)
        {
            this.cadcon = cadcon;
        }

        public int InsProp(string nombre, string apellido)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            conexion = new SqlConnection(this.cadcon);
            conexion.Open();
            sql = $"Insert into propietario values('{this.nombre}','{this.apellido}')";
            try
            {
                comando = new SqlCommand(sql, conexion);
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return -1;
            }
            
            sql1 = $"select MAX(id) from propietario";
            try
            {
                comando = new SqlCommand(sql1, conexion);
                SqlDataReader registros = comando.ExecuteReader();
                if (registros.Read())
                {
                    id = int.Parse(registros[0].ToString());
                    return this.id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            conexion.Close();
            return this.id;
            

        }
    }
}