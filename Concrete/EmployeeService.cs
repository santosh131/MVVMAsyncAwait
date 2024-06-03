using MVVMAsyncAwait.Interfaces;
using MVVMAsyncAwait.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace MVVMAsyncAwait.Concrete
{
    public class EmployeeService : IEmployeeService
    {
        public async Task<List<EmployeeModel>> GetEmployeeModelsAsync()
        {
            var a = new List<EmployeeModel>();
            await Task.Run(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    a.Add(new EmployeeModel() { Id = i, Name = $"Sam {i.ToString()}" });
                }
            });
            return a;
        }

        public async Task<List<EmployeeModel>> GetEmployeeModelsAsync(Func<EmployeeModel, bool> predicate)
        {
            List<EmployeeModel> a = new List<EmployeeModel>();
            await Task.Run(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    a.Add(new EmployeeModel() { Id = i, Name = $"Sam {i.ToString()}" });
                }
            });

            return a
                .Where(predicate)
                .ToList();
        }

        public async Task<List<EmployeeModel>> GetEmployeeModelLongProcessAsync(IProgress<int> progress, CancellationToken cts)
        {
            List<EmployeeModel> lst = new List<EmployeeModel>();
            await Task.Run(async () =>
            {
                int totalRecords = 100;
                for (int i = 0; i < totalRecords; i++)
                {
                    await Task.Delay(1000);
                    cts.ThrowIfCancellationRequested();
                    lst.Add(new EmployeeModel() { Id = i, Name = $"Sam {i.ToString()}" });
                    if (progress != null)
                    {
                        progress.Report(i * 100 / totalRecords);
                    }
                }
            }, cts);
            return lst;
        }
    }
}
