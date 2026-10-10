namespace Editor.Services;
// Подробный лог в файл, тк статус бар - лишь короткое сообщение
public interface ILogger
{
    void Info(string message); // информационное сообщение
    void Error(string message); // сообщение об ошибке
}