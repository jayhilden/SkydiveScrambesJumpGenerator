using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Sql.Models
{
    public enum ConfigurationKeys
    {
        RandomizationLocked = 1,
        AdminPassword = 2,
        BcryptLoopCount = 3
    }

    public class Configuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ConfigurationKeys ConfigurationID { get; set; }

        [MaxLength(100)]
        public string ConfigurationKey { get; set; }

        [MaxLength(100)]
        public string ConfigurationValue { get; set; }
    }
}
