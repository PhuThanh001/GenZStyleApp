﻿using Newtonsoft.Json;
using ProjectParticipantManagement.BAL.Errors;
using ProjectParticipantManagement.BAL.Exceptions;

namespace GenZStyleApp_API.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        public ExceptionMiddleware()
        {
            //define logging
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                //logging
                await next(context);
            }
            catch (Exception ex)
            {
                //logging
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
            switch (ex)
            {
                case NotFoundException _:
                    context.Response.StatusCode = (int)StatusCodes.Status404NotFound;
                    break;
                case BadRequestException _:
                    context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                    break;
            }

            Error error = new Error()
            {
                StatusCode = context.Response.StatusCode,
                Message = JsonConvert.DeserializeObject<List<ErrorDetail>>(ex.Message)
            };



            await context.Response.WriteAsync(error.ToString());
        }
    }
}
