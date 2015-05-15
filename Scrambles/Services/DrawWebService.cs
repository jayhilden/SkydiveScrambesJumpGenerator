using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Sql;
using Data.Sql.Models;
using Scrambles.Models;

namespace Scrambles.Services
{
    public class DrawWebService
    {
        private readonly PiiaDb _db;

        public DrawWebService(PiiaDb db)
        {
            _db = db;
        }

        public IEnumerable<DrawSingleJump> GetDraw(JumpGroupFlag leftRight)
        {
            var draws = _db.RoundJumperMaps
                .Where(x => x.JumpGroup == leftRight)
                .OrderBy(x=>x.Round.RoundNumber);

            var drawSingleJumps = from d in draws
                select new DrawSingleJump
                {
                    Jumper1 = new DrawJumper
                    {
                        Name = d.UpJumper1.FirstName + " " + d.UpJumper1.LastName,
                        NumberOfJumps = d.UpJumper1.NumberOfJumps
                    },
                    Jumper2 = new DrawJumper
                    {
                        Name = d.UpJumper2.FirstName + " " + d.UpJumper2.LastName,
                        NumberOfJumps = d.UpJumper2.NumberOfJumps
                    },
                    Jumper3 = new DrawJumper
                    {
                        Name = d.DownJumper1.FirstName + " " + d.DownJumper1.LastName,
                        NumberOfJumps = d.DownJumper1.NumberOfJumps
                    },
                    Jumper4 = new DrawJumper
                    {
                        Name = d.DownJumper2.FirstName + " " + d.DownJumper2.LastName,
                        NumberOfJumps = d.DownJumper2.NumberOfJumps
                    },
                };
            return drawSingleJumps;
        }

        public DrawJumper RepoToUI(Jumper jumper)
        {
            return new DrawJumper
            {
                Name = jumper.FirstName + " " + jumper.LastName,
                NumberOfJumps = jumper.NumberOfJumps
            };
        }
    }
}