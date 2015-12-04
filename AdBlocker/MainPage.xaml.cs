using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Windows.UI.Popups;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AdBlocker.Resources;
using Windows.Web.Http;
using Windows.Storage;
using System.IO;
using System.Threading.Tasks;
using Nokia.SilentInstaller.Runtime;
using System.Text;

namespace AdBlocker
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                progress.Items.Clear();
                progress.Items.Add("Downloading config now....");
                //progress.Items.Add("Downloading first hosts file...");
                //string host1 = await GetString("https://adaway.org/hosts.txt");
                //if (host1.Contains("Response status code"))
                //{
                //    host1 = "";
                //    progress.Items.Add("Error with first file...");
                //}
                //progress.Items.Add("Downloading second hosts file...");
                //string host2 = await GetString("https://raw.githubusercontent.com/StevenBlack/hosts/master/hosts");
                //if (host2.Contains("Response status code"))
                //{
                //    host2 = "";
                //    progress.Items.Add("Error with second file...");
                //}
                StringBuilder UserHosts = new StringBuilder();
                StorageFolder userList = Windows.Storage.ApplicationData.Current.LocalFolder;
                int counter = 0;
                string line;
                // Read the file and display it line by line.
                System.IO.StreamReader file =
                   new System.IO.StreamReader(userList.Path + "\\DataFolder\\userList.txt");
                while ((line = file.ReadLine()) != null)
                {
                    progress.Items.Add("Downloading " + line);
                    string tempLines = await GetString(line);
                    if (tempLines.Contains("Response status code"))
                    {
                        tempLines = "";
                        progress.Items.Add("Error encountered while downloading " + line);
                    }
                    UserHosts.Append(tempLines).AppendLine();
                    counter++;
                }

                file.Close();

                await WriteToFile(UserHosts.ToString());
                string readFile1 = await ReadFile();
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                //StorageFolder hosts = "C:\\Windows\\System32\\DRIVERS\\etc";
                //NRSCopyFile(local.Path.ToString + "DataFolder\hosts.txt", "\Windows\System32\DRIVERS\etc\HOSTS");
                //File.Copy(local.Path + "\\DataFolder\\hosts.txt", "C:\\Windows\\System32\\DRIVERS\\ETC\\HOSTS",true);
                RPCManager RPC = new RPCManager();
                RPC.Start();
                CSilentInstallerRuntime NrsCopy = new CSilentInstallerRuntime();
                bool didFinish = NrsCopy.NRSCopyFile(local.Path + "\\DataFolder\\hosts.txt", "C:\\Windows\\System32\\DRIVERS\\ETC\\HOSTS");
                if(didFinish == bool.Parse("true"))
                {
                    String report = "Downloaded " + counter + " URLs";
                    progress.Items.Add(report);
                    progress.Items.Add("FINISHED!");
                } else
                {
                    progress.Items.Add("ERROR while coping to HOSTS (Are you Interop/Cap Unlocked?)");
                }
               
                //MessageBox.Show("Here is your new hosts file:" + Environment.NewLine + readFile1, "Finished", MessageBoxButton.OK);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR",MessageBoxButton.OK);
                progress.Items.Add("Errors during compilation:");
                progress.Items.Add(ex.Message);
            }
        }

        private async Task<string> ReadFile()
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                // Get the DataFolder folder.
                var dataFolder = await local.GetFolderAsync("DataFolder");

                // Get the file.
                var file = await dataFolder.OpenStreamForReadAsync("hosts.txt");

                // Read the data.
                using (StreamReader streamReader = new StreamReader(file))
                {
                    return streamReader.ReadToEnd();
                }

            }
            else
            {
                return "Error reading file!";
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
        private async Task WriteToFileAdv(string content, string file1)
        {
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(content.ToCharArray());

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("DataFolder",
                CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync(file1,
            CreationCollisionOption.ReplaceExisting);

            // Write the data from the textbox.
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }
        }
        public async Task<string> GetString(string URIString)
        {
            try
            {
                //Create HttpClient
                HttpClient httpClient = new HttpClient();

                //Define Http Headers
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("text/html");

                //Call
                string ResponseString = await httpClient.GetStringAsync(new Uri(URIString));
                //Replace current URL with your URL
                return ResponseString;
            }

            catch (Exception ex)
            {
                //....
                return ex.Message;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserHostsPage.xaml", UriKind.Relative));

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/about.xaml", UriKind.Relative));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string curHosts;
                using (StreamReader streamReader = new StreamReader("C:\\Windows\\System32\\DRIVERS\\ETC\\HOSTS"))
                {
                    curHosts = streamReader.ReadToEnd();
                }
                MessageBox.Show(curHosts, "Current Hosts File", MessageBoxButton.OK);
            }
            catch
            {
                MessageBox.Show("You must be Interop-Unlocked with all Capabilities unlocked as well to use this app!", "Not unlocked!", MessageBoxButton.OK);
            }

        }

        private async void button4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await WriteToFile("# Hosts reset by AdBlockerWP");
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                RPCManager RPC = new RPCManager();
                RPC.Start();
                CSilentInstallerRuntime NrsCopy = new CSilentInstallerRuntime();
                bool didFinish = NrsCopy.NRSCopyFile(local.Path + "\\DataFolder\\hosts.txt", "C:\\Windows\\System32\\DRIVERS\\ETC\\HOSTS");
                if (didFinish == bool.Parse("true"))
                {
                    progress.Items.Add("Finished unblocking ads...");
                }
                else
                {
                    progress.Items.Add("Error writing to hosts file... (Are you interop-unlocked?)...");
                }
            }
            catch
            {
                MessageBox.Show("You must be Interop-Unlocked with all Capabilities unlocked as well to use this app!", "Not unlocked!", MessageBoxButton.OK);
            }
        }

        private void progress_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                String item = progress.SelectedItem.ToString();
                MessageBox.Show(item, "", MessageBoxButton.OK);
            }
            catch
            {

            }
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder DataFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (File.Exists(DataFolder.Path + "\\DataFolder\\userList.txt"))
            {
                //Do nothing for now.
            }
            else
            {
                await WriteToFileAdv("https://raw.githubusercontent.com/StevenBlack/hosts/master/hosts" + Environment.NewLine + "https://adaway.org/hosts.txt", "userList.txt");
            }
        }

        private void editWinHostBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HostFile.xaml", UriKind.Relative));
        }
    }
}