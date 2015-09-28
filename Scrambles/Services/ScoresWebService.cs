using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Sql;
using Data.Sql.Models;
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

        public RoundJumperMap GetRow(int roundJumperMapID)
        {
            return _db.RoundJumperMaps.Single(x => x.ID == roundJumperMapID);
        }

        public void Save(RoundJumperMap roundData)
        {
            var row = _db.RoundJumperMaps.Single(x => x.ID == roundData.ID);
            row.Camera = roundData.Camera;
            row.Score = roundData.Score;
            _db.SaveChanges();
        }

        internal IEnumerable<ResultsListRow> GetResultsList(UpDownFlag upDown)
        {
            const string sql = @"
SELECT j.FirstName + ' ' + j.LastName Name, 
	ISNULL((SELECT SUM(Score)
		FROM dbo.RoundJumperMap m 
		WHERE UpJumper1ID = j.JumperID
			OR m.UpJumper2ID = j.JumperID
			OR m.DownJumper1ID = j.JumperID
			OR m.DownJumper2ID = j.JumperID), 0) TotalScore
FROM dbo.Jumper j
WHERE j.RandomizedUpDown = @p0";
            return _db.SqlQuery<ResultsListRow>(sql, (int) upDown);
        }

        public ResultsVM GetResultsViewModel()
        {
            var up = GetResultsList(UpDownFlag.UpJumper);
            var down = GetResultsList(UpDownFlag.DownJumper);
            return new ResultsVM(up, down);
        }
    }
}