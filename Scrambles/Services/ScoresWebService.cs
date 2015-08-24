﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Sql;
using Scrambles.Models;

namespace Scrambles.Services
{
    public class ScoresWebService
    {
        private readonly PiiaDb _db;

        public ScoresWebService(PiiaDb db)
        {
            _db = db;
        }

        public IEnumerable<ScoresListRow> GetList()
        {
            const string query = @"
SELECT map.ID, 
    map.RoundID roundNumber, 
    map.score, 
    map.camera,
	dbo.f_JumperName(map.UpJumper1ID) jumper1, 
	dbo.f_JumperName(map.UpJumper2ID) jumper2,
	dbo.f_JumperName(map.DownJumper1ID) jumper3,
	dbo.f_JumperName(map.DownJumper2ID) jumper4
FROM dbo.RoundJumperMap map
";
            return _db.SqlQuery<ScoresListRow>(query).ToList();
        }
    }
}