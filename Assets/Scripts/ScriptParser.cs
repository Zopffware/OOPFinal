using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ScriptParser {
    public static string speaker = "";



	public static void oldParse(string script) {
        bool textBlock = false;
        bool prompt = false;
        int promptCount = 0;
        List<string> prompts = new List<string>();
        int promptSelection = 0;
        string promptCode = "";

        foreach (string line in script.Split('\n')) {
            string[] lineData = line.Trim().Split('|');
            if (!textBlock && !prompt) {
                Command command;
                try {
                    command = (Command)(Enum.Parse(typeof(Command), lineData[0].ToUpper()));
                } catch (ArgumentException e) {
                    throw new ArgumentException("Invalid command: " + lineData[0]);
                }
                switch (command) {
                    case Command.TEXT:
                        displayText(speaker, lineData[1]);
                        break;
                    case Command.SPEAKER:
                        speaker = lineData[1];
                        break;
                    case Command.SPEAKERTEXT:
                        speaker = lineData[1];
                        displayText(speaker, lineData[2]);
                        break;
                    case Command.TEXTBLOCK:
                        textBlock = true;
                        break;
                    case Command.PROMPT:
                        prompt = true;
                        promptCount = 0;
                        prompts = new List<string>();
                        break;
                    case Command.LINK:
                        oldParse(getFile(lineData[1]));
                        break;
                    case Command.PORTRAIT:
                        displayPortrait(characters.get(lineData[1]).getPose(lineData[2]), lineData[3]);
                        break;
                    case Command.BACKGROUND:
                        setBackground(lineData[1]);
                        break;
                    case Command.ADDPOINTS:
                        characters.get(lineData[1]).setPoints(characters.get(lineData[1]).getPoints() + lineData[2]);
                        break;
                }
            } else if (textBlock) {
                if (lineData[0].Equals("[end]")) {
                    textBlock = false;
                } else {
                    displayText(lineData[0]);
                }
            } else if (prompt) {
                if (lineData[0].Equals("[end]")) {
                    if (promptCount == 0) {
                        promptSelection = promptUserChoice(prompts);
                    } else if (promptSelection == promptCount) {
                        oldParse(promptCode.TrimEnd('\n'));
                    }
                    promptCount++;
                    if (promptCount > prompts.Count) {
                        prompt = false;
                    }
                } else if (promptCount == 0) {
                    prompts.Add(lineData[0]);
                } else if (promptSelection == promptCount) {
                    promptCode += line + '\n';
                }
            }
        }
	}
	/*public static void displayText(string character, string text) {
		string displayText;
		if (character.Equals("Python")) {
			displayText = new Regex("sh?").Replace(text, "th");
		} else {
			displayText = text;
		}

	}*/

    private enum Command {
        TEXTBLOCK,
        TEXT,
        SPEAKERTEXT,
        SPEAKER,
        PROMPT,
        LINK,
        PORTRAIT,
        BACKGROUND,
        ADDPOINTS
    }
}