using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioPage : ContentPage
{
    public RelatorioPage()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await CarregarRelatorio();
    }

    private async Task CarregarRelatorio()
    {
        try
        {
            // Busca todos os produtos salvos no banco
            List<Produto> produtos = await App.Db.GetAll();

            // LINQ: Agrupa por categoria e calcula a soma de cada grupo
            var resumo = produtos.GroupBy(p => p.Categoria ?? "Sem Categoria")
                                 .Select(g => new
                                 {
                                     Categoria = g.Key,
                                     TotalGeral = g.Sum(p => p.Total)
                                 }).ToList();

            lst_relatorio.ItemsSource = resumo;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        // Remove a página atual da pilha e volta para a anterior (ListaProduto)
        await Navigation.PopAsync();
    }
}