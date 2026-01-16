using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using demko140126.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace demko140126;

public partial class EditProductWindow : Window
{
    private Product product;
    private List<ProductType> productTypes;
    
    public string SelectedImagePath = string.Empty;
    
    public EditProductWindow(Product productInput)
    {
        using var ctx = new Demko140126Context();
        InitializeComponent();
        product = productInput;
        
        productTypes = ctx.ProductTypes.ToList();
        ProductTypeComboBox.ItemsSource = productTypes.Select(t => t.Title).ToList();

        Otrisovka();
    }
    
    private void Otrisovka()
    {
        TitleTextBox.Text = product.Title;
        ArticleNumberTextBox.Text = product.ArticleNumber;
        WorkshopNumberTextBox.Text = product.ProductionWorkshopNumber?.ToString();
        PersonCountTextBox.Text = product.ProductionPersonCount?.ToString();
        MinCostTextBox.Text = product.MinCostForAgent.ToString();

        if (product.ProductTypeId != null)
        {
            var selectedType = productTypes
                .FirstOrDefault(t => t.Id == product.ProductTypeId);

            if (selectedType != null)
                ProductTypeComboBox.SelectedItem = selectedType.Title;
        }
        
        if (!string.IsNullOrEmpty(product.Image))
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory!, product.Image);

            if (File.Exists(fullPath))
                ProductImage.Source = new Bitmap(fullPath);
        }
    }
    
    private async void SaveProduct_OnClick(object? sender, RoutedEventArgs e)
    {
        using var ctx = new Demko140126Context();

        if (string.IsNullOrWhiteSpace(TitleTextBox.Text)) return;
        if (string.IsNullOrWhiteSpace(ArticleNumberTextBox.Text)) return;
        if (!int.TryParse(WorkshopNumberTextBox.Text, out int workshop)) return;
        if (!int.TryParse(PersonCountTextBox.Text, out int persons)) return;
        if (!decimal.TryParse(MinCostTextBox.Text, out decimal minCost)) return;

        var dbProduct = await ctx.Products.FirstAsync(p => p.Id == product.Id);

        dbProduct.Title = TitleTextBox.Text.Trim();
        dbProduct.ArticleNumber = ArticleNumberTextBox.Text.Trim();
        dbProduct.ProductionWorkshopNumber = workshop;
        dbProduct.ProductionPersonCount = persons;
        dbProduct.MinCostForAgent = minCost;
        if (!string.IsNullOrEmpty(SelectedImagePath))
            dbProduct.Image = SelectedImagePath;

        if (ProductTypeComboBox.SelectedItem is string selectedTitle)
        {
            var type = await ctx.ProductTypes.FirstAsync(t => t.Title == selectedTitle);
            dbProduct.ProductTypeId = type.Id;
        }

        await ctx.SaveChangesAsync();
        Close(dbProduct);
    }
    
    private async void SelectImageButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var bitmap = await SelectAndSaveImage();
        if (bitmap != null)
        {
            ProductImage.Source = bitmap;
        }
    }
    
    private async Task<Bitmap?> SelectAndSaveImage()
    {
        var showDialog = StorageProvider.OpenFilePickerAsync(
            options: new Avalonia.Platform.Storage.FilePickerOpenOptions()
            {
                Title = "Select an image",
                FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
            });
        var storageFile = await showDialog;
        try
        {
            var bmp = new Bitmap(storageFile.First().TryGetLocalPath());
            var guid = Guid.NewGuid();

            var baseDir = AppContext.BaseDirectory!;
            var imageDir = Path.Combine(baseDir, "products");

            var imagePath = Path.Combine(imageDir, $"{guid}.jpg");
            bmp.Save(imagePath);

            SelectedImagePath = $"products/{guid}.jpg";

            var fullPath = Path.GetFullPath(imagePath);
            Console.WriteLine("Saving image to: " + fullPath);

            return bmp;
        }
        catch
        {
            return null;
        }
    }
}