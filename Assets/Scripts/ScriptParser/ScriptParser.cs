using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ScriptParser {
    private static string speaker = "";
    public static GameControl currentPrefab, nextPrefab;
    private static int commandIndex = 0;

    private static List<ICommand> currentScript;

    public static List<ICommand> parse(string script) {
        List<ICommand> commands = new List<ICommand>();
        bool textBlock = false;
        bool prompt = false;
        PromptCommand currentPrompt = null;
        string currentChoice = null;
        string currentConsequences = "";

        foreach (string line in script.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
            string[] lineData = line.Trim().Split('|');
            if (textBlock) {
                if (lineData[0].Equals("[end]")) {
                    textBlock = false;
                } else {
                    commands.Add(new TextCommand(lineData[0]));
                }
            } else if (prompt) {
                if (currentPrompt == null) {
                    currentPrompt = new PromptCommand();
                }
                if (currentChoice == null) {
                    if (lineData[0].Equals("[end]")) {
                        commands.Add(currentPrompt);
                        currentPrompt = null;
                        prompt = false;
                    } else {
                        currentChoice = lineData[0];
                    }
                } else {
                    if (lineData[0].Equals("[end]")) {
                        currentPrompt.addConsequences(currentChoice, parse(currentConsequences));
                        currentConsequences = "";
                        currentChoice = null;
                    } else {
                        currentConsequences += line + "\r\n";
                    }
                }
            } else {
                Command command;
                try {
                    command = (Command)(Enum.Parse(typeof(Command), lineData[0].ToUpper()));
                } catch (ArgumentException e) {
                    throw new ArgumentException("Invalid command: " + lineData[0]);
                }
                switch (command) {
                    case Command.SPEAKER:
                        commands.Add(new SpeakerCommand(lineData[1]));
                        break;
                    case Command.TEXT:
                        commands.Add(new TextCommand(lineData[1]));
                        break;
                    case Command.SPEAKERTEXT:
                        commands.Add(new SpeakerCommand(lineData[1]));
                        commands.Add(new TextCommand(lineData[2]));
                        break;
                    case Command.TEXTBLOCK:
                        textBlock = true;
                        break;
                    case Command.PORTRAIT:
                        commands.Add(new PortraitCommand(lineData[1], lineData[2][0]/*, lineData[3][0]*/));
                        break;
                    case Command.BACKGROUND:
                        commands.Add(new BackgroundCommand(lineData[1]));
                        break;
                    case Command.LINK:
                        commands.Add(new LinkCommand(lineData[1]));
                        break;
                    case Command.ADDPOINTS:
                        commands.Add(new AddPointsCommand(lineData[1], Int16.Parse(lineData[2])));
                        break;
                    case Command.PROMPT:
                        prompt = true;
                        break;
                }
            }
        }

        return commands;
    }

    public static void executeCommand(ICommand command) {
        if (command.GetType().Equals(typeof(SpeakerCommand))) {                                     //speaker
            SpeakerCommand speakerCommand = (SpeakerCommand)command;
            Text speaker = GameObject.Find("Speaker").GetComponentInChildren<Text>();
            if (speakerCommand.speaker.Equals("")) {
                speaker.color = Color.clear;
            } else {
                speaker.color = Color.white;
                speaker.text = speakerCommand.speaker;
            }
            advanceScript();

        } else if (command.GetType().Equals(typeof(TextCommand))) {                                 //text
            TextCommand textCommand = (TextCommand)command;
            GameObject.Find("Dialogue").GetComponentInChildren<Text>().text = textCommand.text;

        } else if (command.GetType().Equals(typeof(PortraitCommand))) {                             //portrait
            PortraitCommand portraitCommand = (PortraitCommand)command;
            Image portrait = GameObject.Find(portraitCommand.side == 'L' ? "LeftPortrait" : "RightPortrait").GetComponentInChildren<Image>();
            Debug.Log(portrait);
            if (portraitCommand.character.Equals("")) {
                portrait.color = Color.clear;
            } else {
                portrait.color = Color.white;
                portrait.sprite = Resources.Load<Sprite>("Characters\\" + portraitCommand.character + "Girl");
            }
            advanceScript();

        } else if (command.GetType().Equals(typeof(BackgroundCommand))) {                           //background
            BackgroundCommand backgroundCommand = (BackgroundCommand)command;
            GameObject.Find("Background").GetComponentInChildren<Image>().sprite =
                    Resources.Load<Sprite>("Backgrounds\\" + backgroundCommand.name);
            advanceScript();

        } else if (command.GetType().Equals(typeof(LinkCommand))) {                                 //link
            LinkCommand linkCommand = (LinkCommand)command;
            readScript(linkCommand.fileName);
            advanceScript();

        } else if (command.GetType().Equals(typeof(AddPointsCommand))) {                            //addpoints
            AddPointsCommand addPointsCommand = (AddPointsCommand)command;
            switch (addPointsCommand.character) {
                case "Java":
                    GameControl.control.JavaLovePoints += addPointsCommand.points;
                    break;
                case "JS":
                    GameControl.control.JSLovePoints += addPointsCommand.points;
                    break;
                case "C++":
                    GameControl.control.CPPLovePoints += addPointsCommand.points;
                    break;
                case "C#":
                    GameControl.control.CSLovePoints += addPointsCommand.points;
                    break;
                case "HTML":
                    GameControl.control.HTMLLovePoints += addPointsCommand.points;
                    break;
                case "Python":
                    GameControl.control.PYLovePoints += addPointsCommand.points;
                    break;
            }
            advanceScript();

        } else if (command.GetType().Equals(typeof(PromptCommand))) {                               //prompt
            PromptCommand promptCommand = (PromptCommand)command;
            //TODO: prompt user for choice, execute script according to choice
            throw new NotImplementedException();
        }
    }

    public static void readScript(string filename) {
        try {
            using (StreamReader sr = new StreamReader((filename.StartsWith("C:\\") || filename.StartsWith("/"))
                    ? filename : "CharacterScripts\\" + filename)) {
                currentScript = parse(sr.ReadToEnd());
                commandIndex = 0;
            }
        } catch (Exception e) {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }
    }

    public static void advanceScript() {
        if (commandIndex >= currentScript.Count - 1) {
            Debug.Log("The script has finished.");
        } else {
            executeCommand(currentScript[commandIndex++]);
        }
    }

	/*public static void oldParse(string script) {
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
	}*/
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