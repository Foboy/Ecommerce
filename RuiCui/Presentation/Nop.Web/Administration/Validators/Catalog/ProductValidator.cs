using FluentValidation;
using Nop.Admin.Models.Catalog;
using Nop.Services.Localization;
using System.Text.RegularExpressions;

namespace Nop.Admin.Validators.Catalog
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Products.Fields.Name.Required"));
            RuleFor(x => x.Sku).Matches(@"^[a-z|A-Z|0-9]{0,12}$").WithMessage("商品编码必须是1-12数字和字母的组合");
        }
    }
}