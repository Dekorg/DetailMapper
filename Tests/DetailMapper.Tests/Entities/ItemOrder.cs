using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper.Tests.Entities
{
    public class ItemOrder
    {
        public int Id { get; set; }

        // ForeignKey of Order
        public int OrderID { get; set; }

        public Order Order { get; set; }


        public decimal Quantity { get; set; }
        public string ProductId { get; set; }
    }
}
