using System.Net.Http.Json;
using SistemaSimulator.DTOs;     
using SistemaSimulator.Services; 

const string apiUrl = "https://localhost:7009/api/produtos";
using HttpClient client = new HttpClient();

Console.WriteLine("SIMULADOR DE ESTOQUE ATIVO");

string[] nomesExemplo = { "Teclado", "Mouse", "Monitor", "Cabo HDMI", "Headset", "Gabinete" };
Random random = new Random();

while (true)
{
    var novoProdutoDto = new ProdutoDTO
    {
        Nome = $"{nomesExemplo[random.Next(nomesExemplo.Length)]} {random.Next(100, 999)}",
        Preco = (decimal)(random.NextDouble() * 500 + 10),
        QuantidadeEstoque = random.Next(1, 50),
        DataCriacao = DateTime.Now 
    };

    try
    {
        var response = await client.PostAsJsonAsync(apiUrl, novoProdutoDto);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[ {DateTime.Now:HH:mm:ss} ] Enviado para API: {novoProdutoDto.Nome} | Preço: {novoProdutoDto.Preco:C2}");

            LogService.SalvarLogLocal(novoProdutoDto);
        }
        else
        {
            Console.WriteLine($"[ {DateTime.Now:HH:mm:ss} ] Erro ao enviar para a API.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Falha de conexão: {ex.Message}");
    }
    await Task.Delay(2000);
}