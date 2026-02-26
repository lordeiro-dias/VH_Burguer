using VH_Burguer.Applications.Autenticacao;
using VH_Burguer.DTOs.AutenticacaoDto;
using VHBurguer.Domains;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VH_Burguer.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJWT _tokenJwt;

        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJWT tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        // compara a hash SHA256
        private static bool VerificarSenha(string senhaDigitada, byte[] senhaHashBanco)
        {
            using var sha = System.Security.Cryptography.SHA1.Create();
            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigitada));

            return hashDigitado.SequenceEqual(senhaHashBanco);
        }

        public TokenDto Login(LoginDto loginDto)
        {
            Usuario usuario = _repository.ObterPorEmail(loginDto.Email);

            if(usuario == null)
            {
                throw new DomainException("E-mail ou senha inválidos");
            }

            // comparar a senha digitada com a senha armazenada
            if(VerificarSenha(loginDto.Senha, usuario.Senha))
            {
                throw new DomainException("E-mail ou senha inválidos");
            }

            if(usuario.StatusUsuario == false)
            {
                throw new DomainException("Usuário está inativo. Não é possível fazer login.");
            }

            // gerando o token
            var token = _tokenJwt.GerarToken(usuario);

            TokenDto novoToken = new TokenDto() { Token = token };

            return novoToken;
        }
    }
}
