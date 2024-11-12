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
                case ColorNotFoundException colorNotFoundEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.NotFound,
                            JsonConvert.SerializeObject(colorNotFoundEx.Message)
                        )
                    );
                case RoomNotFoundException roomNotFoundEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.NotFound,
                            JsonConvert.SerializeObject(roomNotFoundEx.Message)
                        )
                    );
                case SubjectNotFoundException subjectNotFoundEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.NotFound,
                            JsonConvert.SerializeObject(subjectNotFoundEx.Message)
                        )
                    );
                case WeekdayNotFoundException weekdayNotFoundEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.NotFound,
                            JsonConvert.SerializeObject(weekdayNotFoundEx.Message)
                        )
                    );
                case RoomNameAlreadyExistsException roomNameExistsEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.AlreadyExists,
                            JsonConvert.SerializeObject(roomNameExistsEx.Message)
                        )
                    );
                case ColorNameAlreadyExistsException colorNameExistsEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.AlreadyExists,
                            JsonConvert.SerializeObject(colorNameExistsEx.Message)
                        )
                    );
                case SubjectNameAlreadyExistsException subjectNameExistsEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.AlreadyExists,
                            JsonConvert.SerializeObject(subjectNameExistsEx.Message)
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
