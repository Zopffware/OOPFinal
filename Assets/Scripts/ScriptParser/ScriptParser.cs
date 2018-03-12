using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScriptParser : MonoBehaviour {
    private static string currentSpeaker = "";
    private static int commandIndex = 0;
    private static List<ICommand> currentScript = new List<ICommand>();
    private static bool isPrompting = false;

    public static List<ICommand> parse(string script) {
        List<ICommand> commands = new List<ICommand>();
        bool textBlock = false;
        bool prompt = false;
        PromptCommand currentPrompt = null;
        string currentChoice = null;
        string currentConsequences = "";
        foreach (string line in script.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
            if (!string.IsNullOrEmpty(line.Trim())) {
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
                        case Command.DATECHECK:
                            commands.Add(new DateCheckCommand(lineData[1]));
                            break;
                        case Command.FINALCHECK:
                            commands.Add(new FinalCheckCommand());
                            break;
                    }
                }
            }
        }

        return commands;
    }

    public static void executeCommand(ICommand command) {
        if (command.GetType().Equals(typeof(SpeakerCommand))) {                                     //speaker
            SpeakerCommand speakerCommand = (SpeakerCommand)command;
            currentSpeaker = speakerCommand.speaker;
            GameObject speakerBox = GameObject.Find("Speaker");
            Text speakerText = speakerBox.GetComponentInChildren<Text>();
            Image speakerImage = speakerBox.GetComponentInChildren<Image>();
            if (speakerCommand.speaker.Equals("")) {
                speakerText.color = Color.clear;
                speakerImage.color = Color.clear;
            } else {
                speakerText.color = Color.white;
                speakerImage.color = new Color(1f, 1f, 1f, 0.4f);
                speakerText.text = speakerCommand.speaker;
            }
            advanceScript();

        } else if (command.GetType().Equals(typeof(TextCommand))) {                                 //text
            TextCommand textCommand = (TextCommand)command;
            GameObject.Find("Dialogue").GetComponentInChildren<Text>().text = textCommand.text;

        } else if (command.GetType().Equals(typeof(PortraitCommand))) {                             //portrait
            PortraitCommand portraitCommand = (PortraitCommand)command;
            Image portrait = GameObject.Find(portraitCommand.side == 'L' ? "LeftPortrait" : "RightPortrait").GetComponentInChildren<Image>();
            if (portraitCommand.character.Equals("")) {
                portrait.color = Color.clear;
            } else {
                portrait.color = Color.white;
                portrait.sprite = Resources.Load<Sprite>("Characters\\" + portraitCommand.character + "Girl");
            }
            advanceScript();

        } else if (command.GetType().Equals(typeof(BackgroundCommand))) {                           //background
            BackgroundCommand backgroundCommand = (BackgroundCommand)command;
            Image background = GameObject.Find("Background").GetComponentInChildren<Image>();
            if (backgroundCommand.name.Equals("")) {
                background.color = Color.black;
            } else {
                background.color = Color.white;
                background.sprite = Resources.Load<Sprite>("Backgrounds\\" + backgroundCommand.name);
            }
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
                case "JSHTML":
                    GameControl.control.JSHTMLLovePoints += addPointsCommand.points;
                    break;
                case "C++":
                    GameControl.control.CPPLovePoints += addPointsCommand.points;
                    break;
                case "C#":
                    GameControl.control.CSLovePoints += addPointsCommand.points;
                    break;
                case "Python":
                    GameControl.control.PYLovePoints += addPointsCommand.points;
                    break;
            }
            advanceScript();

        } else if (command.GetType().Equals(typeof(PromptCommand))) {                               //prompt
            PromptCommand promptCommand = (PromptCommand)command;
            isPrompting = true;
            int i = 0;
            List<GameObject> promptButtons = new List<GameObject>();
            foreach (string choice in promptCommand.getChoices()) {
                GameObject promptButton = Instantiate(Resources.Load<GameObject>("Prefabs\\PromptButton"),
                        new Vector3(0, 0), Quaternion.identity);
                promptButtons.Add(promptButton);
                promptButton.transform.SetParent(GameObject.Find("Background").transform);
                promptButton.transform.position = new Vector3(Screen.width / 2, (Screen.height - 55) - i * 40);
                promptButton.GetComponentInChildren<Text>().text = choice;
                promptButton.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                    isPrompting = false;
                    foreach (GameObject button in promptButtons) {
                        Destroy(button);
                    }
                    currentScript.InsertRange(commandIndex, promptCommand.getConsequences(choice));
                    advanceScript();
                });
                i++;
            }
        } else if (command.GetType().Equals(typeof(DateCheckCommand))) {                            //datecheck
            DateCheckCommand dateCheckCommand = (DateCheckCommand)command;
            switch (dateCheckCommand.character) {
                case "Java":
                    if (GameControl.control.JavaLovePoints >= 15) {
                        //date start
                    }
                    break;
                case "JSHTML":
                    if (GameControl.control.JSHTMLLovePoints >= 15) {
                        //date start
                    }
                    break;
                case "C++":
                    if (GameControl.control.CPPLovePoints >= 15) {
                        //date start
                    }
                    break;
                case "C#":
                    if (GameControl.control.CSLovePoints >= 15) {
                        //date start
                    }
                    break;
                case "Python":
                    if (GameControl.control.PYLovePoints >= 15) {
                        //date start
                    }
                    break;
            }
        } else if (command.GetType().Equals(typeof(FinalCheckCommand))) {                           //finalcheck
            FinalCheckCommand finalCheckCommand = (FinalCheckCommand)command;
            PromptCommand promptCommand = new PromptCommand();
            if (GameControl.control.JavaLovePoints >= 15) {
                List<ICommand> commandList = new List<ICommand>();
                commandList.Add(new LinkCommand("\\Home\\Narrator\\Endings\\Java.txt"));
                promptCommand.addConsequences("Java", commandList);
            }
            if (GameControl.control.JSHTMLLovePoints >= 15) {
                List<ICommand> commandList = new List<ICommand>();
                commandList.Add(new LinkCommand("\\Home\\Narrator\\Endings\\HTMLJS.txt"));
                promptCommand.addConsequences("HTML & JS", commandList);
            }
            if (GameControl.control.CPPLovePoints >= 15) {
                List<ICommand> commandList = new List<ICommand>();
                commandList.Add(new LinkCommand("\\Home\\Narrator\\Endings\\C++.txt"));
                promptCommand.addConsequences("C++", commandList);
            }
            if (GameControl.control.CSLovePoints >= 15) {
                List<ICommand> commandList = new List<ICommand>();
                commandList.Add(new LinkCommand("\\Home\\Narrator\\Endings\\C#.txt"));
                promptCommand.addConsequences("C#", commandList);
            }
            if (GameControl.control.PYLovePoints >= 15) {
                List<ICommand> commandList = new List<ICommand>();
                commandList.Add(new LinkCommand("\\Home\\Narrator\\Endings\\Python.txt"));
                promptCommand.addConsequences("Python", commandList);
            }
            List<ICommand> noneCommandList = new List<ICommand>();
            noneCommandList.Add(new LinkCommand("\\Home\\Narrator\\Endings\\None.txt"));
            promptCommand.addConsequences("None", noneCommandList);
            currentScript.Insert(commandIndex, promptCommand);
            advanceScript();
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
        if (!isPrompting) {
            if (commandIndex >= currentScript.Count) {
                Debug.Log("The script has finished.");
            } else {
                executeCommand(currentScript[commandIndex++]);
            }
        }
    }

    public static List<ICommand> getCurrentScript() {
        return currentScript;
    }
    public static int getCommandIndex() {
        return commandIndex;
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
        ADDPOINTS,
        DATECHECK,
        FINALCHECK
    }
}