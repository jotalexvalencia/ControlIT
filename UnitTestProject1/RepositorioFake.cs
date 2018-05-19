using ControlIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    internal class RepositorioFake : Repositorio
    {
        public RepositorioFake() : base("cadenaConexionFake") { }

        public override IEnumerable<Propietario> ObtenerPropietarios()
        {
            return new List<Propietario>()
            {
                new Propietario { Id = 1, Nombre = "Prop1", Apellido = "Ape1"},
                new Propietario { Id = 2, Nombre = "Prop2", Apellido = "Ape2"},
                new Propietario { Id = 3, Nombre = "Prop3", Apellido = "Ape3"}
            };
        }
    }
}
