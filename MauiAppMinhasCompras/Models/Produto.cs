using SQLite;

namespace MauiAppMinhasCompras.Models
{
    /// <summary>
    /// Representa a tabela 'Produto' no banco de dados local.
    /// </summary>
    public class Produto
    {
        // Define que esta propriedade é a Chave Primária da tabela.
        // O 'AutoIncrement' faz com que o SQLite gere o ID automaticamente (1, 2, 3...)
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Armazena a descrição ou nome do produto (ex: "Arroz 5kg")
        public string Descricao { get; set; }

        // Armazena a quantidade do item. 
        // Usamos 'double' para permitir valores fracionados (ex: 1.5 kg)
        public double Quantidade { get; set; }

        // Armazena o preço unitário do produto
        public double Preco { get; set; }

        /// <summary>
        /// Propriedade calculada que retorna o valor total do item.
        /// No SQLite, por padrão, propriedades apenas com 'get' não são criadas como colunas,
        /// o que é ideal, pois o cálculo é feito em tempo real.
        /// </summary>
        public double Total { get => Quantidade * Preco; }
    }
}