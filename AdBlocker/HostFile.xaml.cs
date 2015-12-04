using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Windows.Storage;
using Nokia.SilentInstaller.Runtime;

namespace AdBlocker
{
    public partial class HostFile : PhoneApplicationPage
    {
        bool WantQuit = false;
        public HostFile()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            saveBtn.IsEnabled = false;
            emptyBtn.IsEnabled = false;
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            StreamReader hostText = new StreamReader("C:\\Windows\\System32\\DRIVERS\\ETC\\HOSTS");
            String stringHost = hostText.ReadToEnd();
            hostText.Close();
            stringHost = stringHost.Replace("\r\n", "<br />").Replace("\r","<br />").Replace("\n","<br />");
            String CurrentHTML =
            @"<html>
<head>

</head>
<div style=""width: 100%; height:100%;"" id=""textbox"" contenteditable='true'>
" + stringHost + @"
</div>
";

            wb1.NavigateToString(CurrentHTML);
            pb1.Visibility = Visibility.Collapsed;
            saveBtn.IsEnabled = true;
            emptyBtn.IsEnabled = true;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //Do your work here
            if(WantQuit == true)
            {
                NavigationService.GoBack();

            }else
            {
                e.Cancel = true;
            }
            WantQuit = true;
        }
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //await WriteToFile(hostsText.Text);
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            RPCManager RPC = new RPCManager();
            RPC.Start();
            CSilentInstallerRuntime NrsCopy = new CSilentInstallerRuntime();
            bool didFinish = NrsCopy.NRSCopyFile(local.Path + "\\DataFolder\\hosts.txt", "C:\\Windows\\System32\\DRIVERS\\ETC\\HOSTS");
            if (didFinish == bool.Parse("true"))
            {
                MessageBox.Show("Hosts file saved!", "Saved!", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Could not save Hosts File....", "Uh-Oh!", MessageBoxButton.OK);
            }
        }
        private async Task WriteToFile(string content)
        {
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(content.ToCharArray());

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("DataFolder",
                CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync("hosts.txt",
            CreationCollisionOption.ReplaceExisting);

            // Write the data from the textbox.
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }
        }

        private void emptyBtn_Click(object sender, RoutedEventArgs e)
        {
            //hostsText.Text = "";
        }

        private void wb1_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void wb1_Navigating(object sender, NavigatingEventArgs e)
        {
            pb1.Visibility = Visibility.Visible;
            saveBtn.IsEnabled = false;
            emptyBtn.IsEnabled = false;
        }
    }
}