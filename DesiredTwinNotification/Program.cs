using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesiredTwinNotification
{
  class Program
  {
    private const string DeviceConnectionString = "";

    static async Task Main(string[] args)
    {
      var deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);
      await deviceClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyChangedAsync, null);

      using var cts = new CancellationTokenSource();
      while (!cts.IsCancellationRequested)
      {
        Console.WriteLine("Waiting desired property changed...");
        await Task.Delay(1000, cts.Token);
      }
    }

    private static async Task OnDesiredPropertyChangedAsync(TwinCollection desiredProperties, object userContext)
    {
      var deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

      Console.WriteLine("\tSending current time as reported property");
      var reportedProperties = new TwinCollection
      {
        ["DateTimeLastDesiredPropertyChangeReceived"] = DateTime.UtcNow
      };

      await deviceClient.UpdateReportedPropertiesAsync(reportedProperties);
    }
  }
}
