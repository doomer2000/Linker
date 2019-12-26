using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlank.Models
{
    public class UrlInfo
    {
        [Key]
        [RegularExpression("\\b(?=[a-z]*[0-9])(?=[0-9]*[a-z])[a-z0-9]{3,6}\\b", ErrorMessage= "Minimum one number(0-9), minimum one letter(a-z), 2 < length < 7.")]
        [Column(TypeName = "VARCHAR(6)")]
        public string Hash { get; set; }
        [Column(TypeName = "VARCHAR(2048)")]
        [Required]
        [Url(ErrorMessage = "Please enter valid url.")]
        public string Url { get; set; }
        [Required]
        public DateTime CreatonTime { get; set; }
        [Required]
        public int UsesCount { get; set; }
    }
}
