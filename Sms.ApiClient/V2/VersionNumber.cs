namespace Sms.ApiClient.V2
{
	public static class ClientUtils
	{
		private static string _versionNumber = null;

		public static string VersionNumber => _versionNumber ?? (_versionNumber = typeof(ClientUtils).Assembly.GetName().Version.ToString());
	}
}