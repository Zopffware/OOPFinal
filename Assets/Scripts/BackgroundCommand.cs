public class BackgroundCommand : ICommand {
    public readonly string fileName;

    public BackgroundCommand(string fileName) {
        this.fileName = fileName;
    }
}