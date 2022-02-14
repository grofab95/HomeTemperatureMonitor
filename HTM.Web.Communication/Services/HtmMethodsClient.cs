using Grpc.Net.Client;
using HTM.Infrastructure;

namespace HTM.Web.Communication.Services;

public class HtmMethodsClient
{
    private readonly GrpcChannel _grpcChannel;
    
    public HtmMethodsClient()
    {
        _grpcChannel = GrpcChannel.ForAddress("localhost");
    }

    public Task<string> GetMessageByCommand(SerialPortCommand command)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            
        }

        return Task.FromResult("dupa");
    }
}