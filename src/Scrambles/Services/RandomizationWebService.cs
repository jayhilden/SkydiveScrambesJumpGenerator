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
                .Where(x => x.ConfigurationID == ConfigurationKeys.RandomizationLocked)
                .Select(x=>x.ConfigurationValue)
                .Single();
            return int.Parse(value) == 1;
        }

        public void LockUnlockRandomization(bool locked)
        {
            var stringVal = locked ? "1" : "0";
            var db = _db.Configurations.Single(x => x.ConfigurationID == ConfigurationKeys.RandomizationLocked);
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


        internal void AssignJumpersToRounds(JumpGroupFlag group)
        {
            var jumpers = _db.Jumpers.Where(x => x.JumpGroup == group);
            if (!jumpers.Any())
            {
                return;
            }
            var up = jumpers.Where(x => x.RandomizedUpDown == UpDownFlag.UpJumper).OrderByDescending(x => x.NumberOfJumps).ToList();
            var down = jumpers.Where(x => x.RandomizedUpDown == UpDownFlag.DownJumper).OrderByDescending(x => x.NumberOfJumps).ToList();
            var upCombinations = GetCombinationPairs(up);
            var downCombinations = GetCombinationPairs(down);
            var startIndex = 0;
            //LinkedList<Jumper> a, b, c, d;
            //AddJumpersToLinkedLists(up, out a, out b);
            //AddJumpersToLinkedLists(down, out c, out d);

            //var teamCount = up.Count / 2;
            //var upStart = upCombinations.First;
            //var downStart = downCombinations.First;
            //var aStart = a.First;
            //var bStart = b.First;
            //var cStart = c.First;
            //var dStart = d.First;
            foreach (var round in _db.Rounds.OrderBy(x => x.RoundNumber))
            {
                var upLL = new LinkedList<Tuple<Jumper, Jumper>>(upCombinations.Clone());
                var downLL = new LinkedList<Tuple<Jumper, Jumper>>(downCombinations.Clone());
                var upNode = upLL.First;
                var downNode = downLL.First;
                for (var i = 0; i < startIndex; i++)
                {
                    upNode = upNode.Next;
                    downNode = downNode.Next;
                }
                AssignJumpersToRounds(round, group, upNode, downNode);
                startIndex++;
                //upStart = upStart.NextOrFirst();
                //downStart = downStart.NextOrFirst();
                //bStart = bStart.NextOrFirst();//+1
                //cStart = cStart.NextOrFirst().NextOrFirst();//+2
                //dStart = dStart.NextOrFirst().NextOrFirst().NextOrFirst();//+3
                //AssignNumbersToRound(round, group, teamCount, aStart, bStart, cStart, dStart);

            }
            _db.SaveChanges();
        }

        private void AssignJumpersToRounds(Round r, JumpGroupFlag leftRight, LinkedListNode<Tuple<Jumper, Jumper>> upNode, LinkedListNode<Tuple<Jumper, Jumper>> downNode)
        {
            while (upNode != null)
            {
                var map = new RoundJumperMap
                {
                    RoundID = r.RoundID,
                    UpJumper1ID = upNode.Value.Item1.JumperID,
                    UpJumper2ID = upNode.Value.Item2.JumperID,
                    DownJumper1ID = downNode.Value.Item1.JumperID,
                    DownJumper2ID = downNode.Value.Item2.JumperID,
                    JumpGroup = leftRight
                };
                _db.RoundJumperMaps.Add(map);
                upNode = RemoveAlreadyUsedJumpers(upNode);
                downNode = RemoveAlreadyUsedJumpers(downNode);
            }            
        }

        private static LinkedListNode<Tuple<Jumper, Jumper>> RemoveAlreadyUsedJumpers(LinkedListNode<Tuple<Jumper, Jumper>> upCombinations)
        {
            var left = upCombinations.Value.Item1;
            var right = upCombinations.Value.Item2;
            var list = upCombinations.List;
            var itemsToRemove = list.Where(tuple => tuple.Item1 == left || tuple.Item1 == right || tuple.Item2 == left || tuple.Item2 == right).ToList();
            foreach (var toRemove in itemsToRemove)
            {
                list.Remove(toRemove);
            }
            return list.First;
        }

        //private static void AddJumpersToLinkedLists(List<Jumper> jumpers, out LinkedList<Jumper> even, out LinkedList<Jumper> odd)
        //{
        //    odd = new LinkedList<Jumper>();
        //    even = new LinkedList<Jumper>();
        //    for (var i = 0; i < jumpers.Count; i++)
        //    {
        //        var jumper = jumpers[i];
        //        if (i % 2 == 0)
        //        {
        //            even.AddLast(jumper);
        //        }
        //        else
        //        {
        //            odd.AddLast(jumper);
        //        }
        //    }
        //}

        private static List<Tuple<T, T>> GetCombinationPairs<T>(List<T> list)
        {
            var tuples = list
                .Select((value, index) => new {value, index})
                .SelectMany(x =>
                        list.Skip(x.index + 1),
                    (x, y) => Tuple.Create(x.value, y)
                );
            //return new LinkedList<Tuple<T, T>>(tuples);
            return tuples.ToList();
        }

        #endregion

    }
}