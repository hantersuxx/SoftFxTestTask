using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Symbol
    {
        public Symbol()
        {
            Quotes = new List<Quote>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<Quote> Quotes { get; set; }
    }
}