using System;
using System.Data.SqlClient;
using ControlIT.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AgregaPropietario()
        {
            string constr = "Data Source = DESKTOP-1U193CO; Initial Catalog=ControlIT;Integrated Security=True";
            PropietarioSql sql = new PropietarioSql();
            SqlConnection conexion;
            //Aqui va la conexion de tu connection string del webconfig
            conexion = new SqlConnection(constr);
            sql.getConexion(constr);
           
            conexion.Open();
            string query = $"select MAX(id) from propietario";
            int maximo=0;
            SqlCommand comando;
           

            comando = new SqlCommand(query, conexion);
                SqlDataReader registros = comando.ExecuteReader();
                if (registros.Read())
                {
                    maximo= int.Parse(registros[0].ToString());
                   
                }
            int id = sql.InsProp("Pablo", "B");

            //tu assert
             Assert.AreEqual(maximo + 1, id);
        }
    }
}
