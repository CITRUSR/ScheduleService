using Grpc.Core;
using Grpc.Core.Interceptors;
using Serilog;

namespace ScheduleService.API.Middlewares;

public class ServerExceptionsInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case RpcException rpcE:
                    Log.Error(rpcE.Status.Detail);
                    break;
            }

            throw;
        }
    }
}
