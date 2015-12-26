using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper.Tests.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}
