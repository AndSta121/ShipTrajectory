using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AndelaStanic.DiplomskiRad.ShipTrajectory.Domain;
using AndelaStanic.DiplomskiRad.ShipTrajectory.Models;
using AndelaStanic.DiplomskiRad.ShipTrajectory.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.Controllers
{
    public class ShipController : Controller
    {
        private ITrajectory _trajectory;
        public ShipController (ITrajectory trajectory)
        {
            _trajectory = trajectory;
        }
        [HttpGet]
        [Route("[controller]/trajectories")]
        public async Task<IActionResult> Index()
        {
            var trajectories = await _trajectory.GetList();

            var ships = ToShipModel(trajectories);

            return Json(new { Data = ships });
        }

        [HttpGet]
        [Route("[controller]/searchradius")]
        public async Task<IActionResult> SearchRadius([FromQuery]float lat, float lng)
        {
            var trajectories = await _trajectory.SearchRadius(new[] { lat, lng});

            var ships = ToShipModel(trajectories);

            return Json(new { Data = ships });
        }
        private List<ShipModel> ToShipModel(List<TrajectoryModel> trajectories)
        {
            if (trajectories == null || trajectories.Count == 0)
            {
                return new List<ShipModel>();
            }

            var result = new List<ShipModel>();

            foreach (var trajectory in trajectories)
            {
                var ship = new ShipModel
                {
                    ShipID = trajectory.ShipID,
                    ShipName = trajectory.ShipName,
                    ShipType = trajectory.ShipType,
                    Geometry = trajectory.Geometry
                };

                result.Add(ship);
            }

            return result;
        }
    }
}