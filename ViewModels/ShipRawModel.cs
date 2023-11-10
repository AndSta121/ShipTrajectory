using System;
using System.Collections.Generic;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.ViewModels
{
    public class ShipRawModel
    {
        public int ShipID { get; set; }
        public string ShipName { get; set; }        
        public string ShipType { get; set; }
        public List<Position> Positions { get; set; }
    }

    public class Position
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string CurrentTime { get; set; }
    }

}
