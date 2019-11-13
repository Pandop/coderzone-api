using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.helpers.utils
{
	interface ICertificateProvider
	{
		X509Certificate2 ReadX509SigningCert();
	}
	public class CertificateSetting
	{
		public string CertFileName { get; set; }
		public string PrivateKeyPWD { get; set; }
		public string JwtBearerAuthority { get; set; }

		public string JwtBearerAudience { get; set; }
	}

	public abstract class BaseCertificateProvider : ICertificateProvider
	{
		public BaseCertificateProvider(CertificateSetting certSetting)
		{
			CertificateSetting = certSetting;
		}

		protected CertificateSetting CertificateSetting { get; }

		public abstract X509Certificate2 ReadX509SigningCert();
	}
}
