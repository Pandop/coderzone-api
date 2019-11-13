using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace CoderzoneGrapQLAPI.helpers
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	internal class EmailAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dispayName = validationContext.DisplayName;
			// convert object to string
			string stringValue = value != null ? value.ToString() : "";
			// do not validate any format when the value is empty, 'required' validator will deal with it
			if (string.IsNullOrEmpty(stringValue))
			{
				return ValidationResult.Success;
			}
			else
			{

				try
				{
					// By using this constructor, it's actually using the .net official logic to test if it's an email
					MailAddress m = new MailAddress(stringValue);

					return ValidationResult.Success;
				}
				catch (FormatException)
				{
					return new ValidationResult($"{dispayName} is not a valid email");
				}
			}
		}
	}
}