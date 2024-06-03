# MVVMAsyncAwait  

Install following packages  
**CommunityToolkit.Mvvm**
**Microsoft.Extensions.DependencyInjection**

A simple WPF application using MVVM & async , await. Implemented button click events using the AsyncRelayCommand.

```  
private async Task<List<EmployeeModel>> GetEmployeesAsync(){
  var e = await _employeeService.GetEmployeeModelsAsync();
}  
```  

## Cancellation of async operation  

**async** operation can be cancelled using the **CancellationTokenSource**. Send the CancellationToken to the async function. cts.ThrowIfCancellationRequested will throw an cancellation exception when the CTS.Cancel() method is called.

```

private async Task<List<EmployeeModel>> GetEmployeesLongProcessAsync()
{
**Cts = new CancellationTokenSource();**  
Task<List<EmployeeModel>> task = _employeeService.GetEmployeeModelLongProcessAsync(progressIndicator, Cts.Token);  
}  

private void CancelEmployeesLongProcess()
{
	**Cts.Cancel();**
	CanLongProcess = true;
}
```

```
 public async Task<List<EmployeeModel>> GetEmployeeModelLongProcessAsync(IProgress<int> progress, CancellationToken cts){  
            await Task.Run(async () =>
            {
                int totalRecords = 100;
                for (int i = 0; i < totalRecords; i++)
                {
                    await Task.Delay(1000);
                    **cts.ThrowIfCancellationRequested();**
					if (progress != null)
                    {
                        **progress.Report(i * 100 / totalRecords);**
                    }
                }
            }, cts);
}  
```

## Progress of async operation  
IProgress<T> implementation is offered Natively by .Net. Here progress.Report will call the ReportProgress. ReportProgress will handle the UI.  

```
private async Task<List<EmployeeModel>> GetEmployeesLongProcessAsync()
{
  CanLongProcess = false;
  var progressIndicator = new Progress<int>(ReportProgress);
   Task<List<EmployeeModel>> task = _employeeService.GetEmployeeModelLongProcessAsync(progressIndicator, Cts.Token);
}

private void ReportProgress(int value)
{
	CurrentProgress = value;
}
```

Note: Always await in try catch block if the async operation needs the functionality of cancellation
