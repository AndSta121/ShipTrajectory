using AndelaStanic.DiplomskiRad.ShipTrajectory.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndelaStanic.DiplomskiRad.ShipTrajectory.Repositories
{
    public class TrajectoryRepository : ITrajectoryRepository
    {
        IConfiguration _configuration = null;
        public TrajectoryRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<TrajectoryModel>> GetList()
        {
            return MockupTrajs();
            var result = new List<TrajectoryModel>();
            try
            {
                var connString = _configuration.GetConnectionString("DefaultConnectionString");

                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                var sql= @"SELECT ST_AsGeoJSON(s.traj)::json As geometry, s.mmsi as ShipID, si.name as shipName, si.shiptype as shipType
                            FROM ships s
                            INNER JOIN shipsinfo si
                            ON s.mmsi = si.mmsi
                            ORDER BY s.mmsi
                            LIMIT 1";

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var model = new TrajectoryModel
                        {
                            ShipID = await reader.GetFieldValueAsync<int>(1),
                            ShipName = !reader.IsDBNull(2) ? await reader.GetFieldValueAsync<string>(2) : String.Empty,
                            ShipType = !reader.IsDBNull(3) ? await reader.GetFieldValueAsync<string>(3) : String.Empty,
                            Geometry = !reader.IsDBNull(0) ? await reader.GetFieldValueAsync<string>(0) : String.Empty
                        };

                        result.Add(model);
                    };
                }
            }
            catch (Exception e)
            {
                var x = e;
                throw;
            }

            return result;
        }

        public async Task<List<TrajectoryModel>> SearchRadius(float[] latlng)
        {
            return MockupTrajs();
            //var result = new List<TrajectoryModel>();
            //try
            //{
            //    var connString = _configuration.GetConnectionString("DefaultConnectionString");

            //    await using var conn = new NpgsqlConnection(connString);
            //    await conn.OpenAsync();

            //    var sql = @$"SELECT ST_AsGeoJSON(s.traj)::json As geometry, s.mmsi as ShipID, si.name as shipName, si.shiptype as shipType
            //                FROM ships s
            //                INNER JOIN shipsinfo si
            //                ON s.mmsi = si.mmsi
            //                WHERE ST_intersects(s.traj, ST_BUFFER(ST_SetSRID(ST_MakePoint({ latlng[0]}, { latlng[1]}),4326),  
            //                { _configuration.GetValue<int>("Settings:DefaultRadius")}))";
                
            //    await using (var cmd = new NpgsqlCommand(sql, conn))
            //    await using (var reader = await cmd.ExecuteReaderAsync())
            //    {
            //        while (await reader.ReadAsync())
            //        {
            //            var model = new TrajectoryModel
            //            {
            //                ShipID = await reader.GetFieldValueAsync<int>(1),
            //                ShipName = !reader.IsDBNull(2) ? await reader.GetFieldValueAsync<string>(2) : String.Empty,
            //                ShipType = !reader.IsDBNull(3) ? await reader.GetFieldValueAsync<string>(3) : String.Empty,
            //                Geometry = !reader.IsDBNull(0) ? await reader.GetFieldValueAsync<string>(0) : String.Empty
            //            };

            //            result.Add(model);
            //        };
            //    }
            //}
            //catch (Exception e)
            //{
            //    var x = e;
            //    throw;
            //}

            //return result;
        }
        public async Task<List<TrajectoryRawModel>> GetListRaw()
        {
            return MockupRawTrajs();
            var result = new List<TrajectoryRawModel>();
            try
            {
                var connString = _configuration.GetConnectionString("DefaultConnectionString");

                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                var sql = @"SELECT ST_AsGeoJSON(geom)::json As geometry, 
                                    mmsi as ShipID, 
                                    name as Name, 
                                    shiptype as ShipType, 
                                    t as timestamp
                            FROM aisinputfiltered WHERE mmsi=636018261";

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var model = new TrajectoryRawModel
                        {
                            ShipID = await reader.GetFieldValueAsync<int>(1),
                            ShipName = !reader.IsDBNull(2) ? await reader.GetFieldValueAsync<string>(2) : String.Empty,
                            ShipType = !reader.IsDBNull(3) ? await reader.GetFieldValueAsync<string>(3) : String.Empty,
                            CurrentTime = !reader.IsDBNull(4) ? await reader.GetFieldValueAsync<DateTime>(4) : DateTime.MinValue,
                            Geometry = !reader.IsDBNull(0)
                            ? JsonConvert.DeserializeObject<GeometryRaw>(await reader.GetFieldValueAsync<string>(0))
                            : null
                        };

                        result.Add(model);
                    };
                }
            }
            catch (Exception e)
            {
                var x = e;
                throw;
            }

            return result;
        }

        private List<TrajectoryModel> MockupTrajs()
        {
            return new List<TrajectoryModel>
            {
                MockupTraj(1111, "Mihaela 1",new float[,]
                {
                    { 55.68f, 12.6f },
                    { 55.38f, 12.22f },
                    { 55.58f, 12.12f },
                    { 56.18f, 12.02f },
                    { 56.38f, 12.22f },
                    { 56.58f, 12.12f }
                }),
                MockupTraj(1112, "Mihaela 2",new float[,]
                {
                    { 55.68f, 12.6f},
                    { 57.38f, 12.22f },
                    { 57.58f, 12.12f },
                    { 58.18f, 12.02f },
                    { 58.38f, 12.22f },
                    { 58.58f, 12.12f }
                })
            };
        }

        private List<TrajectoryRawModel> MockupRawTrajs()
        {
            return new List<TrajectoryRawModel>
            {
                MockupRawTraj(1111, "Mihaela One",new [] { 55.68f, 12.6f }),
                MockupRawTraj(1111, "Mihaela One",new [] { 55.38f, 12.22f }),
                MockupRawTraj(1111, "Mihaela One",new [] { 55.58f, 12.12f }),
                MockupRawTraj(1111, "Mihaela One",new [] { 56.18f, 12.02f }),
                MockupRawTraj(1111, "Mihaela One",new [] { 56.38f, 12.22f }),
                MockupRawTraj(1111, "Mihaela One",new [] { 56.58f, 12.12f }),
                MockupRawTraj(1112, "Mihaela 2",new [] { 55.68f, 12.6f }),
                MockupRawTraj(1112, "Mihaela 2",new [] { 57.38f, 12.22f }),
                MockupRawTraj(1112, "Mihaela 2",new [] { 57.58f, 12.12f }),
                MockupRawTraj(1112, "Mihaela 2",new [] { 58.18f, 12.02f }),
                MockupRawTraj(1112, "Mihaela 2",new [] { 58.38f, 12.22f }),
                MockupRawTraj(1112, "Mihaela 2",new [] { 58.58f, 12.12f })
            };
        }

        private TrajectoryModel MockupTraj(int id, string name, float[,] coordinates)
        {
            return new TrajectoryModel
            {
                ShipID = id,
                ShipName = name,
                ShipType = "Yacht",
                Geometry = JsonConvert.SerializeObject(new Geo
                {
                    type = "LineString",
                    coordinates = coordinates
                })
            };
        }

        private TrajectoryRawModel MockupRawTraj(int id, string name, float[] coordinates)
        {
            return new TrajectoryRawModel
            {
                CurrentTime = DateTime.Now,
                ShipID = id,
                ShipName = name,
                ShipType = "Yacht",
                Geometry = new GeometryRaw
                {
                    Type = "Point",
                    Coordinates = coordinates
                }
            };
        }
    }

    public class Geo
    {
        public string type { get; set; }
        public float[,] coordinates { get; set; }
    }
}
