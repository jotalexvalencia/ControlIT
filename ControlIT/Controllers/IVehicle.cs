﻿using ControlIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ControlIT.Controllers
{
    interface IVehicle
    {
        List<CantidadVehiculos> GetCantVehiculos();
    }
}