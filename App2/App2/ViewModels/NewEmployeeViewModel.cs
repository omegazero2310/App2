using App2.Models;
using App2.Services;
using App2.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static SQLite.SQLite3;

namespace App2.ViewModels
{
    [QueryProperty(nameof(EmployeeID), nameof(EmployeeID))]
    public class NewEmployeeViewModel : BaseViewModel
    {
        private string _id;
        private string _name;
        private DateTime? _dob = DateTime.Now;
        private string _extraInfo;
        private bool _isNew;
        private string _errorStr;
        public IDataStore<Employee> DataStore => DependencyService.Get<IDataStore<Employee>>();
        public Dictionary<string, string> ErrorMessages { get; set; }
        public NewEmployeeViewModel()
        {
            this.ErrorMessages = new Dictionary<string, string>();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            this.IsNew = true;
        }

        private bool ValidateSave()
        {
            return true;
        }
        public bool IsNew
        {
            get => _isNew;
            set
            {
                _isNew = value;
                OnPropertyChanged();
            }
        }
        public string ErrorStr
        {
            get => _errorStr;
            set
            {
                _errorStr = value;
                OnPropertyChanged();
            }
        }
        public string EmployeeID
        {
            get => _id;
            set
            {
                this.IsNew = false;
                _id = value;
                GetEmployee(value);
            }
        }

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
            set => SetProperty(ref _name, value);
        }
        public DateTime DateOfBirth
        {
            get => _dob.GetValueOrDefault();
            set => SetProperty(ref _dob, value);
        }
        public string ExtraInfo
        {
            get => _extraInfo;
            set => SetProperty(ref _extraInfo, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            try
            {
                Employee newEmployee = new Employee()
                {
                    Id = _id,
                    Name = _name,
                    DateOfBirth = _dob.Value,
                    ExtraInfo = _extraInfo ?? string.Empty
                };
                EmployeeValidator validationRules = new EmployeeValidator();
                var result = validationRules.Validate(newEmployee);
                if (!result.IsValid)
                {
                    foreach (var e in result.Errors)
                    {
                        ErrorMessages[e.PropertyName] = e.ErrorMessage;
                    }
                    OnPropertyChanged(nameof(ErrorMessages));
                    return;
                }
                else
                {
                    this.ErrorStr = string.Empty;
                    if (this.IsNew)
                        await DataStore.AddItemAsync(newEmployee);
                    else
                        await DataStore.UpdateItemAsync(newEmployee);
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Server Error", ex.Message, "OK");
            }

        }
        private async void GetEmployee(string value)
        {
            var item = await DataStore.GetItemAsync(value);
            Id = item.Id;
            Name = item.Name;
            DateOfBirth = item.DateOfBirth;
            ExtraInfo = item.ExtraInfo;
            this.IsNew = false;
        }
    }
}
