using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using GameOfChallengers.Models;
using GameOfChallengers.Services;

namespace GameOfChallengers.ViewModels
{
    public class DroppedItemViewModel : BaseViewModel
    {
        private static DroppedItemViewModel _instance;

        public static DroppedItemViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DroppedItemViewModel();
                }
                return _instance;
            }
        }



        public ObservableCollection<Item> Dataset { get; set; }
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh;

        public DroppedItemViewModel()
        {
            //a list of the characters on the team
            //a list of actuall characters to use in the current game
            //the second list will reload from the first at the start of each game
            //the first list will be reset by the build team actions

            Title = "Dropped Item";
            Dataset = new ObservableCollection<Item>();
           // LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
        }



        // Return True if a refresh is needed
        // It sets the refresh flag to false
        public bool NeedsRefresh()
        {
            if (_needsRefresh)
            {
                _needsRefresh = false;
                return true;
            }

            return false;
        }

        // Sets the need to refresh
        public void SetNeedsRefresh(bool value)
        {
            _needsRefresh = value;
        }

        //not sure if this is required 
        //async Task ExecuteLoadDataCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    try
        //    {
        //        Dataset.Clear();
        //        var dataset = await DataStore.GetAllAsync_Item(true);
        //        foreach (var data in dataset)
        //        {
        //            if (data.Type == 0)// just Characters
        //            {
        //                Dataset.Add(data);
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }

        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}


    }
}