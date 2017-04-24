using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Quote
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SymbolId { get; set; }
        public DateTime DateTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }

        [ForeignKey(nameof(SymbolId))]
        [JsonIgnore]
        public virtual Symbol Symbol { get; set; }
    }
}