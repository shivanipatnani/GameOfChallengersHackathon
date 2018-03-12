using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using GameOfChallengers.Models;
using GameOfChallengers.Services;

namespace GameOfChallengers.ViewModels
{
    public class TeamViewModel : BaseViewModel
    {
        private static TeamViewModel _instance;

        public static TeamViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TeamViewModel();
                }
                return _instance;
            }
        }

        public ObservableCollection<Creature> Dataset { get; set; }
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh;

        public TeamViewModel()
        {
            //a list of the characters on the team
            //a list of actuall characters to use in the current game
            //the second list will reload from the first at the start of each game
            //the first list will be reset by the build team actions

            Title = "Current Team";
            Dataset = new ObservableCollection<Creature>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
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
        
        public void LoadData()
        {
            Dataset.Clear();
            if (CharactersViewModel.Instance.Dataset.Count == 0)
            {
                CharactersViewModel.Instance.LoadDataCommand.Execute(null);
            }
            else if (CharactersViewModel.Instance.NeedsRefresh())
            {
                CharactersViewModel.Instance.LoadDataCommand.Execute(null);
            }
            var dataset = CharactersViewModel.Instance.GetAllCreatures();
            int teamCount = 0;
            foreach (var data in dataset)
            {
                if ((data.Type == 0) && (teamCount < 6))//&& data.OnTeam)//the creature is a character, the team is not full, and it is on the current team
                {
                    teamCount++;
                    Creature newOne = new Creature();
                    newOne.Update(data);
                    newOne.Id = Guid.NewGuid().ToString();
                    Dataset.Add(newOne);
                }
            }
        }
        async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Dataset.Clear();
                //var dataset = await SQLDataStore.GetAllAsync_Creature(true);
                if (CharactersViewModel.Instance.Dataset.Count == 0)
                {
                    CharactersViewModel.Instance.LoadDataCommand.Execute(null);
                }
                else if (CharactersViewModel.Instance.NeedsRefresh())
                {
                    CharactersViewModel.Instance.LoadDataCommand.Execute(null);
                }
                var dataset = CharactersViewModel.Instance.GetAllCreatures();
                int teamCount = 0;
                foreach (var data in dataset)
                {
                    if ((data.Type == 0) && (teamCount < 6) && data.OnTeam)//the creature is a character, the team is not full, and it is on the current team
                    {
                        teamCount++;
                        Dataset.Add(data);
                    }
                }
                if (teamCount < 6)
                {
                    foreach (var data in dataset)//if the team is not full more characters must be added
                    {
                        if ((data.Type == 0) && (teamCount < 6) && !Dataset.Contains(data))//the creature is a character, the team is not full, and the character is not in the team
                        {
                            teamCount++;
                            data.OnTeam = true;
                            Dataset.Add(data);
                        }
                    }
                }
                if (teamCount < 6)//if you didn't make enough characters you get some sucky ones
                {
                    int numOfSuckyCharacters = 0;
                    for (int i=teamCount; i<6; i++)
                    {
                        numOfSuckyCharacters++;
                        Creature character = new Creature();
                        character.Type = 0;
                        character.OnTeam = true;
                        character.Name = "Sucky Character " + numOfSuckyCharacters.ToString();
                        character.Attack = 1;
                        character.Defense = 1;
                        character.Speed = 1;
                        character.MaxHealth = 1;
                        character.CurrHealth = character.MaxHealth;
                        Dataset.Add(character);
                        await DataStore.AddAsync_Creature(character);
                    }
                }
                //                      ***temp for demo***
                foreach (var data in Dataset)
                {
                    data.RHandItemID = "bow";
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            finally
            {
                IsBusy = false;
            }
        }
    }
}
