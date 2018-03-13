using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GameOfChallengers.ViewModels;
using GameOfChallengers.Services;
using GameOfChallengers.Controllers;
using GameOfChallengers.Models;


namespace GameOfChallengers.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = ValueForRoll;
            SettingDataSource.IsToggled = true;
            DebugSetting.IsToggled = true;
            ServerPost.IsToggled = false;
        }



        private void Switch_OnDebugSetting(object sender, ToggledEventArgs e)
        {

            if (e.Value == true)
            {
                GameGlobals.Debug = true;
                MockDatastore.IsVisible = true;
                ClearDatabase.IsVisible = true;
                ServerItem.IsVisible = true;
                RandomNumber.IsVisible = true;
                DebugValue.IsVisible = true;
            }
            else
            {
                GameGlobals.Debug = false;
                MockDatastore.IsVisible = false;
                ClearDatabase.IsVisible = false;
                ServerItem.IsVisible = false;
                RandomNumber.IsVisible = false;
                DebugValue.IsVisible = false;
            }

        }



        private void Switch_OnRandomNumber(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
                GameGlobals.DisableRandomNumbers = true;
            else
                GameGlobals.DisableRandomNumbers = false;
        }


        private void Switch_OnMiss(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
                GameGlobals.EnableCriticalMiss = true;
            else
                GameGlobals.EnableCriticalMiss = false;

        }


        private void Switch_OnHit(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
                GameGlobals.EnableCriticalHit = true;
            else
                GameGlobals.EnableCriticalHit = false;
        }
        private void Round_Healing(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
                GameGlobals.AllowRoundHealing = true;
            else
                GameGlobals.AllowRoundHealing = false;

        }
        
        private void Volcano(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
                GameGlobals.EnableRandomBadThings = true;
            else
                GameGlobals.EnableRandomBadThings = false;

        }

        private void set_roll(object sender, ToggledEventArgs e)
        {
            GameGlobals.RollValue = Convert.ToInt32(ValueForRoll.Text);
        }
        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.

            if (e.Value == true)
            {
                ItemsViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Mock);
                MonstersViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Mock);
                CharactersViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Mock);
                ScoresViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Mock);
            }
            else
            {
                ItemsViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Sql);
                MonstersViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Sql);
                CharactersViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Sql);
                ScoresViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Sql);
            }

            // Have data refresh...
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            MonstersViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.SetNeedsRefresh(true);
            ScoresViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.LoadDataCommand.Execute(null);
            MonstersViewModel.Instance.LoadDataCommand.Execute(null);
            ScoresViewModel.Instance.LoadDataCommand.Execute(null);
            ItemsViewModel.Instance.LoadDataCommand.Execute(null);
        }

        private async void ClearDatabase_Command(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete", "Sure you want to Delete All Data, and start over?", "Yes", "No");
            if (answer)
            {
                // Call to the SQL DataStore and have it clear the tables.
                SQLDataStore.Instance.InitializeDatabaseNewTables();
            }
        }

        //private async void GetItems_Command(object sender, EventArgs e)
        //{
        //    var answer = await DisplayAlert("Get", "Sure you want to Get Items from the Server?", "Yes", "No");
        //    if (answer)
        //    {
        //        // Call to the Item Service and have it Get the Items
        //        ItemsController.Instance.GetItemsFromServer();
        //    }
        //}

        private async void GetItemsPost_Command(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                //ItemsController.Instance.GetItemsFromGame(int number, int level, AttributeEnum attribute, ItemLocationEnum location, bool random, bool updateDataBase)

                var number = 25;    // 10 items
                var level = 20;  // Max Value of 6
                var attribute = AttributeEnum.Unknown;  // Any Attribute
                var location = ItemLocationEnum.Unknown;    // Any Location
                var random = true;  // Random between 1 and Level
                var updateDataBase = true;  // Add them to the DB

                var myDataList = await ItemsController.Instance.GetItemsFromGame(number, level, attribute, location, random, updateDataBase);

                var myOutput = "No Results";

                if (myDataList != null && myDataList.Count > 0)
                {
                    // Reset the output
                    myOutput = "";

                    foreach (var item in myDataList)
                    {
                        // Add them line by one, use \n to force new line for output display.
                        myOutput += item.FormatOutput() + "\n";
                    }
                }

                var answer = await DisplayAlert("Returned List", myOutput, "Yes", "No");
            }
        }
    }
}