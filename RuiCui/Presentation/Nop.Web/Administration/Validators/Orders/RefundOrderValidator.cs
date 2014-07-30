using FluentValidation;
using Nop.Admin.Models.Orders;
namespace Nop.Admin.Validators.Orders
{
    public class RefundOrderValidator : AbstractValidator<RefundOrderModel>
    {
        public RefundOrderValidator() {
            RuleFor(x => x.RefundAmount).GreaterThanOrEqualTo(0).WithMessage("还款金额不能小于0");
            RuleFor(x => x.ChoseOrderItemIds).NotEmpty().WithMessage("至少选择一件商品");
        }
    }
}