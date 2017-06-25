using System;
using System.Collections.Generic;
using System.Linq;
using Data.Sql;
using Data.Sql.Models;
using Moq;
using NUnit.Framework;
using Scrambles.Services;

namespace Web.Tests
{
    [TestFixture]
    public class RandomizationWebServiceTests
    {
        private RandomizationWebService _randomizationWebService;
        private readonly Mock<PiiaDb> _dbMock = new Mock<PiiaDb>();
        private readonly List<Jumper> _jumperList = new List<Jumper>();
        private readonly List<Round> _roundList = new List<Round>();
        private readonly List<RoundJumperMap> _roundJumperMap = new List<RoundJumperMap>();

        [OneTimeSetUp]
        public void RunOnce()
        {
            _dbMock.Setup(i => i.Jumpers).Returns(_jumperList.ToDbSet);
            _dbMock.Setup(i => i.RoundJumperMaps).Returns(_roundJumperMap.ToDbSet);
            _dbMock.Setup(i => i.Rounds).Returns(_roundList.ToDbSet);
            _randomizationWebService = new RandomizationWebService(_dbMock.Object);
        }

        [SetUp]
        public void RunEveryTest()
        {
            _jumperList.Clear();
            _roundList.Clear();
            _roundJumperMap.Clear();
        }

        /// <summary>
        /// test the randomization for 8 jumpers, 3 rounds
        /// With this scheme there should NEVER be any duplicates!
        /// </summary>
        [TestCase(8, 3, 0, Description = "max number of rounds with no duplicates")]
        [TestCase(8, 4, 1, Description = "1 duplicate expected")]
        [TestCase(8, 6, 3, Description = "common scenario, 3 dups required")]
        [TestCase(16, 7, 0, Description = "with 16 people, there are 7 total rounds without re-randomizing")]
        [TestCase(12, 4, 0, Description = "max rounds with no dups for 12 jumpers")]
        [TestCase(12, 6, 2, Description = "Common scenario with 12 jumpers")]
        public void AssignJumpersToRounds(int numberOfJumpers, int numberOfRounds, int expectedRoundsWithDups)
        {
            //Arrange
            const JumpGroupFlag grp = JumpGroupFlag.Left;
            var maxJumpCount = 1000;
            for (var i = 1; i <= numberOfJumpers / 2; i++)
            {
            var up = new Jumper {FirstName = $"up{i}", JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount--, JumperID = 99 + i};
                var down = new Jumper { FirstName = $"down{i}", JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount-- / 2, JumperID = 199 + i };
                _jumperList.Add(up);
                _jumperList.Add(down);
            }

            for (var i = 1; i <= numberOfRounds; i++)
            {
                _roundList.Add(new Round {RoundNumber = i, RoundID = i});
            }
           

            //Act
            _randomizationWebService.AssignJumpersToRounds(grp);

            //Assert
            Assert.IsNotEmpty(_roundJumperMap);
            AssertEveryJumperEveryRound(_roundJumperMap, _jumperList, _roundList);
            AssertNoDuplicates(_roundJumperMap, expectedRoundsWithDups);

        }

        /// <summary>
        /// assert that every jumper is used exactly once in every once
        /// </summary>
        /// <param name="roundJumperMap"></param>
        /// <param name="jumperList"></param>
        /// <param name="roundList"></param>
        private static void AssertEveryJumperEveryRound(List<RoundJumperMap> roundJumperMap, List<Jumper> jumperList, List<Round> roundList)
        {
            foreach (var r in roundList)
            {
                foreach (var j in jumperList)
                {
                    var cnt = roundJumperMap.Count(x =>
                        x.RoundID == r.RoundID
                        && (x.UpJumper1ID == j.JumperID
                            || x.UpJumper2ID == j.JumperID
                            || x.DownJumper1ID == j.JumperID
                            || x.DownJumper2ID == j.JumperID));
                    Assert.AreEqual(1, cnt, $"Count is incorrect for round {r.RoundNumber}, jumper {j}");
                }
            }
        }

        private static void AssertNoDuplicates(List<RoundJumperMap> roundJumperMap, int expectedDuplicatePairs)
        {
            var hashMap4 = new HashSet<List<int>>();
            var hashMap2 = new HashSet<Tuple<int, int>>();
            var duplicateRounds = new HashSet<int>();
            foreach (var i in roundJumperMap)
            {
                var jumpers = new List<int>
                {
                    i.UpJumper1ID,
                    i.UpJumper2ID,
                    i.DownJumper1ID,
                    i.DownJumper2ID
                };
                if (hashMap4.Contains(jumpers))
                {
                    Assert.Fail($"all 4 jumpers are duplicated, that is never acceptable!  {i}");
                }
                hashMap4.Add(jumpers);

                var pair1 = new Tuple<int, int>(i.UpJumper1ID, i.UpJumper2ID);
                if (hashMap2.Contains(pair1))
                {
                    Console.WriteLine("Pair1 is duplicated: " + i);
                    duplicateRounds.Add(i.RoundID);
                }
                hashMap2.Add(pair1);

                var pair2 = new Tuple<int, int>(i.DownJumper1ID, i.DownJumper2ID);
                if (hashMap2.Contains(pair2))
                {
                    Console.WriteLine("Pair2 is duplicated: " + i);
                    duplicateRounds.Add(i.RoundID);
                }
                hashMap2.Add(pair2);
            }
            Assert.AreEqual(expectedDuplicatePairs, duplicateRounds.Count, "Duplicate pairs count is not expected");
        }
    }
}
