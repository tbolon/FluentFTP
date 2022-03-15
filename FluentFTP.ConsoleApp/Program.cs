using System;
using System.IO;

namespace FluentFTP
{
	public static class Program
	{
		public static int Main(string[] args)
		{
			if (args.Length != 5)
			{
				Console.WriteLine("Usage: FluentFtpConsoleApp [host] [username] [password] [local] [remote]");
				return 1;
			}

			var host = args[0];
			var username = args[1];
			var password = args[2];
			var local = args[3];

			if (!File.Exists(local))
			{
				throw new InvalidOperationException($"File {local} does not exist");
			}

			var remote = args[4];

			Helpers.FtpTrace.LogToConsole = true;

			using (var client = new FtpClient(host, username, password))
			{
				client.EncryptionMode = FtpEncryptionMode.Explicit;
				client.ValidateAnyCertificate = true;
				client.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
				client.Connect();



				var status = client.UploadFile(local, remote, FtpRemoteExists.NoCheck);


				if (status != FtpStatus.Success)
				{
					throw new InvalidOperationException($"Unable to transfer file: {status} : {client.LastReply.ErrorMessage}");
				}

				Console.WriteLine("Success");
			}

			return 0;
		}
	}
}