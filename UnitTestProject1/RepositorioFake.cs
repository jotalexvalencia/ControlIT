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

        public int PropietarioId;

        public bool ObtenerPorIdLlamado, ObtenerTodosLLamado, CrearLlamado, EliminarLlamado, ActualizarLlamado;

        public Propietario Propietario;

        private List<Propietario> list = new List<Propietario>()
        {
            new Propietario { Id = 1, Nombre = "Prop1", Apellido = "Ape1"},
            new Propietario { Id = 2, Nombre = "Prop2", Apellido = "Ape2"},
            new Propietario { Id = 3, Nombre = "Prop3", Apellido = "Ape3" ,
                Vehiculos = new [] { new Vehiculo() }
            }
        };

        public override IEnumerable<Propietario> ObtenerPropietarios()
        {
            ObtenerTodosLLamado = true;
            return list;
        }

        public override Propietario ObtenerPropietario(int id)
        {
            ObtenerPorIdLlamado = true;
            PropietarioId = id;
            return ObtenerPropietarios().FirstOrDefault(x => x.Id == id);
        }

        public override void CrearPropietario(Propietario prop)
        {
            CrearLlamado = true;
            prop.Id = 10;
            Propietario = prop;
        }

        public override void ActualizarPropietario(Propietario prop)
        {
            ActualizarLlamado = true;
            Propietario = prop;
        }

        public override void EliminarPropietario(int id)
        {
            EliminarLlamado = true;
            PropietarioId = id;
        }
    }
}
