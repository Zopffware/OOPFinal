public class DateCheckCommand : ICommand {
    public readonly string character;
    public DateCheckCommand(string character) {
        this.character = character;
    }
}
