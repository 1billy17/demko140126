using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using demko140126.Models;
using Microsoft.EntityFrameworkCore;

namespace demko140126;

public partial class MainWindow : Window
{
    ObservableCollection<Product> products = new ObservableCollection<Product>();
    List<Product> dataSourceProducts;
    
    private const int ItemsPerPage = 20;
    private int currentPage = 1;
    
    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        using var ctx = new Demko140126Context();

        dataSourceProducts = ctx.Products.Include(p => p.ProductType).Include(p => p.ProductMaterials).ThenInclude(pm => pm.Material).ToList();

        TypeProductComboBox.ItemsSource = new List<string> { "Все" }.Concat(ctx.ProductTypes.Select(g => g.Title)).ToList();
        
        DisplayServices();
    }

    public void DisplayServices()
    {
        var temp = dataSourceProducts;
        products.Clear();
        
        switch (TitleComboBox.SelectedIndex)
        {
            case 0: break;
            case 1: temp = temp.OrderBy(p => p.Title).ToList(); break;
            case 2: temp = temp.OrderByDescending(p => p.Title).ToList(); break;
        }
        
        switch (WorkshopNumberComboBox.SelectedIndex)
        {
            case 0: break;
            case 1: temp = temp.OrderBy(p => p.ProductionWorkshopNumber).ToList(); break;
            case 2: temp = temp.OrderByDescending(p => p.ProductionWorkshopNumber).ToList(); break;
        }
        
        switch (MinCostComboBox.SelectedIndex)
        {
            case 0: break;
            case 1: temp = temp.OrderBy(p => p.MinCostForAgent).ToList(); break;
            case 2: temp = temp.OrderByDescending(p => p.MinCostForAgent).ToList(); break;
        }
        
        if (TypeProductComboBox.SelectedItem is string selectedType && selectedType != "Все")
        {
            temp = temp.Where(p => p.ProductType.Title == selectedType).ToList();
        }
        
        if (!string.IsNullOrEmpty(SearchTextBox.Text))
        {
            var search = SearchTextBox.Text;
            temp = temp.Where(p => IsContains(p.Title, search)).ToList();
        }
        
        foreach (var item in temp)
        {
            products.Add(item);
        }
        
        currentPage = 0; 
        UpdateListBox();
    }
    
    public bool IsContains(string title, string search)
    {
        string message = (title).ToLower();
        search = search.ToLower();
        return message.Contains(search);
    }
    
    public void UpdateListBox()
    {
        var pagedProducts = products.Skip(currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();
        ProductsListBox.ItemsSource = pagedProducts;
        
        CountProductsTextBlock.Text = $"{currentPage + 1} / {Math.Ceiling((double)products.Count / ItemsPerPage)}";
        
        BackButton.IsEnabled = currentPage > 0;
        NextButton.IsEnabled = (currentPage + 1) * ItemsPerPage < products.Count;
    }
    
    public void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateListBox();
        }
    }

    public void Next_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((currentPage + 1) * ItemsPerPage < products.Count)
        {
            currentPage++;
            UpdateListBox();
        }
    }
    
    public void AddProductButton_OnClick(object? sender, RoutedEventArgs e)
    {
    }
    
    public void TitleProductComboBox_OnSelectionChanged(object? sender, RoutedEventArgs e)
    {
        DisplayServices();
    }
    
    public void WorkshopNumberComboBox_OnSelectionChanged(object? sender, RoutedEventArgs e)
    {
        DisplayServices();
    }
    
    public void MinCostComboBox_OnSelectionChanged(object? sender, RoutedEventArgs e)
    {
        DisplayServices();
    }
    
    public void TypeProductComboBox_OnSelectionChanged(object? sender, RoutedEventArgs e)
    {
        DisplayServices();
    }
    
    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        DisplayServices();
    }
}