using FluentValidation;
using Order.Logic.Enum;
using Order.Logic.Model;

namespace Order.Logic.Validations
{
    public class OrderInfoModelValidation : AbstractValidator<OrderInfoModel>
    {
        public OrderInfoModelValidation()
        {
            RuleFor(x => x).NotNull().WithMessage("Geçersiz kayıt.");
            RuleFor(x => x.Status).Must(x => System.Enum.IsDefined(typeof(OrderStatusEnum), x)).WithMessage("Geçersiz Status değeri.");
        }
    }
}
