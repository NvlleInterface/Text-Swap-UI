
public class Response
{
    public Data Data { get; set; }
    public string[] Message { get; set; }
    public bool Error { get; set; }
    public int StatusCode { get; set; }
    public string Titre { get; set; }
}

public class Data
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Role { get; set; }
}
