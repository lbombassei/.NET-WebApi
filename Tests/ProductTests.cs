using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Application.Context;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Microsoft.EntityFrameworkCore.Query;

namespace Tests
{
    [TestClass]
    public class ProductTests
    {

        private List<Categorias> _testCategorias;
        private List<Produtos> _testProducts;

        public ProductTests()
        {
            _testCategorias = new List<Categorias>
            {
                new Categorias { Id = 1, Nome = "Teste" }
            };

                    _testProducts = new List<Produtos>
            {
                new Produtos { Id = 1, CategoriaId = 1, Nome= "Produto 1", PrecoUnitario= 1000, QuantidadeEstoque= 10, Status= true, Categoria = _testCategorias[0]},
                new Produtos { Id = 2, CategoriaId = 1, Nome = "Produto 2", PrecoUnitario = 20.0, QuantidadeEstoque= 10, Status= true, Categoria = _testCategorias[0] },
                new Produtos { Id = 3, CategoriaId = 1, Nome = "Produto 3", PrecoUnitario = 30.0, QuantidadeEstoque= 10, Status= true , Categoria = _testCategorias[0]}
            };
        }

        [TestMethod]
        public async Task TestGetAll()
        {
            var options = new DbContextOptionsBuilder<ProvaContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new ProvaContext(options);
            context.Produtos.AddRange(_testProducts);
            await context.SaveChangesAsync();
            var service = new ProdutoService(context);

            // Act
            var result = await service.GetAll("Produto 1");

            // Assert
            CollectionAssert.AreEquivalent(_testProducts.Where(p => p.Nome.Contains("Produto 1")).ToList(), result.ToList());
        }




        [TestMethod]
        public void TestGetById()
        {
            // Arrange
            var mockContext = new Mock<IProvaContext>();
            mockContext.Setup(c => c.Produtos).ReturnsDbSet(_testProducts);

            var service = new ProdutoService(mockContext.Object);

            // Act
            var result = service.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task TestAdd()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProvaContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new ProvaContext(options);
            context.Produtos.AddRange(_testProducts);
            await context.SaveChangesAsync();

            var service = new ProdutoService(context);

            var produto = new Produtos { Nome = "Produto de teste", CategoriaId = 1, PrecoUnitario = 10.99, QuantidadeEstoque = 50 };

            // Act
            var result = await service.Add(produto);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(produto.Nome, result.Nome);
            Assert.AreEqual(produto.PrecoUnitario, result.PrecoUnitario);

            var found = await context.Produtos.FindAsync(result.Id);
            Assert.IsNotNull(found);
        }





        [TestMethod]
        public async Task TestUpdate()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProvaContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new ProvaContext(options);
            context.Produtos.AddRange(_testProducts);
            await context.SaveChangesAsync();

            var service = new ProdutoService(context);

            // busca o objeto no banco de dados antes de atualizá-lo
            var existingProduct = await context.Produtos.FindAsync((long)1);

            if (existingProduct != null)
            {
                // Atualiza o objeto existente
                var updatedProduct = new Produtos { Id = 1, Nome = "Produto Atualizado", PrecoUnitario = 100.09, QuantidadeEstoque = 500 };
                await service.Update(updatedProduct);

                // Assert
                var result = await context.Produtos.FindAsync(1L);
                Assert.IsNotNull(result);
                Assert.AreEqual(updatedProduct.Nome, result.Nome);
                Assert.AreEqual(updatedProduct.PrecoUnitario, result.PrecoUnitario);
            }
            else
            {
                Assert.Fail("O objeto que você está tentando atualizar não existe no banco de dados.");
            }
        }

        [TestMethod]
        public async Task TestDelete()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProvaContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ProvaContext(options))
            {
                context.Produtos.AddRange(_testProducts);
                await context.SaveChangesAsync();

                var service = new ProdutoService(context);

                // busca o objeto no banco de dados antes de excluí-lo
                var existingProduct = await context.Produtos.FindAsync((long)1);

                if (existingProduct != null)
                {
                    // Act
                    var result = await service.Delete(1);

                    // Assert
                    Assert.IsNull(result);

                    var deletedProduct = await context.Produtos.FindAsync(1L);
                    Assert.IsNull(deletedProduct);
                }
                else
                {
                    Assert.Fail("O objeto que você está tentando excluir não existe no banco de dados.");
                }
            }
        }






    }
}