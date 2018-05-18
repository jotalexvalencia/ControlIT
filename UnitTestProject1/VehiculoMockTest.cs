using ControlIT.Controllers;
using ControlIT.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class VehiculoMockTest 
    {
        [TestMethod]
        public void GetTodos()
        {
            // Arrange   
            
            VehiculoMock controller = new VehiculoMock();
        

            // Act
            var result = controller.GetCantVehiculos();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}
