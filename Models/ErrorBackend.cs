namespace frontendnet.Models;

public class BackendErrorResponse
{
    public List<BackendErrorDetail> errors { get; set; } = new();
}

public class BackendErrorDetail
{
    public string msg { get; set; }
    public string path { get; set; }
}