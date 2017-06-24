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
        [TestCase(16, 6, 0, Description = "good test case, we usually have 16 people")]
        public void AssignJumpersToRounds(int numberOfJumpers, int numberOfRounds, int expectedDuplicatePairs)
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
            _roundJumperMap.ForEach(Console.WriteLine);
            Assert.IsNotEmpty(_roundJumperMap);
            AssertNoDuplicates(_roundJumperMap, expectedDuplicatePairs * numberOfRounds);

        }

        private static void AssertNoDuplicates(List<RoundJumperMap> roundJumperMap, int expectedDuplicatePairs)
        {
            var hashMap4 = new HashSet<List<int>>();
            var hashMap2 = new HashSet<Tuple<int, int>>();
            int duplicatePairCount = 0;
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
                    Assert.Fail($"jumpers are duplicated!  {i}");
                }
                hashMap4.Add(jumpers);

                var pair1 = new Tuple<int, int>(i.UpJumper1ID, i.UpJumper2ID);
                if (hashMap2.Contains(pair1))
                {
                    Console.WriteLine("Pair1 is duplicated: " + i);
                    duplicatePairCount++;
                }
                hashMap2.Add(pair1);

                var pair2 = new Tuple<int, int>(i.DownJumper1ID, i.DownJumper2ID);
                if (hashMap2.Contains(pair2))
                {
                    Console.WriteLine("Pair2 is duplicated: " + i);
                    duplicatePairCount++;
                }
                hashMap2.Add(pair2);
            }
            Assert.AreEqual(expectedDuplicatePairs, duplicatePairCount, "Duplicate pairs count is not expected");
        }

        private static void AssertRoundMap(RoundJumperMap round, int grpIndex, Jumper up1, Jumper up2, Jumper down1, Jumper down2)
        {
            Assert.AreEqual(up1.JumperID, round.UpJumper1ID, $"round {round.ID}, grp {grpIndex}, UP 1 expected {up1.FirstName}");
            Assert.AreEqual(up2.JumperID, round.UpJumper2ID, $"round {round.ID}, grp {grpIndex}, UP 2 expected {up1.FirstName}");
            Assert.AreEqual(down1.JumperID, round.DownJumper1ID, $"round {round.ID}, grp {grpIndex}, down 1 expected {up1.FirstName}");
            Assert.AreEqual(down2.JumperID, round.DownJumper2ID, $"round {round.ID}, grp {grpIndex}, down 2 expected {up1.FirstName}");
        }
    }
}
