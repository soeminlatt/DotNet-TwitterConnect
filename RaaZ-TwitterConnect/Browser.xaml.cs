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
using System.Windows.Shapes;

using Twitterizer;

namespace RaaZ_TwitterConnect
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : Window
    {
        public Browser()
        {
            InitializeComponent();
        }
        XMLStuff myXML;

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            browserControl.Refresh();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            myXML = new XMLStuff();
            statusBarMessage.Text = "Checking for login info..";
            bool loggedIn = AlreadyLoggedIn();
            statusBarMessage.Text = "Not logged in, shifting to login page.";
            if (!loggedIn)
            {
                Login();
                statusBarMessage.Text = "";
            }
            else
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            TwitterStuff.pin = tbPin.Text;
            Properties.Settings.Default.Save();
            TwitterStuff.tokenResponse2 = OAuthUtility.GetAccessToken(TwitterStuff.consumerKey, TwitterStuff.consumerSecret, TwitterStuff.tokenResponse.Token, tbPin.Text);
            myXML.writeToXML(TwitterStuff.tokenResponse2.ScreenName.ToString(), TwitterStuff.tokenResponse2.Token, TwitterStuff.tokenResponse2.TokenSecret);
            TwitterStuff.screenName = TwitterStuff.tokenResponse2.ScreenName.ToString();
            SetLocalTokens();
            //MessageBox.Show("Logged In and local tokens are set");
            statusBarMessage.Text = "Successfuly logged in.";
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void browserControl_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            lblUri.Content = browserControl.Source.AbsoluteUri.ToString();
            progressBar.IsIndeterminate = false;
            progressBar.Visibility = System.Windows.Visibility.Hidden;
            // MessageBox.Show(browserControl.Source.AbsoluteUri.ToString());
        }

        private void Login()
        {
            TwitterStuff.tokenResponse = OAuthUtility.GetRequestToken(TwitterStuff.consumerKey, TwitterStuff.consumerSecret, TwitterStuff.callbackAddy);
            string target = "http://twitter.com/oauth/authenticate?oauth_token=" + TwitterStuff.tokenResponse.Token;
            try
            {
                browserControl.Navigate(new Uri(target));
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }


        /// <summary>
        /// Initialize token
        /// </summary>
        private void SetLocalTokens()
        {
            TwitterStuff.tokens = new OAuthTokens();
            TwitterStuff.tokens.AccessToken = TwitterStuff.tokenResponse2.Token;
            TwitterStuff.tokens.AccessTokenSecret = TwitterStuff.tokenResponse2.TokenSecret;
            TwitterStuff.tokens.ConsumerKey = TwitterStuff.consumerKey;
            TwitterStuff.tokens.ConsumerSecret = TwitterStuff.consumerSecret;
        }
        /// <summary>
        /// if already logged in, then tokens are loaded from xml file using this method.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="tokenSec"></param>
        /// <returns></returns>
        private bool SetLocalTokens(string accessToken, string tokenSec)
        {
            try
            {
                TwitterStuff.tokens = new OAuthTokens();
                TwitterStuff.tokens.AccessToken = accessToken;
                TwitterStuff.tokens.AccessTokenSecret = tokenSec;
                TwitterStuff.tokens.ConsumerKey = TwitterStuff.consumerKey;
                TwitterStuff.tokens.ConsumerSecret = TwitterStuff.consumerSecret;
                // MessageBox.Show("Tokens with arguments initialized.");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool AlreadyLoggedIn()
        {
            try
            {
                //MessageBox.Show("Trying to get login info");
                List<string> LoginInfo = myXML.readFromXml();

                TwitterStuff.screenName = LoginInfo[1];
                if (!SetLocalTokens(LoginInfo[2], LoginInfo[3]))
                    return false;
                //MessageBox.Show("Already logged in.");
                return true;
            }
            catch (Exception e)
            {
                statusBarMessage.Text = "Not logged in.";
                return false;
            }
        }

        private void browserControl_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
