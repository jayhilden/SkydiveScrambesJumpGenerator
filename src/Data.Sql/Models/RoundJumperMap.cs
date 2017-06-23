using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Data.Sql.Models
{
    public class RoundJumperMap
    {
        public int ID { get; set; }

        public int RoundID { get; set; }

        [ForeignKey("UpJumper1")]
        public int UpJumper1ID { get; set; }

        [ForeignKey("UpJumper2")]
        public int UpJumper2ID { get; set; }

        [ForeignKey("DownJumper1")]
        public int DownJumper1ID { get; set; }

        [ForeignKey("DownJumper2")]
        public int DownJumper2ID { get; set; }

        public JumpGroupFlag JumpGroup { get; set; }

        public int? Score { get; set; }

        [MaxLength(100)]
        public string Camera { get; set; }

        [MaxLength(1000)]
        public string VideoUrl { get; set; }

        public virtual Round Round { get; set; }
        
        public virtual Jumper UpJumper1 { get; set; }
        public virtual Jumper UpJumper2 { get; set; }
        public virtual Jumper DownJumper1 { get; set; }
        public virtual Jumper DownJumper2 { get; set; }

        public override string ToString()
        {
            return
                $"ID {ID}, RoundID {RoundID}, UP1 {UpJumper1ID}, UP2 {UpJumper2ID}, DOWN1 {DownJumper1ID}, DOWN2 {DownJumper2ID}";
        }
    }
}
