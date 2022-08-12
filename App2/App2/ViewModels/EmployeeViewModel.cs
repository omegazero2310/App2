using App2.Models;
using App2.Services;
using App2.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        public IDataStore<Employee> DataStore => DependencyService.Get<IDataStore<Employee>>();
        private Employee _selectedEmployee;
        public ObservableCollection<Employee> Employees { get; }
        public Command LoadEmployeesCommand { get; }
        public Command AddEmployeeCommand { get; }
        public Command<Employee> EmployeeTapped { get; }

        public EmployeeViewModel()
        {
            Title = "List Employee";
            Employees = new ObservableCollection<Employee>();
            LoadEmployeesCommand = new Command(async () => await ExecuteLoadEmployeesCommand());

            EmployeeTapped = new Command<Employee>(OnEmployeeSelected);

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

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);
                OnEmployeeSelected(value);
            }
        }

        private async void OnAddEmployee(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        private async void OnEmployeeSelected(Employee item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}
