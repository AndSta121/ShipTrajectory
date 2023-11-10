using System;
using System.Collections.Generic;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.ViewModels
{
    public class ShipModel
    {
        public int ShipID { get; set; }
        public string ShipName { get; set; }        
        public string ShipType { get; set; }
        public string Geometry { get; set; }
    }
}
