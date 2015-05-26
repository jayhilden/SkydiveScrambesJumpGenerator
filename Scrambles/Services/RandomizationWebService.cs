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
            RemoveRandomization();
            AssignLeftRight();
            AssignUpDown(JumpGroupFlag.Left);
            AssignUpDown(JumpGroupFlag.Right);
            AssignJumpersToRound(JumpGroupFlag.Left);
            AssignJumpersToRound(JumpGroupFlag.Right);
        }

        /// <summary>
        /// delete the randomization, this is necessary if you want to add or remove a jumper
        /// </summary>
        public void RemoveRandomization()
        {
            _db.RoundJumperMaps.RemoveRange(_db.RoundJumperMaps);
            foreach (var j in _db.Jumpers)
            {
                j.RandomizedLetter = null;
                j.JumpGroup = null;
                j.RandomizedUpDown = null;
            }
            _db.SaveChanges();
        }

        #region privates

        private void AssignUpDown(JumpGroupFlag leftRight)
        {
            var jumpersIngroup = _db.Jumpers.Where(x => x.JumpGroup == leftRight);
            var cnt = jumpersIngroup.Count();
            var upJumperCount = cnt/2;
            var i = 0;
            foreach (var jumper in jumpersIngroup.OrderByDescending(x => x.NumberOfJumps))
            {
                var upDown = (++i <= upJumperCount) ? UpDownFlag.UpJumper : UpDownFlag.DownJumper;
                jumper.RandomizedUpDown = upDown;
            }
            _db.SaveChanges();
        }

        private void AssignLeftRight()
        {
            //if the # of jumpers is divisible by 8 then it will be even
            //else if the # is divisible by 4 then put the last 4 all on the same side
            var i = 0;
            var total = _db.Jumpers.Count();
            var last4Left = total%8 != 0;//send the last 4 jumpers all to the left
            foreach (var jumper in _db.Jumpers.OrderByDescending(x => x.NumberOfJumps))
            {
                i++;
                JumpGroupFlag leftRight;
                if (last4Left && total - i <= 4)
                {
                    leftRight = JumpGroupFlag.Left;
                }
                else
                {
                    leftRight = (i%2 == 0) ? JumpGroupFlag.Left : JumpGroupFlag.Right;
                }
                jumper.JumpGroup = leftRight;
            }
            _db.SaveChanges();
        }


        private void AssignJumpersToRound(JumpGroupFlag group)
        {
            var jumpers = _db.Jumpers.Where(x => x.JumpGroup == group);
            var up = jumpers.Where(x => x.RandomizedUpDown == UpDownFlag.UpJumper).OrderByDescending(x=>x.NumberOfJumps).ToList();
            var down = jumpers.Where(x => x.RandomizedUpDown == UpDownFlag.DownJumper).OrderByDescending(x => x.NumberOfJumps).ToList();
            var roundList = _db.Rounds.OrderBy(x => x.RoundNumber).ToList();
            foreach (var round in roundList)
            {
                AssignNumbersToRound(round, group, up.Clone(), down.Clone());
            }
        }

        /// <summary>
        /// Assign 2 up jumpers and 2 down jumpers to a team
        /// </summary>
        private void AssignNumbersToRound(Round r, JumpGroupFlag leftRight, IList<Jumper> upJumpers, IList<Jumper> downJumpers)
        {
            upJumpers = upJumpers.Shuffle().ToList();
            downJumpers = downJumpers.Shuffle().ToList();
            const int index1 = 0;
            const int index2 = 1;
            while (upJumpers.Count > 0)
            {
                var up1 = upJumpers[index1];
                var up2 = upJumpers[index2];
                var down1 = downJumpers[index1];
                var down2 = downJumpers[index2];
                var map = new RoundJumperMap
                {
                    RoundID = r.RoundID,
                    UpJumper1ID = up1.JumperID,
                    UpJumper2ID = up2.JumperID,
                    DownJumper1ID = down1.JumperID,
                    DownJumper2ID = down2.JumperID,
                    JumpGroup = leftRight
                };
                _db.RoundJumperMaps.Add(map);
                upJumpers.Remove(up1);
                upJumpers.Remove(up2);
                downJumpers.Remove(down1);
                downJumpers.Remove(down2);
            }
            _db.SaveChanges();

        }

        #endregion

    }
}