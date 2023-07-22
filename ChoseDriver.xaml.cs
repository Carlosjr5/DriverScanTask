using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace WpfApp1_TaskPixel
{
    /// <summary>
    /// Interaction logic for ChoseDriver.xaml
    /// </summary>
    public partial class ChoseDriver : Window
    {
        public string ChosenDriver { get; private set; }

        public ChoseDriver()
        {
            InitializeComponent();
            LoadAvailableDrives();
        }

        private void LoadAvailableDrives()
        {
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                ComboBoxDrives.Items.Add(drive.Name);
            }

            if (ComboBoxDrives.Items.Count > 0)
            {
                ComboBoxDrives.SelectedIndex = 0;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ChosenDriver = ComboBoxDrives.SelectedItem.ToString();
            DialogResult = true;
        }
    }
}
