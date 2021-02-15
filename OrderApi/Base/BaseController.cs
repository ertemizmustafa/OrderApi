using Microsoft.AspNetCore.Mvc;
using Order.Logic.Model;

namespace OrderApi.Base
{
    public class BaseController : ControllerBase
    {
        protected new IActionResult Ok()
        {
            return base.Ok(ResponseModel.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(ResponseModel.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return base.Ok(ResponseModel.Error(errorMessage));
        }

        protected IActionResult FromResult<T>(ResponseModel<T> responseModel)
        {
            return responseModel.IsSuccessfull ? Ok(responseModel.Result) : Error(responseModel.Message);
        }
    }
}
