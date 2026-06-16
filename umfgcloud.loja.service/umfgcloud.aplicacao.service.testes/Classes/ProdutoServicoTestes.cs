using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umfgcloud.loja.dominio.service.DTO;

namespace umfgcloud.aplicacao.service.testes.Classes
{
    [TestClass]
    public sealed class ProdutoServicoTestes : AbstractServicoTestes
    {
        private const string C_OWNER = "Juliano Maciel";
        private const string C_CATEGORY = "produto";
        private const decimal C_VALOR_NEGATIVO = -89.90m;
        private const decimal C_VALOR_ZERO = 0m;

        #region AdicionarAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaValorVendaNegativo()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = "123456789",
                    ValorCompra = 39.90m,
                    ValorVenda = -89.90m,
                };

                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaValorCompraZero()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = "123456789",
                    ValorCompra = C_VALOR_ZERO,
                    ValorVenda = 89.90m,
                };

                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaDescricaoNula()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = null!,
                    EAN = "123456789",
                    ValorCompra = 39.90m,
                    ValorVenda = 89.90m,
                };

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaEANNulo()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = null!,
                    ValorCompra = 39.90m,
                    ValorVenda = 89.90m,
                };

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion AdicionarAsync

        #region ObterTodosAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterTodosAsync_Sucesso()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO 1",
                    EAN = "111111111",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO 2",
                    EAN = "222222222",
                    ValorCompra = 30.00m,
                    ValorVenda = 60.00m,
                });

                var produtos = await servico.ObterTodosAsync();

                Assert.IsNotNull(produtos);
                Assert.AreEqual(2, produtos.Count());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterTodosAsync_ListaVazia()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                var produtos = await servico.ObterTodosAsync();

                Assert.IsNotNull(produtos);
                Assert.AreEqual(0, produtos.Count());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion ObterTodosAsync

        #region ObterPorIdAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterPorIdAsync_Sucesso()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO BUSCA",
                    EAN = "987654321",
                    ValorCompra = 15.00m,
                    ValorVenda = 25.00m,
                });

                var id = (await servico.ObterTodosAsync()).First().Id;

                var produto = await servico.ObterPorIdAsync(id);

                Assert.IsNotNull(produto);
                Assert.AreEqual(id, produto.Id);
                Assert.AreEqual("PRODUTO BUSCA", produto.Descricao);
                Assert.AreEqual("987654321", produto.EAN);
                Assert.AreEqual(15.00m, produto.ValorCompra);
                Assert.AreEqual(25.00m, produto.ValorVenda);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterPorIdAsync_FalhaIdInexistente()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await Assert.ThrowsExceptionAsync<ApplicationException>(() => servico.ObterPorIdAsync(Guid.NewGuid()));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion ObterPorIdAsync

        #region AtualizarAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_Sucesso()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO ORIGINAL",
                    EAN = "111111111",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });

                var id = (await servico.ObterTodosAsync()).First().Id;

                await servico.AtualizarAsync(new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = id,
                    Descricao = "PRODUTO ATUALIZADO",
                    EAN = "999999999",
                    ValorCompra = 50.00m,
                    ValorVenda = 100.00m,
                });

                var produtoAtualizado = await servico.ObterPorIdAsync(id);

                Assert.IsNotNull(produtoAtualizado);
                Assert.AreEqual(id, produtoAtualizado.Id);
                Assert.AreEqual("PRODUTO ATUALIZADO", produtoAtualizado.Descricao);
                Assert.AreEqual("999999999", produtoAtualizado.EAN);
                Assert.AreEqual(50.00m, produtoAtualizado.ValorCompra);
                Assert.AreEqual(100.00m, produtoAtualizado.ValorVenda);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_FalhaIdInexistente()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                var dto = new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = Guid.NewGuid(),
                    Descricao = "PRODUTO INEXISTENTE",
                    EAN = "000000000",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                };

                await Assert.ThrowsExceptionAsync<ApplicationException>(() => servico.AtualizarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_FalhaValorCompraNegativo()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO",
                    EAN = "111111111",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });

                var id = (await servico.ObterTodosAsync()).First().Id;

                var dto = new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = id,
                    Descricao = "PRODUTO",
                    EAN = "111111111",
                    ValorCompra = C_VALOR_NEGATIVO,
                    ValorVenda = 20.00m,
                };

                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AtualizarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_FalhaValorVendaNegativo()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO",
                    EAN = "111111111",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });

                var id = (await servico.ObterTodosAsync()).First().Id;

                var dto = new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = id,
                    Descricao = "PRODUTO",
                    EAN = "111111111",
                    ValorCompra = 10.00m,
                    ValorVenda = C_VALOR_NEGATIVO,
                };

                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AtualizarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion AtualizarAsync

        #region RemoverAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_RemoverAsync_Sucesso()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO PARA REMOVER",
                    EAN = "555555555",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });

                var id = (await servico.ObterTodosAsync()).First().Id;

                await servico.RemoverAsync(id);

                var produtos = await servico.ObterTodosAsync();

                Assert.AreEqual(0, produtos.Count());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_RemoverAsync_FalhaIdInexistente()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await Assert.ThrowsExceptionAsync<ApplicationException>(() => servico.RemoverAsync(Guid.NewGuid()));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion RemoverAsync

        #region Instanciar

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public void ProdutoServico_Instanciar_Falha()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                Assert.ThrowsException<InvalidDataException>(() => GetProdutoServicoInvalidJWT(context));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion Instanciar
    }
}
