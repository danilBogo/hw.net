namespace TimMovie.Core.Classes
{
    public class MailSetup
    {
        public MailSetup()
        {
            
        }
        public MailSetup(int port, string host, string password, string fromCompanyName, string fromCompanyAddress)
        {
            Port = port;
            Host = host;
            Password = password;
            FromCompanyName = fromCompanyName;
            FromCompanyAddress = fromCompanyAddress;
        }

        public int Port { get; init; }
        public string Host { get; init; }
        public string Password { get; init; }
        public string FromCompanyName { get; init; }
        public string FromCompanyAddress { get; init; }
    
    }
}