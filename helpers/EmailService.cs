
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.helpers;

namespace CsharpReference.Services
{
	public class EmailAccount
	{
		public string Host { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string FromAddress { get; set; }
		public int Port { get; set; }
		public bool EnableSsl { get; set; }
		public string RedirectToAddress { get; set; }
		public bool BypassCertificateValidation { get; set; }
		public bool SaveToLocalFile { get; set; }
	}

	public class EmailEntity
	{
		public IEnumerable<string> To;
		public IEnumerable<string> Bcc;
		public IEnumerable<string> Cc;
		public string Body;
		public string Subject;
		public string CredentialUser;
		public string CredentialPassword;
		public string Host;
		public int? Port;
		public bool UseSsl;
		public readonly Dictionary<string, Stream> Attachments = new Dictionary<string, Stream>();
	}

	public class EmailService
	{

		public EmailService(IOptions<EmailAccount> emailAccount)
		{
			EmailAccount = emailAccount.Value;
		}

		private EmailAccount EmailAccount { get; }


		public async Task<bool> SendEmail(EmailEntity emailToSend)
		{
			var to = emailToSend.To;
			var cc = emailToSend.Cc;
			var bcc = emailToSend.Bcc;
			var body = emailToSend.Body;

			// redirect email
			if (!string.IsNullOrWhiteSpace(EmailAccount.RedirectToAddress))
			{
				var originalAddressInfo = "This email was originally send to <br/>" + to;
				originalAddressInfo += "This email was originally cc to <br/>" + cc;
				originalAddressInfo += "This email was originally Bcc to <br/>" + bcc;
				body = originalAddressInfo + body;

				to = EmailAccount.RedirectToAddress.Split(",");
				cc = null;
				bcc = null;
			}

			// Create the mail message
			var mailMessage = new MailMessage
			{
				Body = body,
				IsBodyHtml = true,
				From = new MailAddress(EmailAccount.FromAddress, EmailAccount.FromAddress, Encoding.UTF8),
				Subject = emailToSend.Subject,
				SubjectEncoding = Encoding.UTF8,
				Priority = MailPriority.Normal
			};

			// Add recipients
			mailMessage.To.AddRange(to.Select(address => new MailAddress(address)));
			if (cc != null)
			{
				mailMessage.CC.AddRange(cc.Select(address => new MailAddress(address)));
			}
			if (bcc != null)
			{
				mailMessage.Bcc.AddRange(bcc.Select(address => new MailAddress(address)));
			}

			var attachments = emailToSend.Attachments
				.Select(attachment => new Attachment(attachment.Value, attachment.Key));
			mailMessage.Attachments.AddRange(attachments);

			//send email
			ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
			var smtp = new SmtpClient
			{
				Credentials = new NetworkCredential(EmailAccount.Username, EmailAccount.Password),
				Host = EmailAccount.Host,
				EnableSsl = EmailAccount.EnableSsl
			};
			if (EmailAccount.Port > 0) smtp.Port = EmailAccount.Port;

			if (EmailAccount.SaveToLocalFile)
			{
				FileWritingUtilities.WriteEmailToLocalFile(mailMessage);
				return true;
			}

			await smtp.SendMailAsync(mailMessage);

			return true;
		}

		private bool CertificateValidationCallBack(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			// If the certificate is a valid, signed certificate, return true.
			return sslPolicyErrors == SslPolicyErrors.None || EmailAccount.BypassCertificateValidation;
		}

	}
}
