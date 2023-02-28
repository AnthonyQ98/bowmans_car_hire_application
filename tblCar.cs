using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsDatabase
{
   public class tblCar
    {
        public string VehicleRegNo { get; set; }

        public string Make { get; set; }

        public string EngineSize { get; set; }

        public DateTime DateRegistered { get; set; }

        public decimal RentalPerDay { get; set; }

        public bool Available { get; set; }
    }
}
