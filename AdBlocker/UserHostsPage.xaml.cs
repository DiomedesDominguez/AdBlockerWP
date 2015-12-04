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
using Windows.Storage;

namespace AdBlocker
{
    public partial class UserHostsPage : PhoneApplicationPage
    {
        StorageFolder userFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        public UserHostsPage()
        {
            InitializeComponent();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                StreamWriter file = new StreamWriter(userFolder.Path + "\\DataFolder\\userList.txt");
                file.Write(uList.Text);
                file.Close();
                NavigationService.GoBack();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Uh-Oh!", MessageBoxButton.OK);
            }

        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader getUserList = new StreamReader(userFolder.Path + "\\DataFolder\\userList.txt");
                String list = getUserList.ReadToEnd();
                uList.Text = list;
                getUserList.Close();
            } catch(Exception ex)
            {
                MessageBox.Show("Looks like this is your first time opening the custom URL list! To add a URL for AdBlocker to download, just add it on one line of the below text box. Each your URL you add is separated by line so add a URL per line. ", "Uh-Oh!", MessageBoxButton.OK);
            }


        }
    }
}