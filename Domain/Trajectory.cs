using AndelaStanic.DiplomskiRad.ShipTrajectory.Models;
using AndelaStanic.DiplomskiRad.ShipTrajectory.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.Domain
{
    public class Trajectory: ITrajectory
    {
        ITrajectoryRepository _trajectory = null;
        public Trajectory(ITrajectoryRepository trajectory)
        {
            _trajectory = trajectory;
        }

        public async Task<List<TrajectoryModel>> GetList()
        {
            return await _trajectory.GetList();
        }
        public async Task<List<TrajectoryRawModel>> GetListRaw()
        {
            return await _trajectory.GetListRaw();
        }
        public async Task<List<TrajectoryModel>> SearchRadius(float[] latlng)
        {
            return await _trajectory.SearchRadius(latlng);
        }
    }
}
