// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Consoles.Base;

internal class CommandNode
{
    public string Name { get; private set; }

    public IReadOnlyCollection<CommandNode> Commands => commands;

    private readonly List<CommandNode> commands = new();

    public CommandNode(string name)
    {
        Name = name;
    }

    public void AddCommand(CommandNode command) => commands.Add(command);

    public bool NodeExists(string name)
    {
        foreach (CommandNode node in commands)
        {
            if (node.Name == name)
            {
                return true;
            }

            if (node.Commands.Any())
            {
                return node.NodeExists(name);
            }
        }

        return false;
    }
}
