using System;

[Serializable]
public class BackgroundCommand : ICommand {
    public readonly string name;

    public BackgroundCommand(string name) {
        this.name = name;
    }
}