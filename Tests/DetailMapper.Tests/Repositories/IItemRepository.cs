using DetailMapper.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper.Tests.Repositories
{
    public interface IItemRepository
    {
        void Insert(Order order, ItemOrder item);
        void Update(ItemOrder item);
        void Delete(ItemOrder item);
    }
}
