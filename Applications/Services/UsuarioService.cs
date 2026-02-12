using System.Security.Cryptography;
using System.Text;
using VH_Burguer.Domains;
using VH_Burguer.DTOs;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Applications.Services
{
    // service concentra o "como fazer" 
    public class UsuarioService
    {
        // _repository é o canal para acessar os dados.
        private readonly IUsuarioRepository _repository;

        // injeção de dependencias
        // implementamos o repositorio e o service só depende da interface
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        // Por que private?
        // pq o método não é regra de negócio e não faz sentido existir fora do UsuarioService

        private static LerUsuarioDto LerDto(Usuario usuario) // pega a entidade usuario e gera um DTO
        {
            LerUsuarioDto lerUsuario = new LerUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                StatusUsuario = usuario.StatusUsuario ?? true // se não tiver status no banco. deixa como true
            };

            return lerUsuario;
        }
        public List<LerUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();

            List<LerUsuarioDto> usuariosDto = usuarios
                .Select(usuarioBanco => LerDto(usuarioBanco)) // Select que percorre cada usuário e LerDto(usuario)
                .ToList(); // ToList() => Devolve uma lista de DTOs
            return usuariosDto;
        }

        private static void ValidarEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("Email Inválido");
            }
        }

        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha)) // garante que a senha não está vazia
            {
                throw new DomainException("Senha é Obrigatória");
            }

            using var sha256 = SHA256.Create(); // gera um hash SHA256 e devolve em byte[]
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerUsuarioDto ObterPorId(int id)
        {
            Usuario? usuario = _repository.ObterPorId(id);

            if(usuario == null)
            {
                throw new DomainException("Usuário não existe");
            }

            return LerDto(usuario); // se existe usuário, converte para DTO e devolve o usuário
        }

        public LerUsuarioDto ObterPorEmail(string email)
        {
            Usuario? usuario = _repository.ObterPorEmail(email);

            if (usuario == null)
            {
                throw new DomainException("Usuário não existe");
            }

            return LerDto(usuario); // se existe usuário, converte para DTO e devolve o usuário
        }
    }
}
