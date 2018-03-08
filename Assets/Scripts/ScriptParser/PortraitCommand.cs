public class PortraitCommand : ICommand {
    public readonly string character;
    public readonly string poseName;
    public readonly float x;

    public PortraitCommand(string character, string poseName,  float x) {
        this.character = character;
        this.poseName = poseName;
        this.x = x;
    }
}