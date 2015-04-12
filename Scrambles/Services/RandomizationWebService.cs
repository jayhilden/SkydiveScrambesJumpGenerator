using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Sql;
using Data.Sql.Models;

namespace Scrambles.Services
{
    public class RandomizationWebService
    {
        private readonly PiiaDb _db;

        public RandomizationWebService(PiiaDb db)
        {
            _db = db;
        }

        /// <summary>
        /// put the jumpers into up/down and left/right groups
        /// </summary>
        public void Randomize()
        {
            var mod = _db.Jumpers.Count()%4;
            if (mod != 0)
            {
                var msg = string.Format("The number of jumpers must be divisible by 4, please add {0} placehold jumpers", 4 - mod);
                throw new Exception(msg);
            }
            AssignUpDown();
            AssignGroup();
        }

        /// <summary>
        /// Assign 2 up jumpers and 2 down jumpers to a team
        /// </summary>
        public void AssignNumbersToRound(Round r, List<Jumper> upJumpers, List<Jumper> downJumpers, int numberOfTeams)
        {
            var skipCount = r.RoundNumber;
            var retval = new List<string>();
            var upCount = upJumpers.Count;
            for (var i = 0; i < numberOfTeams; i++)
            {
                var up1 = upJumpers[i % upCount];
                var up2 = upJumpers[i + skipCount % upCount];
                var down1 = downJumpers[i%upCount];
                var down2 = downJumpers[i + skipCount % upCount];
            }

        }

        #region privates

        private void AssignUpDown()
        {
            var cnt = _db.Jumpers.Count();
            var upJumperCount = Math.Ceiling(cnt/(decimal) 2);
            var i = 0;
            foreach (var jumper in _db.Jumpers.OrderByDescending(x => x.NumberOfJumps))
            {
                var upDown = (++i <= upJumperCount) ? UpDownFlag.UpJumper : UpDownFlag.DownJumper;
                jumper.RandomizedUpDown = upDown;
            }
            _db.SaveChanges();
        }

        private void AssignGroup()
        {
            var i = 0;
            foreach (var jumper in _db.Jumpers.OrderByDescending(x => x.NumberOfJumps))
            {
                var leftRight = (++i%2 == 0) ? JumpGroupFlag.Left : JumpGroupFlag.Right;
                jumper.JumpGroup = leftRight;
            }
            _db.SaveChanges();
        }

        #endregion

    }
}