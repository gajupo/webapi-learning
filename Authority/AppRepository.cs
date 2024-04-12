namespace webapi_learning.Authority
{
    public static class AppRepository
    {
        static List<Application> _applications = new List<Application>
        {
            new Application
            {
                ApplicationId = 1,
                ApplicationName = "MVCWebApp",
                ClientId = "78974923749832749243",
                Secret = "847923479237497239",
                Scope = "read,write,delete"
            }
        };

        public static Application? GetApplicationById(string ClientId)
        {
            return _applications.FirstOrDefault(a => a.ClientId == ClientId);
        }
    }
}
