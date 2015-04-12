﻿using System;
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
            var mod = _db.Jumpers.Count()%8;
            if (mod != 0)
            {
                var msg = string.Format("The number of jumpers must be divisible by 8, please add {0} placehold jumpers", 8 - mod);
                throw new Exception(msg);
            }            
            AssignLeftRight();
            AssignUpDown(JumpGroupFlag.Left);
            AssignUpDown(JumpGroupFlag.Right);
            _db.RoundJumperMaps.RemoveRange(_db.RoundJumperMaps);
            _db.SaveChanges();
            AssignJumpersToRound(JumpGroupFlag.Left);
            AssignJumpersToRound(JumpGroupFlag.Right);
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
            var i = 0;
            foreach (var jumper in _db.Jumpers.OrderByDescending(x => x.NumberOfJumps))
            {
                var leftRight = (++i%2 == 0) ? JumpGroupFlag.Left : JumpGroupFlag.Right;
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
                AssignNumbersToRound(round, up.Clone(), down.Clone());
            }
        }

        /// <summary>
        /// Assign 2 up jumpers and 2 down jumpers to a team
        /// </summary>
        private void AssignNumbersToRound(Round r, IList<Jumper> upJumpers, IList<Jumper> downJumpers)
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
                    DownJumper2ID = down2.JumperID
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