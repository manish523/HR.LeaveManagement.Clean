namespace HR.LeaveManagement.BlazorUI.Services.Base
{
    public partial class Client : IClient
    {
        //public HttpClient HttpClient { get { return new HttpClient(); } }

        HttpClient IClient.HttpClient { get; set; }
    }
}
