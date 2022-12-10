namespace HomeTools.Models;

public class ServerResponse<ResponseType>
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public ResponseType Data { get; set; }
}

public static class ServerResponseMessages
{
    public static ServerResponse<ResponseType> Ok<ResponseType>(ResponseType data) => 
        new ServerResponse<ResponseType>() {Status = true, Data = data};
    
    public static ServerResponse<object> Error(string message = "") => 
        new ServerResponse<object>() {Status = false, Message = message};
}