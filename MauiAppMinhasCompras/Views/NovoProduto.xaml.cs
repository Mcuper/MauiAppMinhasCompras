using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    // Método disparado ao clicar no botão de salvar (ToolbarItem)
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Instancia um novo objeto Produto capturando os valores digitados na tela
            Produto p = new Produto
            {
                // Pega o texto digitado no campo de descrição
                Descricao = txt_descricao.Text,

                // Converte o texto do campo quantidade para um número do tipo double
                Quantidade = Convert.ToDouble(txt_quantidade.Text),

                // Converte o texto do campo preço para um número do tipo double
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Chama o método Insert da nossa classe auxiliar de banco de dados (SQLite)
            // O 'await' é usado porque a operação de escrita no banco é assíncrona
            await App.Db.Insert(p);

            // Exibe uma mensagem de confirmação para o usuário
            await DisplayAlert("Sucesso!", "Registro Inserido", "OK");

            // Opcional: Você pode adicionar um Navigation.PopAsync() aqui para 
            // voltar automaticamente para a lista após salvar.
        }
        catch (Exception ex)
        {
            // Caso ocorra algum erro (como digitar letras em campos de números), 
            // o erro é capturado e exibido em um alerta
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}