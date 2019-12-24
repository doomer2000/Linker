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
        [RegularExpression("[a-z0-9)(@=$]{3,6}", ErrorMessage= "A hash can contain from 3 to 6 characters: numbers, small letters, as well as the same characters( ( , ) , @ , = , $ .);")]
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
