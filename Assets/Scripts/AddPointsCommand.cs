public class AddPointsCommand : ICommand {
    public readonly string character;
    public readonly int points;

    public AddPointsCommand(string character, int points) {
        this.character = character;
        this.points = points;
    }
}
