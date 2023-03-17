public class TextMessageModel
{
    public int Length => Text.Length;
    public string Text { get; set; }

    public void ToUpper()
        => Text = Text.ToUpper();

    public void ToLower()
        => Text = Text.ToLower();

    public void Reverse()
    {
        char[] chars = Text.ToCharArray();
        Array.Reverse(chars);
        Text = new string(chars);
    }

    public override string ToString()
    {
        return $"TextMessageModel {{ Text = {Text} }}";
    }
}