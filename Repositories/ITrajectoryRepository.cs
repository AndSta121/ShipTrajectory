using AndelaStanic.DiplomskiRad.ShipTrajectory.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.Repositories
{
    public interface ITrajectoryRepository
    {
        public Task<List<TrajectoryModel>> SearchRadius(float[] latlng);
        public Task<List<TrajectoryModel>> GetList();

        public Task<List<TrajectoryRawModel>> GetListRaw();
    }
}
