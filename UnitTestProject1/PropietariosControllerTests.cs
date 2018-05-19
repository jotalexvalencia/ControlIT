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
            var controller = new PropietariosController();
            controller.Repositorio = repoFake;

            //Act (actuar)
            var result = controller.Get() as OkNegotiatedContentResult<IEnumerable<Propietario>>;

            //Assert (asertar)
            Assert.IsNotNull(result); // si es nulo es porque no dio Ok
            Assert.AreEqual(3, result.Content.Count());  //aserto que la lista tenga 3 items
            Assert.AreEqual(1, result.Content.First().Id); // chequeo que el primer item
            Assert.AreEqual("Prop1", result.Content.First().Nombre);
            Assert.AreEqual("Ape1", result.Content.First().Apellido);
            //si se desea comprobar un usuario dentro del listado en este caso fue el primero pero si quiero saber si el segundo o x propietario sus datos son
            // nombre=x,apellido=x,y el id se puede 
        }
    }
}
