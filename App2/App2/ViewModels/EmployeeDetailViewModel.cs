using App2.Models;
using App2.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace App2.ViewModels
{
    /// <summary>[ViewModel] view single employee info</summary>
    /// <Modified>
    /// Name Date Comments
    /// annv3 15/08/2022 created
    /// </Modified>
    [QueryProperty(nameof(EmployeeID), nameof(EmployeeID))]
    public class EmployeeDetailViewModel : BaseViewModel
    {
        public IDataStore<Employee> DataStore => DependencyService.Get<IDataStore<Employee>>();
        private string _id;
        private string _name;
        private string _extraInfo;
        private DateTime? _dob = DateTime.Now;

        public string Id
        {
            get => _id;
            set 
            { 
                SetProperty(ref _id, value);
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged();
            }
        }

        public string ExtraInfo
        {
            get => _extraInfo;
            set
            {
                SetProperty(ref _extraInfo, value);
                OnPropertyChanged();
            }
        }
        public DateTime DateOfBirth
        {
            get => _dob.Value;
            set
            {
                SetProperty(ref _dob, value);
                OnPropertyChanged();
            }
        }

        public string EmployeeID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                ExtraInfo = item.ExtraInfo;
                DateOfBirth = item.DateOfBirth;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
