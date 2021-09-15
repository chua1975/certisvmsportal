namespace CertisVMSPortal.Helpers
{
    public class CertisVMSVersion
    { 
        public static string Current()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string displayableVersion = $"{version.Major}.{version.Minor} ({version.Build}.{version.Revision})";

            return displayableVersion;
        }
        
    }
}