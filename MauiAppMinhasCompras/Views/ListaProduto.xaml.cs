using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    // A ObservableCollection é uma lista especial que avisa a Interface (XAML) 
    // sempre que um item é adicionado, removido ou a lista é limpa.
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        // Vincula a lista criada acima ao controle 'lst_produtos' do XAML
        lst_produtos.ItemsSource = lista;
    }

    // O método OnAppearing é executado toda vez que a tela aparece para o usuário
    protected async override void OnAppearing()
    {
        // Busca todos os produtos salvos no banco de dados de forma assíncrona
        List<Produto> tmp = await App.Db.GetAll();

        // Limpa a lista atual para evitar duplicados ao recarregar a tela
        lista.Clear();

        // Adiciona cada produto retornado do banco na nossa lista da interface
        tmp.ForEach(i => lista.Add(i));
    }

    // Evento do botão (ícone +) para navegar até a tela de cadastro de novo produto
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Abre a tela NovoProduto empilhando-a na navegação
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            // Exibe um alerta caso ocorra algum erro inesperado
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Este é o evento de Busca Dinâmica que você perguntou anteriormente!
    // Ele dispara toda vez que o usuário digita ou apaga uma letra no SearchBar
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue; // Obtém o texto digitado

        lista.Clear(); // Limpa a lista exibida para mostrar os novos resultados

        // Chama o método Search do banco de dados passando o termo digitado
        List<Produto> tmp = await App.Db.Search(q);

        // Preenche a lista da interface com os produtos encontrados no filtro
        tmp.ForEach(i => lista.Add(i));
    }

    // Evento para calcular e exibir o valor total de todos os itens da lista
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Usa o LINQ (.Sum) para somar a propriedade 'Total' de cada objeto Produto
        double soma = lista.Sum(i => i.Total);

        // Formata o valor como moeda local (Ex: R$ 100,00) usando o especificador :C
        string msg = $"O total é {soma:C}";

        // Exibe o resultado em uma caixa de diálogo
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    // Evento para o menu de contexto (ex: deslizar para excluir) - A implementar
    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        // Espaço reservado para lógica de edição ou exclusão
    }
}