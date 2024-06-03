using MVVMAsyncAwait.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMAsyncAwait.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetEmployeeModelsAsync();
        Task<List<EmployeeModel>> GetEmployeeModelLongProcessAsync(IProgress<int> progress, CancellationToken cts);
    }
}
