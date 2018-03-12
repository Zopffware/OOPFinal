public class SpeakerCommand : ICommand {
    public readonly string speaker;

    public SpeakerCommand(string speaker) {
        this.speaker = speaker;
    }
}