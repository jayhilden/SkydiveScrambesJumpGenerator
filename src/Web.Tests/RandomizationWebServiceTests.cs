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

        /// <summary>
        /// test the randomization for 8 jumpers, 3 rounds
        /// With this scheme there should NEVER be any duplicates!
        /// </summary>
        [Test]
        public void AssignJumpersToRounds()
        {
            //Arrange
            const JumpGroupFlag grp = JumpGroupFlag.Left;
            var maxJumpCount = 1000;
            var up1 = new Jumper {FirstName = "up1", JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount--, JumperID = 100};
            var up2 = new Jumper {FirstName = "up2", JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount--, JumperID = 101 };
            var up3 = new Jumper { FirstName = "up3", JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount--, JumperID = 102 };
            var up4 = new Jumper { FirstName = "up4", JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount--, JumperID = 103 };

            var down1 = new Jumper { FirstName = "down1", JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount--, JumperID = 200 };
            var down2 = new Jumper { FirstName = "down2", JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount--, JumperID = 201 };
            var down3 = new Jumper { FirstName = "down3", JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount--, JumperID = 202 };
            var down4 = new Jumper { FirstName = "down4", JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount, JumperID = 203 };
            
            _jumperList.AddRange(new List<Jumper> {up1, up2, up3, up4, down1, down2, down3, down4});

            var round1 = new Round {RoundNumber = 1, RoundID = 1};
            var round2 = new Round { RoundNumber = 2, RoundID =2 };
            var round3 = new Round { RoundNumber = 3, RoundID = 3 };
            //var round4 = new Round { RoundNumber = 4, RoundID = 4 };
           // var round5 = new Round { RoundNumber = 5, RoundID = 5 };

            _roundList.AddRange(new List<Round> {round1, round2, round3});

            //Act
            _randomizationWebService.AssignJumpersToRounds(grp);

            //Assert
            //round 1
            _roundJumperMap.ForEach(Console.WriteLine);
            Assert.IsNotEmpty(_roundJumperMap);
            AssertNoDuplicates(_roundJumperMap);
            //var round1Map = _roundJumperMap
            //    .Where(x => x.RoundID == round1.RoundID)
            //    .OrderBy(y=> y.RoundID)
            //    .ThenBy(y=> y.ID)
            //    .ToList();
            
            //Assert.AreEqual(2, round1Map.Count);
            //AssertRoundMap(round1Map[0], 1, up1, up2, down1, down2);
            //AssertRoundMap(round1Map[1], 2, up3, up4, down3, down4);
        }

        private void AssertNoDuplicates(List<RoundJumperMap> roundJumperMap)
        {
            var hashMap4 = new HashSet<List<int>>();
            var hashMap2 = new HashSet<Tuple<int, int>>();
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
                    Assert.Fail("Pair1 is duplicated: " + i);
                }
                hashMap2.Add(pair1);

                var pair2 = new Tuple<int, int>(i.DownJumper1ID, i.DownJumper2ID);
                if (hashMap2.Contains(pair2))
                {
                    Assert.Fail("Pair2 is duplicated: " + i);
                }
                hashMap2.Add(pair2);


            }
            Assert.Pass("No duplicates found!");
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
