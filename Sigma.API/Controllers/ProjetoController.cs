using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sigma.Application.Dtos;
using Sigma.Application.Interfaces;
using Sigma.Domain.Dtos;
using Sigma.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Sigma.API.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de projetos e autenticação.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProjetoService _projetoService;

        public ProjetoController(IConfiguration configuration, IProjetoService projetoService)
        {
            _configuration = configuration;
            _projetoService = projetoService;
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="model">Dados do novo login.</param>
        /// <returns>Resultado da criação do login.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("criarLogin")]
        public async Task<IActionResult> InserirLogin([FromBody] LoginNovoDto model)
        {
            return new JsonResult(await _projetoService.InserirLogin(model));
        }

        /// <summary>
        /// Realiza login e retorna token JWT.
        /// </summary>
        /// <param name="login">Dados de login do usuário.</param>
        /// <returns>Token JWT válido para autenticação.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginNovoDto login)
        {
            var usuario = await _projetoService.ObterLoginPorUsuario(login.Usuario);            

            if (usuario == null || usuario.Senha != login.Senha)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, usuario.Login)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }



        /// <summary>
        /// Insere um novo projeto.
        /// </summary>
        /// <param name="model">Dados do projeto.</param>
        /// <returns>Projeto criado.</returns>
        [Authorize]
        [HttpPost]
        [Route("inserir")]
        public async Task<IActionResult> Inserir([FromBody] ProjetoNovoDto model)
        {
            var projetoCriado = await _projetoService.Inserir(model);
            return Ok(projetoCriado);
        }

        /// <summary>
        /// Lista todos os projetos.
        /// </summary>
        /// <returns>Lista de projetos.</returns>
        [Authorize]
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> Listar()
        {
            var resultado = await _projetoService.Listar();
            return Ok(resultado);
        }

        /// <summary>
        /// Exclui um projeto pelo ID.
        /// </summary>
        /// <param name="id">ID do projeto.</param>
        /// <returns>Resposta sem conteúdo ou erro.</returns>
        [Authorize]
        [HttpDelete]
        [Route("excluir/{id}")]
        public async Task<IActionResult> Excluir(long id)
        {
            try
            {
                await _projetoService.Excluir(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Altera dados de um projeto existente.
        /// </summary>
        /// <param name="id">ID do projeto.</param>
        /// <param name="dto">Dados atualizados do projeto.</param>
        /// <returns>Resposta sem conteúdo.</returns>
        [Authorize]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Alterar(long id, [FromBody] ProjetoEditarDto dto)
        {
            await _projetoService.Alterar(id, dto);
            return NoContent();
        }

        /// <summary>
        /// Consulta projetos por filtros opcionais de nome e status.
        /// </summary>
        /// <param name="nome">Nome para filtro (opcional).</param>
        /// <param name="status">Status para filtro (opcional).</param>
        /// <returns>Lista de projetos filtrados.</returns>
        [Authorize]
        [HttpGet("filtro")]
        public async Task<IActionResult> ConsultarPorFiltro([FromQuery] string? nome, [FromQuery] StatusProjeto? status)
        {
            var projetos = await _projetoService.ConsultarPorFiltro(nome, status);
            return Ok(projetos);
        }


    }
}
