# DetailMapper
Mapping framework of Detail relationships for ORM (EntityFramework, NHibernate, etc).

Installation Nuget
```nuget
Install-Package DetailMapper
```

Example clases
```csharp
// Example DTO Classes
public class OrderDTO
{
    public int Id { get; set; }
    public List<ItemDTO> Items { get; set; }
}
public class ItemDTO
{
    public int Id { get; set; }
    public decimal Quantity { get; set; }
    public string ProductId { get; set; }
}

// Example Entity Classes
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
public class ItemOrder
{
    public int Id { get; set; }
    // ForeignKey of Order
    public int OrderID { get; set; }
    public Order Order { get; set; }
    public decimal Quantity { get; set; }
    public string ProductId { get; set; }
}

// Repositories
public interface IItemRepository
{
    void Insert(Order order, ItemOrder item);
    void Update(ItemOrder item);
    void Delete(ItemOrder item);
}
public interface IOrderRepository
{
    Order Find(int orderId);
}

```
Registration
```csharp
using DetailMapper;
/**************************/

// Definition of Mapper. (Should be created only once)
var orderDTOMapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
var itemsMap = orderDTOMapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
    .WithDependencies<IItemRepository>()
    .AddAction((ctx, item) => ctx.Dependencies.Insert(ctx.Master, item))
    .DeleteAction((ctx, item) => ctx.Dependencies.Delete(item))
    .CreateFunc((ctx) => new ItemOrder())
    .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
    .Build();
```
Usage
```csharp
var orderDTO1 = /* Comes from client */;
var order1 = orderRepository.Find(1);

//
itemsMap.Map(orderDTO1, order1, itemRepository, (itemDTO, item) =>
{
    // Mapping of internal properties (Could be set with AutoMapper)
    item.ProductId = itemDTO.ProductId;
    item.Quantity = itemDTO.Quantity;
});
```
After mapping the detail you could apply the changes to the database

If an item has details, the procedure is the same. You must call a Map in the mapper Action.

```csharp
var orderDTO1 = /* Comes from client */;
var order1 = orderRepository.Find(1);

//
itemsMap.Map(orderDTO1, order1, itemRepository, (itemDTO, item) =>
{
    // Mapping of internal properties (Could be set with AutoMapper)
    item.ProductId = itemDTO.ProductId;
    item.Quantity = itemDTO.Quantity;
    
    discountsMap.Map(itemDTO, item, discountsRepository, (discountDTO, discount) =>
    {
        // Mapping of internal properties (Could be set with AutoMapper)
        discount.PromotionCode = discountDTO.PromotionCode;
    });
});
```

### Version
0.0.0.0 // Initial Commit

License
----

MIT

