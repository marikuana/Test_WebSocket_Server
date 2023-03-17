public class TextMessageModel
{
    public string Text { get; set; }

    public override string ToString()
    {
        return $"TextMessageModel {{ Text = {Text} }}";
    }
}