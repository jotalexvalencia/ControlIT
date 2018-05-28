using ControlIT.Controllers;
using ControlIT.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace UnitTestProject1
{
    [TestClass]
    public class PropietariosControllerTests
    {
        [TestMethod]
        public void Puedo_Obtener_Propietarios()
        {
            //Arrange (preparar)
            var repoFake = new RepositorioFake();
            var controller = new PropietariosController(repoFake);

            //Act (actuar)
            var result = controller.Get() as OkNegotiatedContentResult<IEnumerable<Propietario>>;

            //Assert (asertar)
            Assert.IsNotNull(result); // si es nulo es porque no dio Ok
            Assert.AreEqual(true, repoFake.ObtenerTodosLLamado);
            Assert.AreEqual(3, result.Content.Count());  //aserto que la lista tenga 3 items
            Assert.AreEqual(1, result.Content.First().Id); // chequeo que el primer item
            Assert.AreEqual("Prop1", result.Content.First().Nombre);
            Assert.AreEqual("Ape1", result.Content.First().Apellido);
            //si se desea comprobar un usuario dentro del listado en este caso fue el primero pero si quiero saber si el segundo o x propietario sus datos son
            // nombre=x,apellido=x,y el id se puede 
        }

        [TestMethod]
        public void Puedo_Obtener_Propietario_Por_Id()
        {
            var repoFake = new RepositorioFake();
            var controller = new PropietariosController(repoFake);
            const int idExistente = 1;
            var result = controller.Get(idExistente) as OkNegotiatedContentResult<Propietario>;

            Assert.IsNotNull(result);
            Assert.AreEqual(true, repoFake.ObtenerPorIdLlamado);
            Assert.AreEqual(1, result.Content.Id); // chequeo que el primer item
            Assert.AreEqual("Prop1", result.Content.Nombre);
            Assert.AreEqual("Ape1", result.Content.Apellido);
        }

        [TestMethod]
        public void Obtener_Propietario_Por_Id_Cuando_No_Existe_DebeRetornarNotFound()
        {
            var repoFake = new RepositorioFake();
            var controller = new PropietariosController(repoFake);
            const int idInexistente = 1050;
            var result = controller.Get(idInexistente);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            Assert.AreEqual(true, repoFake.ObtenerPorIdLlamado);
        }

        [TestMethod]
        public void Puedo_Crear_Propietario()
        {
            //Arrange (preparar)
            var repoFake = new RepositorioFake();
            var controller = new PropietariosController(repoFake);
            var prop = new Propietario { Nombre = "PropNuevo", Apellido = "ApeNuevo" };

            //Act (actuar)
            var result = controller.Post(prop) as CreatedAtRouteNegotiatedContentResult<Propietario>;

            //Assert (asertar)
            Assert.IsNotNull(result); // si es nulo es porque no dio Created
            Assert.AreEqual(true, repoFake.CrearLlamado);
            Assert.AreEqual(10, result.Content.Id); 
            Assert.AreEqual("PropNuevo", result.Content.Nombre);
            Assert.AreEqual("ApeNuevo", result.Content.Apellido);
        }

        [TestMethod]
        public void Crear_Propietario_Cuando_Es_Invalido_Retorna_BadRequest()
        {
            //Arrange (preparar)
            var repoFake = new RepositorioFake();
            var controller = new PropietariosController(repoFake);
            var prop = new Propietario();
            controller.ModelState.AddModelError("Nombre", "El nombre es requerido");
            
            //Act (actuar)
            var result = controller.Post(prop) as InvalidModelStateResult;
  
            //Assert (asertar)
            Assert.IsNotNull(result);
            //Como el modelo es invalido no se llamo al metodo crear del repo.
            Assert.AreEqual(false, repoFake.CrearLlamado);
        }

        [TestMethod]
        public void Puedo_Eliminar_Propietario()
        {
            //Arrange
            const int idExistenteSinVehiculos = 1;
            var repo = new RepositorioFake();
            var controller = new PropietariosController(repo);

            //Act
            var result = controller.Delete(idExistenteSinVehiculos);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.AreEqual(idExistenteSinVehiculos, repo.PropietarioId);
            Assert.AreEqual(true, repo.ObtenerPorIdLlamado);
            //Como esta todo ok si llamamos a Eliminar del repo.
            Assert.AreEqual(true, repo.EliminarLlamado);
        }

        [TestMethod]
        public void Eliminar_Propietario_Cuando_No_Existe_Devuelve_NotFound()
        {
            //Arrange
            const int idNoExistente = 10;
            var repo = new RepositorioFake();
            var controller = new PropietariosController(repo);

            //Act
            var result = controller.Delete(idNoExistente);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            Assert.AreEqual(idNoExistente, repo.PropietarioId);
            Assert.AreEqual(true, repo.ObtenerPorIdLlamado);
            //Como el propietario no existe no llamamos a Eliminar del repo.
            Assert.AreEqual(false, repo.EliminarLlamado);
        }

        [TestMethod]
        public void Eliminar_Propietario_Cuando_Tiene_Vehiculos_Devuelve_Conflict()
        {
            //Arrange
            const int idConVehiculos = 3;
            var repo = new RepositorioFake();
            var controller = new PropietariosController(repo);

            //Act
            var result = controller.Delete(idConVehiculos);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ConflictResult));
            Assert.AreEqual(idConVehiculos, repo.PropietarioId);
            Assert.AreEqual(true, repo.ObtenerPorIdLlamado);
            //Como tiene vehiculos no llamamos a Eliminar del repo.
            Assert.AreEqual(false, repo.EliminarLlamado);
        }

        [TestMethod]
        public void Eliminar_Propietario_Cuando_El_Id_No_Es_Valido_Devuelve_BadRequest()
        {
            //Arrange
            int[] idsNoValidos = { -1, 0 };
            var repo = new RepositorioFake();
            var controller = new PropietariosController(repo);

            //Act
            foreach(var id in idsNoValidos)
            {
                var result = controller.Delete(id) as BadRequestErrorMessageResult;

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("El id no es válido.", result.Message);
                Assert.AreEqual(false, repo.EliminarLlamado);
            }
            
        }

        [TestMethod]
        public void Puedo_Actualizar_Propietario()
        {
            //Arrange
            var prop = new Propietario { Id = 1, Nombre = "Modificado", Apellido = "Modificado" };
            var repo = new RepositorioFake();
            var controller = new PropietariosController(repo);

            //Act
            var result = controller.Put(prop.Id, prop);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            //Como esta todo Ok se llamo a los metodos del repo.
            Assert.AreEqual(true, repo.ObtenerPorIdLlamado);
            Assert.AreEqual(true, repo.ActualizarLlamado);
            Assert.IsNotNull(repo.Propietario);
            Assert.AreEqual(1, repo.Propietario.Id);
            Assert.AreEqual("Modificado", repo.Propietario.Nombre);
            Assert.AreEqual("Modificado", repo.Propietario.Apellido);
        }

        [TestMethod]
        public void Actualizar_Propietario_Cuando_El_Modelo_No_Es_Valido_Devuelve_BadRequest()
        {
            //Arrange
            var prop = new Propietario { Id = 1, Nombre = "", Apellido = "Ape1" };
            var repo = new RepositorioFake();
            var controller = new PropietariosController(repo);
            controller.ModelState.AddModelError("Nombre", "El nombre es requerido");

            //Act
            var result = controller.Put(prop.Id, prop) as InvalidModelStateResult;

            //Assert
            Assert.IsNotNull(result);
            //Como el modelo no es valido no se llamo a ninguno de los dos metodos del repositorio
            Assert.AreEqual(false, repo.ObtenerPorIdLlamado);
            Assert.AreEqual(false, repo.ActualizarLlamado); 
        }

        [TestMethod]
        public void Actualizar_Propietario_Cuando_El_Id_No_Existe_Devuelve_NotFound()
        {
            //Arrange
            const int idNoExistente = 10;
            var repo = new RepositorioFake();
            var controller = new PropietariosController(repo);

            //Act
            var result = controller.Put(idNoExistente, new Propietario());

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            //Como el id resulto ser de un propietario que no existe 
            //no se llamo a al metodo Actualizar pero si al obtenerPorid.
            Assert.AreEqual(true, repo.ObtenerPorIdLlamado);
            Assert.AreEqual(false, repo.ActualizarLlamado);
        }

        [TestMethod]
        public void Actualizar_Propietario_Cuando_El_Id_No_Es_Valido_Devuelve_BadRequest()
        {
            //Arrange
            int[] ids = { -1, 0 };
            var repo = new RepositorioFake();
            var controller = new PropietariosController(repo);

            //Act
            foreach(var id in ids)
            {
                var result = controller.Put(id, new Propietario()) as BadRequestErrorMessageResult;

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("El id no es válido.", result.Message);
                //Como el id no es valido no llamamos a ninguno delos dos metodos del repo
                Assert.AreEqual(false, repo.ObtenerPorIdLlamado);
                Assert.AreEqual(false, repo.ActualizarLlamado);
            }

        }
    }
}
