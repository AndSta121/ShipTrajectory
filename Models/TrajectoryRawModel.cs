using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.Models
{
    public class TrajectoryRawModel
    {
        public int ShipID { get; set; }
        public string ShipName { get; set; }
        public GeometryRaw Geometry { get; set; }
        public DateTime CurrentTime { get; set; }
        public string ShipType { get; set; }
    }

    public class GeometryRaw
    {
        public string Type { get; set; }
        public float[] Coordinates { get; set; }
    }
}
