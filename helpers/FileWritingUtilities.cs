using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoderzoneGrapQLAPI.helpers
{
	public static class FileWritingUtilities
	{
		public static void WriteEmailToLocalFile(MailMessage mailMessage)
		{
			var data = new
			{
				Recipients = mailMessage.To.Select(x => x.Address),
				Subject = mailMessage.Subject,
				Body = mailMessage.Body,
			};

			var savePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Test", "Emails");
			var fileName = $"{data.Recipients.FirstOrDefault()}-{data.Subject}.json";

			Directory.CreateDirectory(savePath);
			File.WriteAllText(Path.Combine(savePath, fileName), JsonConvert.SerializeObject(data));
		}
	}
}
