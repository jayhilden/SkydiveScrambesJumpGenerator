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

        public List<DrawSingleJump> GetDraw(JumpGroupFlag leftRight)
        {
            throw new NotImplementedException();
        }
    }
}