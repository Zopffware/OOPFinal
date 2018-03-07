public class LinkCommand : ICommand {
    public readonly string fileName;
    
    public LinkCommand(string fileName) {
        this.fileName = fileName;
    }
}