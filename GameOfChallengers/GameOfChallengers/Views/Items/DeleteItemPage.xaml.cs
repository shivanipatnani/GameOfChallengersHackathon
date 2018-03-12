﻿using GameOfChallengers.Models;
using GameOfChallengers.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameOfChallengers.Views.Items
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteItemPage : ContentPage
    {

        private ItemDetailViewModel _viewModel;

        public Item Data { get; set; }

        public DeleteItemPage(ItemDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Delete " + viewModel.Title;

            InitializeComponent();

            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
                MessagingCenter.Send(this, "DeleteData", Data);

                // Remove Item Details Page manualy
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

                await Navigation.PopAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}