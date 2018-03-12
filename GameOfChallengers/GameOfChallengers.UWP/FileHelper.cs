
using System.IO;

using Xamarin.Forms;

using GameOfChallengers.UWP;

using Windows.Storage;

[assembly: Dependency(typeof(FileHelper))]

namespace GameOfChallengers.UWP

{

    public class FileHelper : IFileHelper

    {

        public string GetLocalFilePath(string filename)

        {

            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);

        }

    }

}