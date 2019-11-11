using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneSort.Resources;
using System.IO;
using System.ComponentModel;

namespace PhoneSort
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<String> listIn;
        bool isSortedaToz = false;
        bool linesAdded = false;
        bool usedOnce = false;
        List<String> templist = new List<String>();
        public MainPage()
        {
            InitializeComponent();
            if (!AccessContentFile())
            {
                MessageBox.Show("No File access!");
            }
            else
            {
                MessageBox.Show("Its there..");
            }

        }

        private bool AccessContentFile()
        {
            try
            {
                using (var s = Application.GetResourceStream(new Uri("files\\words.txt", UriKind.Relative)).Stream)
                {
                    listIn = new StreamReader(s).ReadToEnd().Split('\n').ToList();
                    listIn.ForEach(word => _listBox.Items.Add(word));
                    linesAdded = true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            
            if(linesAdded == true)
            {
                var bgw = new BackgroundWorker();
                var lineSort = listIn.ToList();
                bgw.DoWork += (myobj, myev) =>
                {
                    if(isSortedaToz == false)
                    {
                        //_listBox.ItemsSource = null;
                        //_listBox.Items.Clear();
                        lineSort.Sort();
                        templist.Clear();
                        //_listBox.Items.Clear();
                        lineSort.ForEach(word => templist.Add(word));
                        //listIn.ForEach(word => _listBox.Items.Add(word));
                        isSortedaToz = true;
                    }
                    else
                    {
                        templist.Clear();
                        lineSort.Sort();
                        lineSort.Reverse();
                        lineSort.ForEach(word => templist.Add(word));
                        isSortedaToz = false;

                    }


                };
                bgw.RunWorkerCompleted += (myobj, myev) => 
                {
                    if(usedOnce == false)
                    {
                        _listBox.Items.Clear();
                        _listBox.ItemsSource = templist;
                        usedOnce = true;

                    }
                    else 
                    {
                        _listBox.ItemsSource = null;
                        //_listBox.Items.Add(templist.ToString());
                        _listBox.ItemsSource = templist;
                    
                    }

                };
                bgw.RunWorkerAsync();
            }
        }

        
    }
}