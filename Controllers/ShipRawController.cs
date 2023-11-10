using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AndelaStanic.DiplomskiRad.ShipTrajectory.Domain;
using AndelaStanic.DiplomskiRad.ShipTrajectory.Models;
using AndelaStanic.DiplomskiRad.ShipTrajectory.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.Controllers
{
    public class ShipRawController : Controller
    {
        private ITrajectory _trajectory;
        public ShipRawController(ITrajectory trajectory)
        {
            _trajectory = trajectory;
        }
        [HttpGet]
        [Route("[controller]/trajectories")]
        public async Task<IActionResult> Index()
        {
            var trajectories = await _trajectory.GetListRaw();

            var ships = ToShipModel(trajectories);

            return Json(new { Data = ships });
        }

        private List<ShipRawModel> ToShipModel(List<TrajectoryRawModel> trajectories)
        {
            if (trajectories == null || trajectories.Count == 0)
            {
                return new List<ShipRawModel>();
            }

            var result = new List<ShipRawModel>();

            var shipIds = trajectories.Select(x => x.ShipID).Distinct();
            foreach (var shipId in shipIds)
            {
                var trajectory = trajectories.First(x => x.ShipID == shipId);

                var ship = new ShipRawModel
                {
                    ShipID = trajectory.ShipID,
                    ShipName = trajectory.ShipName,
                    ShipType = trajectory.ShipType,
                    Positions = trajectories.Where(x => x.ShipID == shipId)
                                            .Select(y => new Position 
                                            {
                                                CurrentTime = y.CurrentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                Latitude = y.Geometry?.Coordinates[0] ?? 0,
                                                Longitude = y.Geometry?.Coordinates[1] ?? 0
                                            })
                                            .OrderBy(z=>z.CurrentTime)
                                            .ToList()
                };

                result.Add(ship);
            }

            return result;
        }
    }
}