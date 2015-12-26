using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DetailMapper;
using DetailMapper.Tests.DTOs;
using DetailMapper.Tests.Entities;
using DetailMapper.Tests.Builders;
using DetailMapper.Tests.Repositories;
using Moq;

namespace DetailMapper.Tests
{
    [TestFixture]
    public class TestDetailMapperBuilder
    {
        OrderDTO _orderDTO1;
        Order _order1;
        [SetUp]
        public void SetUp()
        {
            // Objetos
            // Lista de Items con un item nuevo (0), 2 actualizados (1 , 2 ) y 2 borrados (3, 4)
            _orderDTO1 = new OrderDTOBuilder()
                .With(x => x.Id = 1)
                .With(x => x.Items = new ItemDTOBuilder()
                    .BuildCollection(3, (item, index) =>
                    {
                        item.Id = index;
                        item.ProductId = "Item " + index;
                        item.Quantity = index * 2;
                    }).ToList()
                 )
                .Build();


            // Lista de 4 Items con Ids (1, 2, 3, 4)
            _order1 = new OrderBuilder()
                .With(x => x.Id = 1)
                .With(x => x.Items = new ItemBuilder()
                    .BuildCollection(4, (item, index) =>
                    {
                        item.OrderID = 1;
                        item.Id = index + 1;
                        item.ProductId = "Item " + index;
                    }).ToList()
                 )
                .Build();

            // Actualización de ForeingKey
            foreach (var item in _order1.Items) { item.Order = _order1; }
        }

        [Test]
        public void Test_Map_Detail()
        {
            // Arrange
            var mapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
            var detailMap = mapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
                .WithDependencies<object>()
                .AddAction((ctx, item) => ctx.Master.AddItem(item))
                .DeleteAction((ctx, item) => ctx.Master.RemoveItem(item))
                .CreateFunc((ctx) => new ItemOrder())
                .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
                .Build();


           

            // Act
            detailMap.Map(_orderDTO1, _order1, null, (itemDTO, item) => 
            {
                // Mapeo de propiedades internas
                item.ProductId = itemDTO.ProductId;
                item.Quantity = itemDTO.Quantity;
            });

            // Assert
            Assert.That(_order1.Items.Count, Is.EqualTo(3));

            var items = _order1.Items.ToList();

            Assert.That(items[0].Order, Is.EqualTo(_order1));
            Assert.That(items[0].ProductId, Is.EqualTo("Item 1"));
            Assert.That(items[0].Quantity, Is.EqualTo(2m));

            Assert.That(items[1].Order, Is.EqualTo(_order1));
            Assert.That(items[1].ProductId, Is.EqualTo("Item 2"));
            Assert.That(items[1].Quantity, Is.EqualTo(4m));

            Assert.That(items[2].Order, Is.EqualTo(_order1));
            Assert.That(items[2].ProductId, Is.EqualTo("Item 0"));
            Assert.That(items[2].Quantity, Is.EqualTo(0m));


        }

        [Test]
        public void Test_Map_Detail_With_Repository()
        {
            // Arrange

            var mapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
            var detailMap = mapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
                .WithDependencies<IItemRepository>()
                .AddAction((ctx, item) => ctx.Dependencies.Insert(ctx.Master, item))
                .DeleteAction((ctx, item) => ctx.Dependencies.Delete(item))
                .CreateFunc((ctx) => new ItemOrder())
                .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
                .Build();

            Mock<IItemRepository> _itemRepositoryMock = new Mock<IItemRepository>();
            _itemRepositoryMock.Setup(x => x.Insert(_order1, It.IsAny<ItemOrder>()))
                .Callback((Order o, ItemOrder i) =>
                {
                    o.AddItem(i);
                });

            _itemRepositoryMock.Setup(x => x.Delete(It.IsAny<ItemOrder>()))
                .Callback((ItemOrder i) =>
                {
                    i.Order.RemoveItem(i);
                });


            // Act
            detailMap.Map(_orderDTO1, _order1, _itemRepositoryMock.Object, (itemDTO, item) =>
            {
                // Mapeo de propiedades internas
                item.ProductId = itemDTO.ProductId;
                item.Quantity = itemDTO.Quantity;
            });

            // Assert
            _itemRepositoryMock.Verify(x => x.Insert(_order1, It.IsAny<ItemOrder>()), Times.Once());
            _itemRepositoryMock.Verify(x => x.Delete(It.IsAny<ItemOrder>()), Times.Exactly(2));


            Assert.That(_order1.Items.Count, Is.EqualTo(3));

            var items = _order1.Items.ToList();

            Assert.That(items[0].Order, Is.EqualTo(_order1));
            Assert.That(items[0].ProductId, Is.EqualTo("Item 1"));
            Assert.That(items[0].Quantity, Is.EqualTo(2m));

            Assert.That(items[1].Order, Is.EqualTo(_order1));
            Assert.That(items[1].ProductId, Is.EqualTo("Item 2"));
            Assert.That(items[1].Quantity, Is.EqualTo(4m));

            Assert.That(items[2].Order, Is.EqualTo(_order1));
            Assert.That(items[2].ProductId, Is.EqualTo("Item 0"));
            Assert.That(items[2].Quantity, Is.EqualTo(0m));


        }

        [Test]
        public void Test_Map_Detail_RequiresDependency_ThrowsArgumentException()
        {
            // Arrange

            var mapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
            var detailMap = mapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
                .WithDependencies<IItemRepository>(true)
                .AddAction((ctx, item) => ctx.Dependencies.Insert(ctx.Master, item))
                .DeleteAction((ctx, item) => ctx.Dependencies.Delete(item))
                .CreateFunc((ctx) => new ItemOrder())
                .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
                .Build();

            // Act
            
            Assert.That(() =>
            {
                detailMap.Map(_orderDTO1, _order1, null);
            }, Throws.ArgumentNullException);

        }

        [Test]
        public void Test_Map_Detail_No_RequiresDependency()
        {
            // Arrange

            var mapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
            var detailMap = mapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
                .WithDependencies<IItemRepository>(false)
                .AddAction((ctx, item) => { })
                .DeleteAction((ctx, item) => { })
                .CreateFunc((ctx) => new ItemOrder())
                .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
                .Build();

            // Act

            detailMap.Map(_orderDTO1, _order1, null);
        }

        [Test]
        public void Test_Map_Detail_No_DetailCollection_ThrowsException()
        {
            // Arrange

            var mapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
            var detailMap = mapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
                .WithDependencies<IItemRepository>(false)
                .AddAction((ctx, item) => ctx.Dependencies.Insert(ctx.Master, item))
                .DeleteAction((ctx, item) => ctx.Dependencies.Delete(item))
                .CreateFunc((ctx) => new ItemOrder())
                .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
                .Build();

            _order1.Items = null;

            // Act

            Assert.That(() =>
            {
                detailMap.Map(_orderDTO1, _order1, null);
            }, Throws.Exception);

        }


        [Test]
        public void Test_Map_Detail_No_DetailDTOCollection_ThrowsException()
        {
            // Arrange

            var mapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
            var detailMap = mapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
                .WithDependencies<IItemRepository>(false)
                .AddAction((ctx, item) => ctx.Dependencies.Insert(ctx.Master, item))
                .DeleteAction((ctx, item) => ctx.Dependencies.Delete(item))
                .CreateFunc((ctx) => new ItemOrder())
                .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
                .Build();

            _orderDTO1.Items = null;

            // Act

            Assert.That(() =>
            {
                detailMap.Map(_orderDTO1, _order1, null);
            }, Throws.Exception);

        }

        [Test]
        public void Test_Map_Detail_No_Master_ThrowsArgumentNullException()
        {
            // Arrange

            var mapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
            var detailMap = mapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
                .WithDependencies<IItemRepository>(false)
                .AddAction((ctx, item) => ctx.Dependencies.Insert(ctx.Master, item))
                .DeleteAction((ctx, item) => ctx.Dependencies.Delete(item))
                .CreateFunc((ctx) => new ItemOrder())
                .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
                .Build();
            // Act

            Assert.That(() =>
            {
                detailMap.Map(_orderDTO1, null, null);
            }, Throws.ArgumentNullException);

        }

        [Test]
        public void Test_Map_Detail_No_Master_DTO_ThrowsArgumentNullException()
        {
            // Arrange

            var mapperBuilder = DetailMapperBuilder.Create<OrderDTO, Order>();
            var detailMap = mapperBuilder.Detail((dto) => dto.Items, (e) => e.Items)
                .WithDependencies<IItemRepository>(false)
                .AddAction((ctx, item) => ctx.Dependencies.Insert(ctx.Master, item))
                .DeleteAction((ctx, item) => ctx.Dependencies.Delete(item))
                .CreateFunc((ctx) => new ItemOrder())
                .EqualsFunc((itemDTO, item) => itemDTO.Id == item.Id)
                .Build();
            // Act

            Assert.That(() =>
            {
                detailMap.Map(null, _order1, null);
            }, Throws.ArgumentNullException);

        }

    }
}
