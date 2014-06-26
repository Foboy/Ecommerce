using System;
using System.Globalization;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Tax;
using Nop.Services.Directory;
using Nop.Services.Localization;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Price formatter
    /// </summary>
    public partial class PriceFormatter : IPriceFormatter
    {
        private readonly IWorkContext _workContext;
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly TaxSettings _taxSettings;
        private readonly CurrencySettings _currencySettings;

        public PriceFormatter(IWorkContext workContext,
            ICurrencyService currencyService,
            ILocalizationService localizationService,
            TaxSettings taxSettings,
            CurrencySettings currencySettings)
        {
            this._workContext = workContext;
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._taxSettings = taxSettings;
            this._currencySettings = currencySettings;
        }

        #region Utilities

        /// <summary>
        /// Gets currency string
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <returns>Currency string without exchange rate</returns>
        protected string GetCurrencyString(decimal amount)
        {
            return GetCurrencyString(amount, true, _workContext.WorkingCurrency);
        }

        /// <summary>
        /// Gets currency string
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <returns>Currency string without exchange rate</returns>
        protected string GetCurrencyString(decimal amount,
            bool showCurrency, Currency targetCurrency)
        {
            if (targetCurrency == null)
                throw new ArgumentNullException("targetCurrency");

            string result = "";
            if (!String.IsNullOrEmpty(targetCurrency.CustomFormatting))
            {
                result = amount.ToString(targetCurrency.CustomFormatting);
            }
            else
            {
                if (!String.IsNullOrEmpty(targetCurrency.DisplayLocale))
                {
                    result = amount.ToString("C0",new CultureInfo(targetCurrency.DisplayLocale));
                }
                else
                {
                    result = String.Format("{0} ({1})", amount.ToString(), targetCurrency.CurrencyCode);
                    return result;
                }
            }

            if (showCurrency && _currencySettings.DisplayCurrencyLabel)
                result = String.Format("{0} ({1})", result, targetCurrency.CurrencyCode);
            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <returns>Price</returns>
        public string FormatPrice(decimal price)
        {
            return FormatPrice(price, true, _workContext.WorkingCurrency);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <returns>Price</returns>
        public string FormatPrice(decimal price, bool showCurrency, Currency targetCurrency)
        {
            bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return FormatPrice(price, showCurrency, targetCurrency, _workContext.WorkingLanguage, priceIncludesTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public string FormatPrice(decimal price, bool showCurrency, bool showTax)
        {
            bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return FormatPrice(price, showCurrency, _workContext.WorkingCurrency, _workContext.WorkingLanguage, priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <param name="language">Language</param>
        /// <returns>Price</returns>
        public string FormatPrice(decimal price, bool showCurrency,
            string currencyCode, bool showTax, Language language)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode);
            if (currency == null)
            {
                currency = new Currency();
                currency.CurrencyCode = currencyCode;
            }
            bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return FormatPrice(price, showCurrency, currency, language, priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public string FormatPrice(decimal price, bool showCurrency,
            string currencyCode, Language language, bool priceIncludesTax)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode) ?? new Currency() { CurrencyCode = currencyCode };
            return FormatPrice(price, showCurrency, currency, language, priceIncludesTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public string FormatPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax)
        {
            return FormatPrice(price, showCurrency, targetCurrency, language, 
                priceIncludesTax, _taxSettings.DisplayTaxSuffix);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public string FormatPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax, bool showTax)
        {
            //round before rendering
            price = Math.Round(price, 0);
            
            string currencyString = GetCurrencyString(price, showCurrency, targetCurrency);
            if (showTax)
            {
                //show tax suffix
                string formatStr;
                if (priceIncludesTax)
                {
                    formatStr = _localizationService.GetResource("Products.InclTaxSuffix", language.Id, false);
                    if (String.IsNullOrEmpty(formatStr))
                        formatStr = "{0} incl tax";
                }
                else
                {
                    formatStr = _localizationService.GetResource("Products.ExclTaxSuffix", language.Id, false);
                    if (String.IsNullOrEmpty(formatStr))
                        formatStr = "{0} excl tax";
                }
                return string.Format(formatStr, currencyString);
            }
            else
                return currencyString;
        }


        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <returns>Price</returns>
        public string FormatShippingPrice(decimal price, bool showCurrency)
        {
            bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return FormatShippingPrice(price, showCurrency, _workContext.WorkingCurrency, _workContext.WorkingLanguage, priceIncludesTax);
        }

        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public string FormatShippingPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax)
        {
            bool showTax = _taxSettings.ShippingIsTaxable && _taxSettings.DisplayTaxSuffix;
            return FormatShippingPrice(price, showCurrency, targetCurrency, language, priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public string FormatShippingPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax, bool showTax)
        {
            return FormatPrice(price, showCurrency, targetCurrency, language, priceIncludesTax, showTax);
        }
        
        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public string FormatShippingPrice(decimal price, bool showCurrency, 
            string currencyCode, Language language, bool priceIncludesTax)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode) ?? new Currency() { CurrencyCode = currencyCode };
            return FormatShippingPrice(price, showCurrency, currency, language, priceIncludesTax);
        }



        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <returns>Price</returns>
        public string FormatPaymentMethodAdditionalFee(decimal price, bool showCurrency)
        {
            bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            return FormatPaymentMethodAdditionalFee(price, showCurrency, _workContext.WorkingCurrency, 
                _workContext.WorkingLanguage, priceIncludesTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public string FormatPaymentMethodAdditionalFee(decimal price, bool showCurrency,
            Currency targetCurrency, Language language, bool priceIncludesTax)
        {
            bool showTax = _taxSettings.PaymentMethodAdditionalFeeIsTaxable && _taxSettings.DisplayTaxSuffix;
            return FormatPaymentMethodAdditionalFee(price, showCurrency, targetCurrency, language, priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public string FormatPaymentMethodAdditionalFee(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax, bool showTax)
        {
            return FormatPrice(price, showCurrency, targetCurrency, language, 
                priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public string FormatPaymentMethodAdditionalFee(decimal price, bool showCurrency, 
            string currencyCode, Language language, bool priceIncludesTax)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode) ?? new Currency() {CurrencyCode = currencyCode};
            return FormatPaymentMethodAdditionalFee(price, showCurrency, currency, 
                language, priceIncludesTax);
        }



        /// <summary>
        /// Formats a tax rate
        /// </summary>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Formatted tax rate</returns>
        public string FormatTaxRate(decimal taxRate)
        {
            return taxRate.ToString("G29");
        }

        #endregion
    }
}
