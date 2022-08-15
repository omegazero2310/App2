using App2.Models;
using App2.Services;
using App2.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace App2.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);
                OnEmployeeSelected(value);
            }
        }

        public IDataStore<Employee> DataStore => DependencyService.Get<IDataStore<Employee>>();
        public ObservableCollection<Employee> Employees { get; }
        public Command LoadEmployeesCommand { get; }
        public Command AddEmployeeCommand { get; }
        public Command<Employee> EmployeeTapped { get; }
        public Command<Employee> EmployeeSwipeDelete { get; }

        public EmployeeViewModel()
        {
            Title = "List Employee";
            Employees = new ObservableCollection<Employee>();
            LoadEmployeesCommand = new Command(async () => await ExecuteLoadEmployeesCommand());

            EmployeeTapped = new Command<Employee>(OnEmployeeSelected);
            EmployeeSwipeDelete = new Command<Employee>(OnEmployeeDelete);

            AddEmployeeCommand = new Command(OnAddEmployee);
        }

        private async Task ExecuteLoadEmployeesCommand()
        {
            IsBusy = true;

            try
            {
                Employees.Clear();
                var listEmployees = await DataStore.GetItemsAsync(true);
                foreach (var item in listEmployees)
                {
                    Employees.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedEmployee = null;
        }



        private async void OnAddEmployee(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewEmployeePage));
        }

        private async void OnEmployeeSelected(Employee item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(EmployeeDetailPage)}?{nameof(EmployeeDetailViewModel.EmployeeID)}={item.Id}");
        }
        private async void OnEmployeeDelete(Employee item)
        {
            try
            {
                if (item == null)
                    return;
                var result = await App.Current.MainPage.DisplayAlert("Confirm Delete", $"Are you sure to delete User {item.Id} ?", "Confirm", "Cancel", FlowDirection.LeftToRight);
                if (result)
                {
                    await DataStore.DeleteItemAsync(item.Id);
                    Employees.Remove(Employees.Where(s => s.Id == item.Id).First());
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Server Error", $"{ex.Message} ?", "OK");
            }
            
        }
    }
}
