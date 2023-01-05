namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IEnvironmentService
{
    string GetEnvironmentVariable(string variableName, bool decrypt = false, EnvironmentVariableTarget target = EnvironmentVariableTarget.User);
    void SetEnvironmentVariable(string variableName, string inputValue, bool encrypt = false, EnvironmentVariableTarget target = EnvironmentVariableTarget.User);
}