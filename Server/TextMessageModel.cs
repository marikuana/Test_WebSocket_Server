public class TextMessageModel
{
    private string text;

    public string Text
    {
        get => text;
        set => text = value;
    }

    public override string ToString()
    {
        return $"TextMessageModel {{ Text = {Text} }}";
    }
}