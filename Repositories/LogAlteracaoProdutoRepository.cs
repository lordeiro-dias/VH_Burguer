using VH_Burguer.Interfaces;
using VHBurguer.Contexts;
using VHBurguer.Domains;

namespace VH_Burguer.Repositories
{
    public class LogAlteracaoProdutoRepository : ILogAlteracaoProdutoRepository
    {
        private readonly VH_BurguerContext _context;

        public LogAlteracaoProdutoRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Log_AlteracaoProduto> Listar()
        {
            // OrderByDescending -> ordenar por dataS
            List<Log_AlteracaoProduto> log = _context.Log_AlteracaoProduto.OrderByDescending(l => l.DataAlteracao).ToList();
            return log;
        }

        public List<Log_AlteracaoProduto> ListarPorProduto(int produtoId)
        {
            List<Log_AlteracaoProduto> AlteracoesProduto = _context.Log_AlteracaoProduto
                .Where(log => log.ProdutoID == produtoId)
                .OrderByDescending(log => log.DataAlteracao).ToList();

            return AlteracoesProduto;
        }
    }
}
