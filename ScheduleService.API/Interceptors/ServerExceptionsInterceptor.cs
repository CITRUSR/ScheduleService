using Grpc.Core;
using Grpc.Core.Interceptors;
using Newtonsoft.Json;
using ScheduleService.Application.Common.Exceptions;
using Serilog;

namespace ScheduleService.API.Interceptors;

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
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            switch (ex)
            {
                case ValidationException ve:
                    throw new RpcException(
                        new Status(
                            StatusCode.InvalidArgument,
                            JsonConvert.SerializeObject(ve.Errors)
                        )
                    );
                case NotFoundException notFoundEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.NotFound,
                            JsonConvert.SerializeObject(notFoundEx.Message)
                        )
                    );
                case AlreadyExistsException alreadyExistsEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.AlreadyExists,
                            JsonConvert.SerializeObject(alreadyExistsEx.Message)
                        )
                    );
                default:
                    throw new RpcException(
                        new Status(
                            StatusCode.Internal,
                            $"An unexpected error occurred: {ex.Message}"
                        )
                    );
            }
        }
    }
}
