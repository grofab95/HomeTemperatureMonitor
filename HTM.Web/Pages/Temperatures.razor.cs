using System;
using System.Threading.Tasks;
using HTM.Infrastructure.Models;
using HTM.Web.Communication.Services;
using HTM.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace HTM.Web.Pages;

public partial class Temperatures
{
    [Inject] public HtmMethodsClient HtmMethodsClient { get; set; }

    private PleaseWaitComponent _pleaseWaitComponent;
    private TemperatureMeasurement[] _temperatureMeasurement;
    private DateTime _fromDate = DateTime.Now.AddMonths(-1);
    private DateTime _toDate = DateTime.Now;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        await FetchData();
    }

    private async Task FetchData()
    {
        _pleaseWaitComponent.Show();
        await Task.Delay(1000);
       
        _temperatureMeasurement = await HtmMethodsClient.GetTemperaturesMeasurements(
            DateTime.Now.AddMonths(-1), 
            DateTime.Now);
        
        _pleaseWaitComponent.Close();
        StateHasChanged();
    }
}