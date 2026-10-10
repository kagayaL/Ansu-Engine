namespace Editor.Model;

// берет ActionResult и показывает в EditorStatus
// чтоб код не дублировать крч

public sealed class ActionResultDisplayer
{

    // показать результат в статусбар
    public void Show(EditorContext context, ActionResult res)
    {
        if (res.Succeeded)
            context.Status.Success("Ready");
        else
            context.Status.Error(res.Error ?? "Unknown mistake");
    }

}