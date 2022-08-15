﻿using App2.Models;
using App2.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class NewEmployeeViewModel : BaseViewModel
    {
        private string _id;
        private string _name;
        private DateTime? _dob = DateTime.Now;
        private string _extraInfo;
        public IDataStore<Employee> DataStore => DependencyService.Get<IDataStore<Employee>>();
        public NewEmployeeViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_id)
                && !String.IsNullOrWhiteSpace(_name) && _dob.HasValue;
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
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
            Employee newEmployee = new Employee()
            {
                Id = _id,
                Name = _name,
                DateOfBirth = _dob.Value,
                ExtraInfo = _extraInfo
            };

            await DataStore.AddItemAsync(newEmployee);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}