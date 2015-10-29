using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool RandomizationLocked()
        {
            var value = _db.Configurations
                .Where(x => x.ConfigurationID == (int)ConfigurationKeys.RandomizationLocked)
                .Select(x=>x.ConfigurationValue)
                .Single();
            return int.Parse(value) == 1;
        }

        public void LockUnlockRandomization(bool locked)
        {
            var stringVal = locked ? "1" : "0";
            var db = _db.Configurations.Single(x => x.ConfigurationID == (int) ConfigurationKeys.RandomizationLocked);
            db.ConfigurationValue = stringVal;
            _db.SaveChanges();
        }


        /// <summary>
        /// put the jumpers into up/down and left/right groups
        /// </summary>
        public void Randomize()
        {
            var mod = _db.Jumpers.Count()%4;
            if (mod != 0)
            {
                var msg = $"The number of jumpers must be divisible by 4, please add {4 - mod} placehold jumpers";
                throw new Exception(msg);
            }
            RemoveRandomization();
            AssignLeftRight();
            AssignUpDown(JumpGroupFlag.Left);
            AssignUpDown(JumpGroupFlag.Right);
            AssignJumpersToRounds(JumpGroupFlag.Left);
            AssignJumpersToRounds(JumpGroupFlag.Right);
            LockUnlockRandomization(locked: true);
        }

        /// <summary>
        /// delete the randomization, this is necessary if you want to add or remove a jumper
        /// </summary>
        public void RemoveRandomization()
        {
            if (RandomizationLocked())
            {
                throw new Exception("Randomization is locked");
            }
            _db.RoundJumperMaps.RemoveRange(_db.RoundJumperMaps);
            foreach (var j in _db.Jumpers.Where(x=>x.JumpGroup != null || x.RandomizedUpDown != null))
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
            var total = _db.Jumpers.Count();
            //if we have less than 8*4 people then the randomization won't work well with 2 separate
            //groups, so just put everyone in one group and move on
            if (total < 8*4)
            {
                _db.ExecuteSqlCommand("UPDATE Jumper SET JumpGroup = @p0", (int) JumpGroupFlag.Left);
                return;
            }

            //if the # of jumpers is divisible by 8 then it will be even
            //else if the # is divisible by 4 then put the last 4 all on the same side
            var i = 0;
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


        private void AssignJumpersToRounds(JumpGroupFlag group)
        {
            var jumpers = _db.Jumpers.Where(x => x.JumpGroup == group);
            if (!jumpers.Any())
            {
                return;
            }
            var up = jumpers.Where(x => x.RandomizedUpDown == UpDownFlag.UpJumper).OrderByDescending(x => x.NumberOfJumps).ToList();
            var down = jumpers.Where(x => x.RandomizedUpDown == UpDownFlag.DownJumper).OrderByDescending(x => x.NumberOfJumps).ToList();
            LinkedList<Jumper> a, b, c, d;
            AddJumpersToLinkedLists(up, out a, out b);
            AddJumpersToLinkedLists(down, out c, out d);

            var teamCount = a.Count;
            var aStart = a.First;
            var bStart = b.First;
            var cStart = c.First;
            var dStart = d.First;
            foreach (var round in _db.Rounds.OrderBy(x => x.RoundNumber))
            {
                bStart = bStart.NextOrFirst();//+1
                cStart = cStart.NextOrFirst().NextOrFirst();//+2
                dStart = dStart.NextOrFirst().NextOrFirst().NextOrFirst();//+3
                AssignNumbersToRound(round, group, teamCount, aStart, bStart, cStart, dStart);
            }
            _db.SaveChanges();
        }




        /// <summary>
        /// Assign 2 up jumpers and 2 down jumpers to a team
        /// </summary>
        private void AssignNumbersToRound(Round r, JumpGroupFlag leftRight, int teamCount, LinkedListNode<Jumper> up1, LinkedListNode<Jumper> up2, LinkedListNode<Jumper> down1, LinkedListNode<Jumper> down2)
        {
            for(var i = 0; i<teamCount;i++)
            {
                var nextUp1 = up1.Value;
                var nextUp2 = up2.Value;
                var nextDown1 = down1.Value;
                var nextDown2 = down2.Value;
                var map = new RoundJumperMap
                {
                    RoundID = r.RoundID,
                    UpJumper1ID = nextUp1.JumperID,
                    UpJumper2ID = nextUp2.JumperID,
                    DownJumper1ID = nextDown1.JumperID,
                    DownJumper2ID = nextDown2.JumperID,
                    JumpGroup = leftRight
                };
                _db.RoundJumperMaps.Add(map);
                up1 = up1.NextOrFirst();
                up2 = up2.NextOrFirst();
                down1 = down1.NextOrFirst();
                down2 = down2.NextOrFirst();
            }            
        }

        private static void AddJumpersToLinkedLists(List<Jumper> jumpers, out LinkedList<Jumper> even, out LinkedList<Jumper> odd)
        {
            odd = new LinkedList<Jumper>();
            even = new LinkedList<Jumper>();
            for (var i = 0; i < jumpers.Count; i++)
            {
                var jumper = jumpers[i];
                if (i % 2 == 0)
                {
                    even.AddLast(jumper);
                }
                else
                {
                    odd.AddLast(jumper);
                }
            }
        }

        #endregion

    }
}