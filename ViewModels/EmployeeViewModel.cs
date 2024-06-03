using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMAsyncAwait.Interfaces;
using MVVMAsyncAwait.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMAsyncAwait.ViewModels
{
    public class EmployeeViewModel : ObservableObject
    {
        private readonly IEmployeeService _employeeService;

        public IAsyncRelayCommand EmployeesCommand { get; }
        public IAsyncRelayCommand EmployeesTextCommand { get; }
        public IAsyncRelayCommand EmployeesLongProcessCommand { get; }
        public RelayCommand EmployeesLongProcessCancelCommand { get; }
        private List<EmployeeModel> _employees;
        private CancellationTokenSource Cts { get; set; }
        private bool _canLongProcess = false;

        private int _currentProgress;
        public int CurrentProgress
        {
            get
            {
                return _currentProgress;
            }
            set
            {
                _currentProgress = value;
                OnPropertyChanged(nameof(CurrentProgress));
            }
        }

        public List<EmployeeModel> Employees
        {
            get
            {
                return _employees;
            }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public bool CanLongProcess
        {
            get
            {
                return _canLongProcess;
            }

            set
            {
                if (_canLongProcess == value) return;
                _canLongProcess = value;
                OnPropertyChanged(nameof(CanLongProcess));
            }
        }

        public EmployeeViewModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            EmployeesTextCommand = new AsyncRelayCommand(GetEmployeeTextAsync);
            EmployeesCommand = new AsyncRelayCommand(GetEmployeesAsync);
            EmployeesLongProcessCommand = new AsyncRelayCommand(GetEmployeesLongProcessAsync, CanLongProcessRun, AsyncRelayCommandOptions.AllowConcurrentExecutions);
            EmployeesLongProcessCancelCommand = new RelayCommand(CancelEmployeesLongProcess);
            CanLongProcess = true;
        }

        private bool CanLongProcessRun()
        {
            return _canLongProcess;
        }

        private async Task<string> GetEmployeeTextAsync()
        {
            await Task.Delay(5000);
            return "Hello";
        }

        private async Task<List<EmployeeModel>> GetEmployeesAsync()
        {
            var e = await _employeeService.GetEmployeeModelsAsync();
            Employees = e;
            return e;
        }

        private async Task<List<EmployeeModel>> GetEmployeesLongProcessAsync()
        {
            CanLongProcess = false;
            var progressIndicator = new Progress<int>(ReportProgress);
            Cts = new CancellationTokenSource();
            Task<List<EmployeeModel>> task = _employeeService.GetEmployeeModelLongProcessAsync(progressIndicator, Cts.Token);
            try
            {
                var e = await task;
                Employees = e;
                return e;
            }
            catch (OperationCanceledException ex)
            {
                Console.Write($"Cancellation {ex.Message}");
            }
            finally
            {
                Cts.Dispose();
                CanLongProcess = true;
            }
            return new List<EmployeeModel>();
        }

        private void CancelEmployeesLongProcess()
        {
            Cts.Cancel();
            CanLongProcess = true;
        }

        private void ReportProgress(int value)
        {
            CurrentProgress = value;
        }
    }
}
