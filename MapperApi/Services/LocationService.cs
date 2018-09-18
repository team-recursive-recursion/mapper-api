/***
 * Filename: LocationService.cs
 * Author : nielpretorius
 * Class   : LocationService
 *
 *      Process user location.
 ***/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

namespace Mapper_Api.Services
{
    public class LocationService {
        private readonly CourseDb _db;

        public LocationService(CourseDb courseDb)
        {
            _db = courseDb;
        }

        public async Task<IEnumerable<Course>> sortCourseByPosition(Double? lat, Double? lon, int limit){
            string query = @"SELECT cs.* FROM public.""Courses"" cs LEFT JOIN 
            (SELECT *, ST_DistanceSphere(ST_geomFromWkb((el.""PolygonRaw"")), 
             ST_geomFromGeoJson('{{""type"":""Point"",""coordinates"":[ Param1 , Param2 ]}}'))
             FROM public.""Elements"" AS el) AS e ON e.""CourseId"" = cs.""CourseId""   
              GROUP BY cs.  ""CourseId"" ORDER BY MIN(e.""st_distancesphere"") LIMIT Param3";

            query = query.Replace("Param1", lat.ToString());
            query = query.Replace("Param2", lon.ToString());
            query = query.Replace("Param3", limit.ToString());
            
            List<Course> list = await _db.Courses.FromSql(query).ToListAsync();
            return list;
        }
        
    }
}