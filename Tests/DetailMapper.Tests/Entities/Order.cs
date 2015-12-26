using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper.Tests.Entities
{
    public class Order
    {
        public int Id { get; set; }
        private ICollection<ItemOrder> _items = new HashSet<ItemOrder>();
        public virtual ICollection<ItemOrder> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public virtual void AddItem(ItemOrder detail)
        {
            detail.Order = this;
            _items.Add(detail);
        }

        public virtual void RemoveItem(ItemOrder detail)
        {
            _items.Remove(detail);
        }
    }
}
