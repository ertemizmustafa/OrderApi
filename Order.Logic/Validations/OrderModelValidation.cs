using FluentValidation;
using Order.Logic.Common;
using Order.Logic.Model;
namespace Order.Logic.Validations
{
    public class OrderModelValidation : AbstractValidator<OrderModel>
    {
        public OrderModelValidation()
        {
            RuleFor(x => x).NotNull().WithMessage("Geçersiz kayıt.");
            RuleFor(x => x.Orders).Must(CommonExtensions.ContainItem).WithMessage("Geçersiz kayıt.");
            RuleFor(x => x.Orders).ForEach(x => x.SetValidator(new OrderInfoModelValidation())).When(x => x.Orders.ContainItem());
        }
    }
}
