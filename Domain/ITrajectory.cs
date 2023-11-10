using AndelaStanic.DiplomskiRad.ShipTrajectory.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.Domain
{
    public interface ITrajectory
    {
        public Task<List<TrajectoryModel>> SearchRadius(float[] latlng);
        public Task<List<TrajectoryModel>> GetList();

        public Task<List<TrajectoryRawModel>> GetListRaw();
    }
}
