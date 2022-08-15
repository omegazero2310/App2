using App2.Models;
using App2.Services;
using App2.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;

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
        public Command<Employee> EmployeeSwipeEdit { get; }

        public EmployeeViewModel() : base()
        {
            Title = "List Employee";
            Employees = new ObservableCollection<Employee>();
            LoadEmployeesCommand = new Command(async () => await ExecuteLoadEmployeesCommand());

            EmployeeTapped = new Command<Employee>(OnEmployeeSelected);
            EmployeeSwipeDelete = new Command<Employee>(OnEmployeeDelete);
            EmployeeSwipeEdit = new Command<Employee>(OnEmployeeEdit);

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
            if (!IsDisconnected)
                await Shell.Current.GoToAsync(nameof(NewEmployeePage));
            else
                await App.Current.MainPage.DisplayAlert("Cannot connect to the Server", "Check your internet connection", "Ok");
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
                if (!IsDisconnected)
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
                else
                    await App.Current.MainPage.DisplayAlert("Cannot connect to the Server", "Check your internet connection", "Ok");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Server Error", $"{ex.Message} ?", "OK");
            }

        }
        private async void OnEmployeeEdit(Employee item)
        {
            try
            {
                if (!IsDisconnected)
                {
                    if (item == null)
                        return;
                    await Shell.Current.GoToAsync($"{nameof(NewEmployeePage)}?{nameof(NewEmployeeViewModel.EmployeeID)}={item.Id}");
                }
                else
                    await App.Current.MainPage.DisplayAlert("Cannot connect to the Server", "Check your internet connection", "Ok");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Server Error", $"{ex.Message} ?", "OK");
            }

        }
    }
}
