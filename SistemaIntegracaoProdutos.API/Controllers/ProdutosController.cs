using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaIntegracaoProdutos.API.Data;
using SistemaIntegracaoProdutos.API.DTOs;
using Shared;

namespace SistemaIntegracaoProdutos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os produtos cadastrados no banco de dados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            try
            {
                return await _context.Produtos.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao recuperar produtos: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca um produto específico pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });

            return Ok(produto);
        }

        /// <summary>
        /// Cadastra um novo produto.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Produto>> PostProduto(ProdutoDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");

            try
            {
                var produto = new Produto
                {
                    Nome = dto.Nome,
                    Preco = dto.Preco,
                    QuantidadeEstoque = dto.QuantidadeEstoque,
                    DataCriacao = DateTime.Now
                };

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                // Retorna 201 Created e o caminho para acessar o novo recurso
                return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Não foi possível salvar o produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza os dados de um produto existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProduto(int id, ProdutoDTO dto)
        {
            var produtoExistente = await _context.Produtos.FindAsync(id);

            if (produtoExistente == null)
                return NotFound(new { mensagem = "Produto não encontrado para atualização." });

            try
            {
                produtoExistente.Nome = dto.Nome;
                produtoExistente.Preco = dto.Preco;
                produtoExistente.QuantidadeEstoque = dto.QuantidadeEstoque;

                _context.Entry(produtoExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Erro de concorrência ao atualizar o banco.");
            }
        }

        /// <summary>
        /// Remove um produto do banco de dados.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound(new { mensagem = "Produto não encontrado para exclusão." });

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}