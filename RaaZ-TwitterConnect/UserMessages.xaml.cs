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

namespace RaaZ_TwitterConnect
{
    /// <summary>
    /// Interaction logic for UserMessages.xaml
    /// </summary>
    public partial class UserMessages : Window
    {
        public UserMessages(List<string> msgs)
        {
            InitializeComponent();

            foreach (string msg in msgs)
            {
                tbMessages.Text += msg + Environment.NewLine;
            }
        }

    }
}
