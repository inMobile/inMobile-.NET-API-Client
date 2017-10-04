namespace Sms.ApiClient.V2
{
	public static class ClientUtils
	{
		/// <summary>
		/// Version information for an assembly consists of the following four values:
		/// Major Version
		/// Minor Version 
		/// Build Number
		/// Revision
		/// </summary>
		private static string _versionNumber = null;
		public static string VersionNumber => _versionNumber ?? (_versionNumber = typeof(ClientUtils).Assembly.GetName().Version.ToString());
	}
}
