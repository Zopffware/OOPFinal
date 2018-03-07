using System.Collections.Generic;

public class PromptCommand : ICommand {
    private Dictionary<string, List<ICommand>> choices = new Dictionary<string, List<ICommand>>();

    public Dictionary<string, List<ICommand>>.KeyCollection getChoices() {
        return choices.Keys;
    }
    public ICommand[] getConsequences(string choice) {
        return choices[choice].ToArray();
    }
    public void addConsequence(string choice, ICommand consequence) {
        choices[choice].Add(consequence);
    }
}