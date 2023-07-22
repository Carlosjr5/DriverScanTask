using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Path = System.IO.Path;

namespace WpfApp1_TaskPixel
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Stack<string> dirStack;
        private Stack<string> forwardStack = new Stack<string>();
        private string currentDirectory;
        private DirectoryInfo[] directorios;
        public string SelectedDrive { get; private set; }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            dirStack = new Stack<string>();
            currentDirectory = @"C:\";
            newDir(currentDirectory);
            DataContext = this;
        }

        private void newDir(string ruta)
        {
            if (!Directory.Exists(ruta))
            {
                MessageBox.Show($"Please make sure to choose the driver to scan, in the BLUE button `Choose Drive`, thanks!{ruta}", "CJR Task", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DirectoryInfo gd = new DirectoryInfo(ruta);
            directorios = getDir(gd);
            FileInfo[] files = GetArchiv(gd);

            if (directorios == null || files == null)
                return;

            directori.Items.Clear();
            foreach (DirectoryInfo directory in directorios)
            {
                directori.Items.Add(directory.Name);
            }

            lista.Items.Clear();
            foreach (FileInfo file in files)
            {
                lista.Items.Add(file.Name);
            }

            currentDirectory = ruta;
            DirectoryTextBox.Text = currentDirectory;

            if (!dirStack.Contains(currentDirectory))
            {
                dirStack.Push(currentDirectory);
                forwardStack.Clear();
            }

        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (dirStack.Count > 1)
            {
                forwardStack.Push(currentDirectory); 
                dirStack.Pop();
                string previousDirectory = dirStack.Peek();
                newDir(previousDirectory);

                DirectoryTextBox.Text = previousDirectory;
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (forwardStack.Count > 0)
            {
                dirStack.Push(forwardStack.Pop());
                string nextDirectory = dirStack.Peek();
                newDir(nextDirectory);

                DirectoryTextBox.Text = nextDirectory;
            }
        }

        private void directori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (directori.SelectedItem != null)
            {
                string selectedDir = Path.Combine(currentDirectory, directori.SelectedItem.ToString());

                if (!selectedDir.Equals(currentDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    newDir(selectedDir);
                    dirStack.Push(currentDirectory);

                    DirectoryTextBox.Text = selectedDir;
                }
            }
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lista.SelectedItem != null)
            {
                string selectedFile = Path.Combine(currentDirectory, lista.SelectedItem.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChoseDriver selectedDriver = new ChoseDriver();

            if (selectedDriver.ShowDialog() == true)
            {
                string selectedDrive = selectedDriver.ChosenDriver;
                newDir(selectedDrive);
            }
        }

        private DirectoryInfo[] getDir(DirectoryInfo directoryInfo)
        {
            try
            {
                return directoryInfo.GetDirectories();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show($"Can't access directory: {directoryInfo.FullName}");
                return null;
            }
        }

        private FileInfo[] GetArchiv(DirectoryInfo directoryInfo)
        {
            try
            {
                return directoryInfo.GetFiles();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show($"Can't access files: {directoryInfo.FullName}");
                return null;
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchDirectory();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchDirectory();
            }
        }

        private void SearchDirectory()
        {
            string pathToSearch = SearchText.Trim();
            if (Directory.Exists(pathToSearch))
            {
                newDir(pathToSearch);
            }
            else
            {
                MessageBox.Show($"Directory not found: {pathToSearch}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
