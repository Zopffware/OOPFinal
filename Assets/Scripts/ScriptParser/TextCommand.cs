public class TextCommand : ICommand {
    public readonly string text;

    public TextCommand(string text) {
        this.text = text;
    }
}