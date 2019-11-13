using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.helpers.utils
{
	public class InRootFolderCertificateProvider : BaseCertificateProvider
	{
		public InRootFolderCertificateProvider(CertificateSetting certSetting) : base(certSetting)
		{

		}

		public override X509Certificate2 ReadX509SigningCert()
		{
			var certFileName = CertificateSetting.CertFileName;
			var privateKeyPWD = CertificateSetting.PrivateKeyPWD;
			var fileFullPath = Path.Join(Directory.GetCurrentDirectory(), certFileName);
			if (File.Exists(fileFullPath))
			{
				return new X509Certificate2(fileFullPath, privateKeyPWD);
			}
			else
			{
				return null;
			}
		}
	}
}
