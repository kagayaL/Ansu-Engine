namespace EditorModel;

// Результат операции редактора
// Для ожидаемых ошибок, которые юзер может исправить
public readonly record struct ActionResult(bool Succeeded, string? Error)
{
    public static ActionResult Ok()
    {
        return new ActionResult(true, null); // успех
    } 

    public static ActionResult Fail(string error)
    {
        return new ActionResult(false, error); // неудача с текстом для пользователя
    } 
}