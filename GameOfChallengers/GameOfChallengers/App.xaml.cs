using System;

using GameOfChallengers.Views;
using SQLite;
using Xamarin.Forms;

namespace GameOfChallengers
{
	public partial class App : Application
	{

		public App ()
		{
		    InitializeComponent();


            MainPage = new MainPage();
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        static SQLiteAsyncConnection database;

        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (database == null)
                {
                    database = new SQLiteAsyncConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath("SQLLiteDBName2.db3"));
                }
                return database;
            }
        }

    }
}
