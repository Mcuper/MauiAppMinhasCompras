using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    // Esta classe é responsável por gerenciar todas as operações do banco de dados (CRUD)
    public class SQLiteDatabaseHelper
    {
        // Objeto de conexão assíncrona (Async) com o SQLite
        readonly SQLiteAsyncConnection _conn;

        // O construtor recebe o caminho (path) onde o arquivo do banco de dados será salvo
        public SQLiteDatabaseHelper(string path)
        {
            // Inicializa a conexão com o banco de dados no caminho especificado
            _conn = new SQLiteAsyncConnection(path);

            // Cria a tabela 'Produto' de forma assíncrona se ela ainda não existir.
            // O .Wait() força o programa a esperar a criação antes de prosseguir.
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Método para inserir um novo produto no banco de dados
        public Task<int> Insert(Produto p)
        {
            // Retorna a tarefa de inserção (o int retornado é o número de linhas afetadas)
            return _conn.InsertAsync(p);
        }

        // Método para atualizar os dados de um produto existente
        public Task<List<Produto>> Update(Produto p)
        {
            // Comando SQL manual para atualizar campos específicos baseado no ID
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            // Executa a query passando os novos valores do objeto 'p'
            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }

        // Método para excluir um produto do banco de dados usando o ID
        public Task<int> Delete(int id)
        {
            // Busca na tabela de Produtos e deleta o item que tiver o ID correspondente
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // Método para buscar todos os produtos cadastrados
        public Task<List<Produto>> GetAll()
        {
            // Retorna uma lista com todos os registros da tabela Produto
            return _conn.Table<Produto>().ToListAsync();
        }

        // Método para pesquisar produtos por uma descrição (busca parcial)
        public Task<List<Produto>> Search(string q)
        {
            // SQL que busca produtos onde a descrição contém o texto digitado (LIKE)
            // Nota: Verifique se não falta o "FROM" no seu SQL original: "SELECT * FROM Produto..."
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }
    }
}