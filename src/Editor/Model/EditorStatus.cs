namespace EditorModel;

// строка статуса для юзера
// короткое сообщение о последнем действии в редакторе 
public class EditorStatus()
{
    public string Message { get; private set; } = "Готово";

    public bool IsError { get; private set; }
    public void Success(string message)
    {
        Message = message;
        IsError = false;
    }

    public void Error(string message)
    {
        Message = message;
        IsError = true;
    }
}