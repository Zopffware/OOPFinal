using System;

[Serializable]
public class PortraitCommand : ICommand {
    public readonly string character;
   // public readonly string poseName;
    public readonly char side;

    public PortraitCommand(string character,/* string poseName,*/ char side) {
        this.character = character;
        //this.poseName = poseName;
        if (side == 'L' || side == 'R') {
            this.side = side;
        } else {
            throw new ArgumentException("Side must be 'L' or 'R'");
        }
    }
}