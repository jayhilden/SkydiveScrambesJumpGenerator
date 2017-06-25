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

        //internal void AssignJumpersToRoundsWithRetry(JumpGroupFlag group)
        //{
        //    for (var i = 0; i < 10; i++)
        //    {
        //        try
        //        {
        //            AssignJumpersToRounds(group);
        //            return;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //    }
        //    throw new Exception($"{nameof(AssignJumpersToRounds)} failed 10 times");
        //}


        internal void AssignJumpersToRounds(JumpGroupFlag group)
        {
            var jumpers = _db.Jumpers.Where(x => x.JumpGroup == group);
            if (!jumpers.Any())
            {
                return;
            }
            var teamCount = jumpers.Count() / 4;
            var up = jumpers.Where(x => x.RandomizedUpDown == UpDownFlag.UpJumper)
                .OrderByDescending(x => x.NumberOfJumps).ToList();
            var down = jumpers.Where(x => x.RandomizedUpDown == UpDownFlag.DownJumper)
                .OrderByDescending(x => x.NumberOfJumps).ToList();


            List<Tuple<Jumper, Jumper>> upCombinations = null;
            List<Tuple<Jumper, Jumper>> downCombinations = null;
            foreach (var round in _db.Rounds.OrderBy(x => x.RoundNumber))
            {


                var retryCount = 0;
                while (true)
                {
                    if (upCombinations == null || upCombinations.Count == 0)
                    {
                        //new randomized list
                        upCombinations = GetCombinationPairs(up);
                        downCombinations = GetCombinationPairs(down);
                    }
                    var roundMap = AssignJumpersToRounds(round, group, teamCount, ref upCombinations,
                        ref downCombinations);
                    if (roundMap != null)
                    {
                        _db.RoundJumperMaps.AddRange(roundMap);
                        break;
                    }
                    Console.WriteLine($"{nameof(AssignJumpersToRounds)} failed, retry count = {retryCount}");
                    if (++retryCount > 10)
                    {
                        Console.WriteLine("ERROR: failed 10 times, recreating");
                        upCombinations = null;
                        downCombinations = null;
                    }
                    else
                    {
                        //re-randomize the available combinations
                        upCombinations = upCombinations.Shuffle().ToList();
                        downCombinations = downCombinations.Shuffle().ToList();
                    }
                }
            }
            _db.SaveChanges();
        }

        private List<RoundJumperMap> AssignJumpersToRounds(
            Round r, 
            JumpGroupFlag leftRight, 
            int teamCount, 
            ref List<Tuple<Jumper, Jumper>> upAvailableCombinationsOriginal, 
            ref List<Tuple<Jumper, Jumper>> downAvailableCombinationsOriginal)
        {
            var roundMap = new List<RoundJumperMap>();
            var jumpersUsedThisRound = new HashSet<int>();
            //create clones because we may need to rollback
            var upAvailableCombinations = upAvailableCombinationsOriginal.Clone();
            var downAvailableCombinations = downAvailableCombinationsOriginal.Clone();
            for (var i = 1; i <= teamCount; i++)
            {
                var nextUpPair = upAvailableCombinations
                    .FirstOrDefault(pair =>
                        !jumpersUsedThisRound.Contains(pair.Item1.JumperID)
                        && !jumpersUsedThisRound.Contains(pair.Item2.JumperID)
                    );
                var nextDownPair = downAvailableCombinations
                    .FirstOrDefault(pair =>
                        !jumpersUsedThisRound.Contains(pair.Item1.JumperID)
                        && !jumpersUsedThisRound.Contains(pair.Item2.JumperID)
                    );
                if (nextUpPair == null)
                {
                    Console.WriteLine("failed to find up pair");
                }
                if (nextDownPair == null)
                {
                    Console.WriteLine("failed to find down pair");
                }
                if (nextUpPair == null || nextDownPair == null)
                {
                    return null;
                }
                var map = new RoundJumperMap
                {
                    RoundID = r.RoundID,
                    UpJumper1ID = nextUpPair.Item1.JumperID,
                    UpJumper2ID = nextUpPair.Item2.JumperID,
                    DownJumper1ID = nextDownPair.Item1.JumperID,
                    DownJumper2ID = nextDownPair.Item2.JumperID,
                    JumpGroup = leftRight
                };
                Console.WriteLine(map);
                roundMap.Add(map);
                jumpersUsedThisRound.Add(nextUpPair.Item1.JumperID);
                jumpersUsedThisRound.Add(nextUpPair.Item2.JumperID);
                jumpersUsedThisRound.Add(nextDownPair.Item1.JumperID);
                jumpersUsedThisRound.Add(nextDownPair.Item2.JumperID);
                upAvailableCombinations.Remove(nextUpPair);
                downAvailableCombinations.Remove(nextDownPair);
            }
            upAvailableCombinationsOriginal = upAvailableCombinations;
            downAvailableCombinationsOriginal = downAvailableCombinations;
            return roundMap;
        }

        private static List<Tuple<Jumper, Jumper>> GetCombinationPairs(List<Jumper> list)
        {
            var tuples = list
                .Select((value, index) => new {value, index})
                .SelectMany(x =>
                        list.Skip(x.index + 1),
                    (x, y) => Tuple.Create(x.value, y)
                )
                .ToList()
                .Shuffle();

            var tuplesShuffled = tuples.ToList();
            tuplesShuffled.ForEach(i => Console.WriteLine($"[{i.Item1.JumperID}, {i.Item2.JumperID}]"));
            return tuplesShuffled;
        }

        #endregion

    }
}