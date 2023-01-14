using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Core.Commands;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.KubernetesCommands.Configuration;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Bootstrap;
public partial class PowerCommandsManager : IPowerCommandsManager
{
    public readonly IExtendedPowerCommandServices<PowerCommandsConfiguration> Services;
    public PowerCommandsManager(IExtendedPowerCommandServices<PowerCommandsConfiguration> services) { Services = services; }
    public void Run(string[] args)
    {
        var runFlow = new RunFlowManager(args);
        while (runFlow.CurrentRunResultStatus is not RunResultStatus.Quit)
        {
            try
            {
                RunCustomCode();
                var promptText = runFlow.CurrentRunResultStatus == RunResultStatus.Async ? "" : $"\n{ConfigurationGlobals.Prompt}";
                runFlow.Raw = runFlow.RunAutomatedAtStartup ? string.Join(' ', args) : ReadLine.ReadLineService.Service.Read(prompt: promptText);
                if (string.IsNullOrEmpty(runFlow.Raw.Trim())) continue;
                var interpretedInput = runFlow.Raw.Interpret();
                if (runFlow.RunAutomatedAtStartup)
                {
                    Services.Diagnostic.Message($"Started up with args: {interpretedInput.Raw}");
                    ConsoleService.Service.Write($"{nameof(PowerCommandsManager)}", ConfigurationGlobals.Prompt, null);
                    ConsoleService.Service.Write($"{nameof(PowerCommandsManager)} automated startup", $"{interpretedInput.Identifier}", ConsoleColor.Blue);
                    ConsoleService.Service.WriteLine($"{nameof(PowerCommandsManager)} automated startup", interpretedInput.Raw.Replace($"{interpretedInput.Identifier}",""), null);
                    interpretedInput = runFlow.InitializeRunAutomation(interpretedInput);
                }
                Services.Logger.LogInformation($"Console input Identifier:{interpretedInput.Identifier} raw:{interpretedInput.Raw}");
                Services.Diagnostic.Start();
                var runResult = Services.Runtime.ExecuteCommand($"{runFlow.Raw}");
                runFlow.CurrentRunResultStatus = runResult.Status;
                RunResultHandler(runResult);
                Services.Diagnostic.Stop();
                if (runFlow.RunOnceThenQuit) runFlow.CurrentRunResultStatus = RunResultStatus.Quit;
                if(string.IsNullOrEmpty(runResult.ContinueWith)) continue;
                runFlow.ContinueWith = runResult.ContinueWith;
                break;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                var commandsCommand = new CommandsCommand("commands", (Services.Configuration as CommandsConfiguration)!);
                var interpretedInput = runFlow.Raw.Interpret();
                ConsoleService.Service.WriteError(GetType().Name, $"Could not found any commands with a matching Id: {interpretedInput.Raw} and there is no defaultCommand defined in configuration or the defined defaultCommand does not exist.");
                commandsCommand.InitializeAndValidateInput(interpretedInput);
                commandsCommand.Run();
                Services.Logger.LogError(ex, "Could not found any commands with a matching Id");
            }
            catch (Exception e)
            {
                Services.Logger.LogError(e,"Unknown error");
                ConsoleService.Service.WriteError(GetType().Name, "Unknown error occurred, please try again");
            }
        }
        if (string.IsNullOrEmpty(runFlow.ContinueWith)) return;
        Run(new []{runFlow.ContinueWith });
    }
    private void RunResultHandler(RunResult runResult)
    {
        if(Services.Configuration.ShowDiagnosticInformation) Services.Logger.LogInformation($"Command {runResult.ExecutingCommand.Identifier} run with input: [{runResult.Input.Raw}] output: [{runResult.Output.Trim()}] status: [{runResult.Status}]");
        else Services.Logger.LogTrace($"Command {runResult.ExecutingCommand.Identifier} run with input: [{runResult.Input.Raw}] output: [{runResult.Output.Trim()}] status: [{runResult.Status}]");
        Services.Diagnostic.Message($"Input: {runResult.Input.Raw} Output: {runResult.Output} Status: {runResult.Status}");
        switch (runResult.Status)
        {
            case RunResultStatus.Quit:
                Services.Logger.LogInformation($"Command return status Quit, program execution ends...");
                break;
            case RunResultStatus.ArgumentError:
            case RunResultStatus.ExceptionThrown:
            case RunResultStatus.InputValidationError:
            case RunResultStatus.SyntaxError:
                var message = $"Error occurred of type {runResult.Status}";
                Services.Logger.LogError(message);
                ConsoleService.Service.WriteError(GetType().Name, $"{message} {runResult.Output}");
                HelpService.Service.ShowHelp(runResult.ExecutingCommand, clearConsole: false);
                break;
            case RunResultStatus.Continue:
                Services.Logger.LogInformation($"Command {runResult.Input.Identifier} continues with {runResult.ContinueWith}");
                break;
            case RunResultStatus.RunExternalPowerCommand:
            case RunResultStatus.Initializing:
            case RunResultStatus.Ok:
            case RunResultStatus.Help:
            default:
                break;
        }
    }
}