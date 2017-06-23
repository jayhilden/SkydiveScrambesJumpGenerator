using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private RandomizationWebService randomizationWebService;
        private readonly Mock<PiiaDb> dbMock = new Mock<PiiaDb>();
        private readonly List<Jumper> jumperList = new List<Jumper>();
        private readonly List<Round> roundList = new List<Round>();
        private readonly List<RoundJumperMap> roundJumperMap = new List<RoundJumperMap>();

        [OneTimeSetUp]
        public void RunOnce()
        {
            dbMock.Setup(i => i.Jumpers).Returns(jumperList.ToDbSet);
            dbMock.Setup(i => i.RoundJumperMaps).Returns(roundJumperMap.ToDbSet);
            dbMock.Setup(i => i.Rounds).Returns(roundList.ToDbSet);
            randomizationWebService = new RandomizationWebService(dbMock.Object);
        }

        /// <summary>
        /// test the randomization for 8 jumpers, 5 rounds
        /// </summary>
        [Test]
        public void AssignJumpersToRounds()
        {
            //Arrange
            var grp = JumpGroupFlag.Left;
            var maxJumpCount = 1000;
            var up1 = new Jumper {JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount--};
            var up2 = new Jumper { JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount-- };
            var up3 = new Jumper { JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount-- };
            var up4 = new Jumper { JumpGroup = grp, RandomizedUpDown = UpDownFlag.UpJumper, NumberOfJumps = maxJumpCount-- };

            var down1 = new Jumper { JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount-- };
            var down2 = new Jumper { JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount-- };
            var down3 = new Jumper { JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount-- };
            var down4 = new Jumper { JumpGroup = grp, RandomizedUpDown = UpDownFlag.DownJumper, NumberOfJumps = maxJumpCount-- };
            
            jumperList.AddRange(new List<Jumper> {up1, up2, up3, up4, down1, down2, down3, down4});

            var round1 = new Round {RoundNumber = 1, RoundID = Rand.NextInt()};
            var round2 = new Round { RoundNumber = 2, RoundID = Rand.NextInt() };
            var round3 = new Round { RoundNumber = 3, RoundID = Rand.NextInt() };
            var round4 = new Round { RoundNumber = 4, RoundID = Rand.NextInt() };
            var round5 = new Round { RoundNumber = 5, RoundID = Rand.NextInt() };

            roundList.AddRange(new List<Round> {round1, round2, round3, round4, round5});

            //Act
            randomizationWebService.AssignJumpersToRounds(grp);

            //Assert
            //round 1
            Assert.IsNotEmpty(roundJumperMap);
            var round1Map = roundJumperMap.Where(x => x.RoundID == round1.RoundID);
            Assert.AreEqual(2, round1Map.Count());
        }
    }
}
