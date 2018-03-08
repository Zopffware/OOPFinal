using System.Collections.Generic;

public class PromptCommand : ICommand {
    private Dictionary<string, List<ICommand>> choices = new Dictionary<string, List<ICommand>>();

    public Dictionary<string, List<ICommand>>.KeyCollection getChoices() {
        return choices.Keys;
    }
    public void addChoice(string choice) {
        choices.Add(choice, new List<ICommand>());
    }
    public ICommand[] getConsequences(string choice) {
        return choices[choice].ToArray();
    }
    public void addConsequences(string choice, List<ICommand> consequences) {
        choices.Add(choice, consequences);
    }
}