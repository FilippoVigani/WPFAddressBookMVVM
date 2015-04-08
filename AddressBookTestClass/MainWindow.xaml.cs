using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using WpfUtils;

namespace AddressBookTestClass
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        private MainWindowViewModel viewModel;

        public MainWindowViewModel ViewModel
        {
            get
            {
                return viewModel;
            }
            set
            {
                SetProperty(ref viewModel, value);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel(ContactsListView) {Contacts = new ObservableCollection<Contact>()};
        }

        private ObservableCollection<Contact> LoadDefaultContacts()
        {
            var contacts = new ObservableCollection<Contact>
            {
                new Contact()
                {
                    Name = "Paolo Gigi",
                    Number = "0421232323",
                    Email = "paul@hotmail.it"
                },
                new Contact()
                {
                    Name = "Mario Super",
                    Number = "0421222222",
                    Email = "mair@hotmail.it"
                }
            };
            return contacts;
        }

        private void SaveAddressBookButton_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };


            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var stream = saveFileDialog.OpenFile())
                {
                    var serializer = new XmlSerializer(ViewModel.Contacts.GetType());

                    serializer.Serialize(stream, ViewModel.Contacts);
                }
            }
            
        }

        private void LoadAddressBookButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };


            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var stream = openFileDialog.OpenFile())
                {
                    var serializer = new XmlSerializer(ViewModel.Contacts.GetType());

                    using (var reader = new StreamReader(stream))
                    {
                        ViewModel.Contacts = (ObservableCollection<Contact>)(serializer.Deserialize(reader));
                    }

                }
            }
        }

        private void RemoveContactButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedContact == null) return;
            ViewModel.Contacts.Remove(ViewModel.SelectedContact);
        }

        private void AddContactButton_OnClick(object sender, RoutedEventArgs e)
        {
            var entry = new Contact();
            if (ViewModel.SelectedContact != null)
            {
                ViewModel.Contacts.Insert(ViewModel.Contacts.IndexOf(ViewModel.SelectedContact) + 1, entry);
            }
            else
            {
                ViewModel.Contacts.Add(entry);
            }

            ViewModel.SelectedContact = entry;
            ContactsListView.ScrollIntoView(ViewModel.SelectedContact);
        }

        
    }
}
