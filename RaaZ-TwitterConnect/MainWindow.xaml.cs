using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Twitterizer;

namespace RaaZ_TwitterConnect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        TwitterStuff ts;

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            ts = new TwitterStuff();
            userTimeLine();
            updateList(listFollowers);
        }
        public void userTimeLine()
        {
            TwitterResponse<TwitterStatusCollection> timeLine = TwitterTimeline.HomeTimeline(TwitterStuff.tokens);
            foreach (var item in timeLine.ResponseObject)
            {
                string textStatus = item.Text;
                tbTimeLine.Text += ">> " + textStatus + Environment.NewLine;
            }
        }
        public void updateList(ComboBox cb1)
        {
            try
            {
                TwitterResponse<TwitterUserCollection> tu = TwitterFriendship.Followers(TwitterStuff.tokens);
                if (tu == null)
                {
                    MessageBox.Show("You don't have any followers or some error occured.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    var tc = tu.ResponseObject;
                    foreach (TwitterUser twitterUser in tc)
                    {
                        cb1.Items.Add(twitterUser.ScreenName);
                    }
                }
            }
            catch (TwitterizerException te)
            {
                MessageBox.Show(te.Message);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string recipent = listFollowers.SelectedItem.ToString();
                if (recipent != null && tbNewMsg.Text != "")
                {
                    if (tbNewMsg.Text.Length < 140)
                    {
                        MessageBox.Show("Sending to " + recipent);
                        ts.SendMessage(tbNewMsg.Text, recipent);
                        tbNewMsg.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Message length must be less than 140");
                    }
                }
            }
            catch (Exception) { }
        }

        private void btnTweet_Click(object sender, RoutedEventArgs e)
        {
            if (tbTweet.Text != "")
            {
                ts.tweetIt(tbTweet.Text);
                tbTweet.Clear();
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            List<string> tempMesgs = ts.getMessages();
            if (tempMesgs != null)
            {
                UserMessages um = new UserMessages(tempMesgs);
                um.ShowDialog();
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            XMLStuff xs = new XMLStuff();
            xs.Logout();
            Application.Current.Shutdown();
        }
    }
}
