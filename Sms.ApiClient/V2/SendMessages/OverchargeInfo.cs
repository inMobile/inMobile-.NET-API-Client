using System;

namespace Sms.ApiClient.V2.SendMessages
{
	public class OverchargeInfo
	{
		/// <summary>
		/// The price in danish øre, e.g. for 2 DKK, specify 200.
		/// </summary>
		public int Price { get; private set; }

		/// <summary>
		/// The country code for the short code number, e.g. "45" if the short number is "1245"
		/// </summary>
		public string CountryCode { get; set; }

		/// <summary>
		/// The shortcode number, e.g. "1245"
		/// </summary>
		public string ShortCodeNumber { get; set; }

		/// <summary>
		/// The type of overcharge.
		/// </summary>
		public OverchargeType Type { get; set; }

		/// <summary>
		/// The text to be displayed on the end users phone bill
		/// </summary>
		public string InvoiceDescription { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="overchargePrice">The price in danish øre, e.g. for 2 DKK, specify 200.</param>
		/// <param name="shortCodeCountryCode">The country code for the short code number, e.g. "45" if the short number is "1245"</param>
		/// <param name="shortCodeNumber">The shortcode number, e.g. "1245"</param>
		/// <param name="overchargeType">The type of overcharge.</param>
		/// <param name="invoiceDescription">The text to be displayed on the end users phone bill</param>
		public OverchargeInfo(int overchargePrice, string shortCodeCountryCode, string shortCodeNumber, OverchargeType overchargeType, string invoiceDescription)
		{
			if (overchargePrice <= 0)
				throw new ArgumentException("overchargePrice must be positive");
			if (string.IsNullOrEmpty(shortCodeCountryCode))
				throw new ArgumentNullException(shortCodeCountryCode);
			if (string.IsNullOrEmpty(shortCodeNumber))
				throw new ArgumentNullException(shortCodeNumber);

			Price = overchargePrice;
			CountryCode = shortCodeCountryCode;
			ShortCodeNumber = shortCodeNumber;
			Type = overchargeType;
			InvoiceDescription = invoiceDescription ?? "";
		}
	}
}