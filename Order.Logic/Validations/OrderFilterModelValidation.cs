using FluentValidation;
using Order.Logic.Enum;
using Order.Logic.Model;

namespace Order.Logic.Validations
{
    public class OrderFilterModelValidation : AbstractValidator<OrderFilterModel>
    {
        public OrderFilterModelValidation()
        {
            RuleFor(x => x).NotNull().WithMessage("Geçersiz kayıt.");
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber değeri 1 den küçük olamaz.");
            RuleForEach(x => x.Statuses).Must(x => System.Enum.IsDefined(typeof(OrderStatusEnum), x)).WithMessage("Geçersiz Status değeri.");
            RuleFor(x => x.StartDate).Must(x => x.HasValue).When(i => i.EndDate.HasValue).WithMessage("Başlangıç Tarihi giriniz.");
            RuleFor(x => x.EndDate).Must(x => x.HasValue).When(i => i.StartDate.HasValue).WithMessage("Bitiş Tarihi giriniz."); 
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(i => i.StartDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue).WithMessage("Başlangıç Tarihi Bitiş Tarihinden küçük olamaz.");
        }
    }
}
