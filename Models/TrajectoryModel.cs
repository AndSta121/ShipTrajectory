using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.Models
{
    public class TrajectoryModel
    {
        public int ShipID { get; set; }
        public string ShipName { get; set; }
        public string Geometry { get; set; }
        public string ShipType { get; set; }
    }
    
}
