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
        RandomizationLocked = 1
    }

    public class Configuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConfigurationID { get; set; }
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }
    }
}
