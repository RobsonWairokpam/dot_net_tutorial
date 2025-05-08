using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stocks
{
    public class CreateStockDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.00001, 100)]
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public decimal MarketCap { get; set; }
    }
}
