using Grpc.Core;
using Newtonsoft.Json;
using ScheduleService.Application.Common.Exceptions;

namespace ScheduleService.API.Middlewares;

public class GrcpExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case ValidationException valEx:
                    throw new RpcException(
                        new Status(
                            StatusCode.InvalidArgument,
                            JsonConvert.SerializeObject(new { valEx.Errors })
                        )
                    );
            }
        }
    }
}
