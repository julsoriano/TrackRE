using System.Configuration;

namespace TrackRE.ServicesClient
{
    public static class Util
    {
        public static string getRootUri()
        {
            // For IIS Express, use localhost:44301 
            // var uri = "https://localhost:44301/";
            // Get the root URI from Web.config
            var uri = Configuration.PropertyServiceURI;
            return uri;
        }

        public static string getServiceUri(string srv)
        {
            return getRootUri() + "api/" + srv;
            //return "api/" + srv;
        }
    }

    public static class Configuration
    {
        private static string _uri = null;

        public static string PropertyServiceURI
        {
            get
            {
                if (!string.IsNullOrEmpty(_uri))
                    return _uri;

                _uri = getKeyVal("TrackREUri");
                if (string.IsNullOrEmpty(_uri))
                    return "https://localhost:44301/";
                else
                    return _uri;
            }
        }
        public static string getKeyVal(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}