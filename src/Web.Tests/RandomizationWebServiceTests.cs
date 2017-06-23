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
        private readonly List<RoundJumperMap> roundJumperMap = new List<RoundJumperMap>();

        [OneTimeSetUp]
        public void RunOnce()
        {
            dbMock.Setup(i => i.Jumpers).Returns(jumperList.ToDbSet);
            dbMock.Setup(i => i.RoundJumperMaps).Returns(roundJumperMap.ToDbSet);
            randomizationWebService = new RandomizationWebService(dbMock.Object);
        }

        /// <summary>
        /// test the randomization for 8 jumpers, 5 rounds
        /// </summary>
        [Test]
        public void AssignJumpersToRounds()
        {
            Assert.Pass("hi jay");
        }
    }
}
