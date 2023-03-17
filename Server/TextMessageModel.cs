public class TextMessageModel
{
    private string text;

    public string Text
    {
        get => text;
        set => text = value;
    }
    public void ToUpper()
        => Text = Text.ToUpper();

    public void ToLower()
        => Text = Text.ToLower();

    public override string ToString()
    {
        return $"TextMessageModel {{ Text = {Text} }}";
    }
}