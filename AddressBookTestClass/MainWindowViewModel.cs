using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;
using WpfCommonLibrary;

namespace AddressBookTestClass
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(ListView listView)
        {
            this.listView = listView;
        }

        private string search;
        private ObservableCollection<Contact> contacts;
        private Contact selectedContact;
        private ListView listView;

        public string Search
        {
            get { return search; }
            set
            {
                SetProperty(ref search, value);
                Notify(() => FilteredContacts);
                if (SelectedContact == null && FilteredContacts.Count > 0)
                {
                    SelectedContact = Contacts[0];
                }
            }
        }

        public ObservableCollection<Contact> FilteredContacts
        {
            get
            {
                return String.IsNullOrWhiteSpace(search) ? new ObservableCollection<Contact>((from contact in contacts orderby contact.Name select contact).ToList<Contact>()):
                    new ObservableCollection<Contact>((from contact in contacts where contact.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 orderby contact.Name select contact).ToList<Contact>());
            }
        }

        public ObservableCollection<Contact> Contacts
        {
            get { return contacts; }
            set
            {
                SetProperty(ref contacts, value);
                Contacts.CollectionChanged += OnContactsCollectionChanged;
                Notify(() => FilteredContacts);
            }
        } 

        public Contact SelectedContact
        {
            get { return selectedContact; }
            set
            {
                SetProperty(ref selectedContact, value);
                if (selectedContact != null)
                    selectedContact.PropertyChanged += SelectedContact_PropertyChanged;
            }
        }

        private void OnContactsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify(() => FilteredContacts);
        }

        private void SelectedContact_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify(() => FilteredContacts);
            listView.ScrollIntoView(SelectedContact);
        }
    }
}
