using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrambles.Models
{
    public class ResultsVM
    {
        public IEnumerable<ResultsListRow> UpJumperScores { get; private set; }
        public IEnumerable<ResultsListRow> DownJumperScores { get; private set; }

        public ResultsVM(IEnumerable<ResultsListRow> upJumperScores, IEnumerable<ResultsListRow> downJumperScores)
        {
            UpJumperScores = upJumperScores;
            DownJumperScores = downJumperScores;
        }
    }
}