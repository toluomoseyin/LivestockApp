using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaymentHistoryDto
    {
        public string UserId { get; set; }
        public UserToReturnDto User { get; set; }
        public string Id { get; set; }
        public int Page { get; set; }
        public string DateCreated { get; set; }
        public string DeleiveryDate { get; set; }
        public byte PaymentStatus { get; set; }
        public decimal PaymentAmount { get; set; }
        public byte Status { get; set; }
    }
}
