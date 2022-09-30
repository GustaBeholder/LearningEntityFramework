using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Domain;
using Pedidos.ValueObjects;

namespace Pedidos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultaDados();
            //CadastrarPedidos();
            //ConsultaPedidoCarregamentoAdiantado();
            //AtualizarDados();
            //DeletaDados();
        }

        private static void ConsultaPedidoCarregamentoAdiantado()
        {
            using ApplicationContext db = new ();
            List<Pedido> pedidos = db.Pedido
                .Where(p => p.Id == 1)
                .Include(p => p.Cliente)
                .Include(p => p.Items)
                    .ThenInclude(p => p.Produto)     
                .ToList();

            Console.WriteLine(pedidos.Count);
        }
        private static void InserirDados()
        {
            Produto produto = new()
            {
                Descricao = "Produto Teste",
                CodigoBarras = "122254759996",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true

            };

            using ApplicationContext db = new ();

            db.Set<Produto>().Add(produto);

            int registros = db.SaveChanges();

            Console.WriteLine(registros);
        }

        private static void InserirDadosEmMassa()
        {
            Produto produto = new()
            {
                Descricao = "Produto Teste",
                CodigoBarras = "122254759996",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            Cliente cliente = new()
            {
                Nome = "Gustavo Soares",
                CEP = "14680000",
                Cidade = "Jardinópolis",
                Estado = "SP",
                Telefone = "993290359"
            };

            using ApplicationContext db = new();

            db.AddRange(produto, cliente);
            var resultado = db.SaveChanges();
            Console.WriteLine(resultado);
        }

        private static void ConsultaDados()
        {
            using var db = new Data.ApplicationContext();

            //var consultaPorSintaxe = (from c in db.Cliente where c.Id > 0 select c).ToList();

            var consultaPorMetodo = db.Cliente
                .AsNoTracking()
                .Where(c => c.Id > 0)
                .OrderBy(c => c.Id)
                .ToList();

            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine(cliente.Id);
                db.Cliente.Find(cliente.Id);    
            }
        }

        private static void CadastrarPedidos()
        {
            using ApplicationContext db = new ();

            Cliente cliente = db.Cliente.FirstOrDefault()!;
            Produto produto = db.Produto.FirstOrDefault()!;

            Pedido pedido = new ()
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                StatusPedido = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Items = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedido.Add(pedido);

            db.SaveChanges();
        }

        private static void AtualizarDados()
        {
            using ApplicationContext db = new ();

            //Cliente cliente = db.Cliente.Find(1);

            Cliente cliente = new ()
            {
                Id = 1
            };

            var clienteDesconectado = new 
            { 
                Nome = "Teste 3",
                Telefone = "9588855966"
            };
            
            db.Attach(cliente);
            db.Entry(cliente).CurrentValues
                .SetValues(clienteDesconectado);

            //db.Cliente.Update(cliente);
            db.SaveChanges();
        }

        private static void DeletaDados()
        {
            using ApplicationContext db = new ();

            Cliente cliente = db.Cliente
                .Where(p => p.Id == 1)
                .FirstOrDefault()!;

            db.Cliente.Remove(cliente);

            db.SaveChanges();


        }
    }
}
