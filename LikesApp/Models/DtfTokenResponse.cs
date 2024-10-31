namespace LikesApp;

public class DtfTokenResponse
{
    public string Message {  get; set; }

    public DataModel Data { get; set; }

    public class DataModel
    {
        public string AccessToken { get; set; }

        public long AccessExpTimestamp { get; set; }
    }
}
