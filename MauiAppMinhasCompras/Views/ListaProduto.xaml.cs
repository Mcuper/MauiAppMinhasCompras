using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }


    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Navega para a página de relatório detalhado
        await Navigation.PushAsync(new Views.RelatorioPage());
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecinado = sender as MenuItem; // 1. Recupera o MenuItem que foi clicado
            Produto p = selecinado.BindingContext as Produto; // 2. Recupera o objeto Produto vinculado a essa linha

            if (p == null) return;

            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "N o");

            if (confirm)
            {
                await App.Db.Delete(p.Id);  // 3. Remove do Banco de Dados
                lista.Remove(p);  // 4. Remove da lista que aparece na tela (ObservableCollection)
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            // 1. Recupera o produto selecionado
            Produto p = e.SelectedItem as Produto;

            if (p != null)
            {
                // 2. Navega para a página de Edição
                await Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = p
                });

                // 3. LIMPEZA (Importante para Android): 
                // Desmarca o item para que você possa clicar nele de novo depois
                ((ListView)sender).SelectedItem = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", "Erro ao abrir edição: " + ex.Message, "OK");
        }
    }


    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");

        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }


    }

    private async void pck_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var picker = (Picker)sender;
            string selecionada = (string)picker.SelectedItem;

            lst_produtos.IsRefreshing = true;
            lista.Clear();

            // Busca todos os produtos do banco
            List<Produto> tmp = await App.Db.GetAll();

            // APLICAÇÃO DO LINQ PARA FILTRAGEM
            if (selecionada != "Todos")
            {
                tmp = tmp.Where(p => p.Categoria == selecionada).ToList();
            }

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
}