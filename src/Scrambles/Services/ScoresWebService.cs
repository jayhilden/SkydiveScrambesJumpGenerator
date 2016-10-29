using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        public ScoresListVM GetScoreListVM()
        {
            const string query = @"
SELECT map.ID, 
    r.RoundNumber, 
    map.score, 
    map.camera,
	dbo.f_JumperName(map.UpJumper1ID) jumper1, 
	dbo.f_JumperName(map.UpJumper2ID) jumper2,
	dbo.f_JumperName(map.DownJumper1ID) jumper3,
	dbo.f_JumperName(map.DownJumper2ID) jumper4,
    map.VideoUrl
FROM dbo.RoundJumperMap map
	INNER JOIN dbo.Round R ON (R.RoundID = map.RoundID)
";
            var scores = _db.SqlQuery<ScoresListRow>(query).ToList();
            return new ScoresListVM
            {
                Scores = scores,
                IsAdmin = UserService.IsAdmin()
                
            };
        }

        public ScoresEditModel GetEditModel(int roundJumperMapID)
        {
            var dbRow = _db.RoundJumperMaps.Single(x => x.ID == roundJumperMapID);
            
            var editModel = new ScoresEditModel
            {
                RoundJumperMapID = dbRow.ID,
                Camera = dbRow.Camera,
                Score = dbRow.Score,
                DownJumper1 = dbRow.DownJumper1ID,
                DownJumper2 = dbRow.DownJumper2ID,
                UpJumper1 = dbRow.UpJumper1ID,
                UpJumper2 = dbRow.UpJumper2ID,
                VideoUrl = dbRow.VideoUrl
            };
            PopulateLists(editModel);
            return editModel;
        }

        public void PopulateLists(ScoresEditModel editModel)
        {
            var jumperList = BuildJumperList();
            editModel.DownJumper1List = CloneListSetSelected(jumperList, editModel.DownJumper1);
            editModel.DownJumper2List = CloneListSetSelected(jumperList, editModel.DownJumper2);
            editModel.UpJumper1List = CloneListSetSelected(jumperList, editModel.UpJumper1);
            editModel.UpJumper2List = CloneListSetSelected(jumperList, editModel.UpJumper2);
        }

        private List<SelectListItem> BuildJumperList()
        {
            return _db.Jumpers
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x=> new
                {
                    x.JumperID,
                    x.FirstName,
                    x.LastName
                })
                .ToList()
                .Select(x => new SelectListItem
                {
                    Text = $"{x.FirstName} {x.LastName}",
                    Value = x.JumperID.ToString(),
                })
                .ToList();
        }

        private List<SelectListItem> CloneListSetSelected(List<SelectListItem> selectList, int selectedID)
        {
            var clone = new List<SelectListItem>(selectList);
            clone.Single(x => x.Value == selectedID.ToString()).Selected = true;
            return clone;
        }

        public void Save(ScoresEditModel editModel)
        {
            var row = _db.RoundJumperMaps.Single(x => x.ID == editModel.RoundJumperMapID);
            row.Camera = editModel.Camera;
            row.Score = editModel.Score;
            row.DownJumper1ID = editModel.DownJumper1;
            row.DownJumper2ID = editModel.DownJumper2;
            row.UpJumper1ID = editModel.UpJumper1;
            row.UpJumper2ID = editModel.UpJumper2;
            row.VideoUrl = editModel.VideoUrl;
            _db.SaveChanges();
        }

        internal IEnumerable<ResultsListRow> GetResultsList(UpDownFlag upDown)
        {
            const string sql = @"
SELECT j.FirstName + ' ' + j.LastName Name, ISNULL(SUM(Score), 0) TotalScore, COUNT(m.ID) TotalJumps
FROM dbo.Jumper j
	LEFT OUTER JOIN dbo.RoundJumperMap m  ON (
			m.UpJumper1ID = j.JumperID
			OR m.UpJumper2ID = j.JumperID
			OR m.DownJumper1ID = j.JumperID
			OR m.DownJumper2ID = j.JumperID)
WHERE j.RandomizedUpDown = @p0
GROUP BY j.FirstName + ' ' + j.LastName";
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