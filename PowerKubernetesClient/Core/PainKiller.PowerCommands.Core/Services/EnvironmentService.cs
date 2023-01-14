namespace PainKiller.PowerCommands.Core.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        private EnvironmentService() { }

        private static readonly Lazy<IEnvironmentService> Lazy = new(() => new EnvironmentService());
        public static IEnvironmentService Service => Lazy.Value;
        public string GetEnvironmentVariable(string variableName, bool decrypt = false, EnvironmentVariableTarget target = EnvironmentVariableTarget.User)
        {
            var retVal = Environment.GetEnvironmentVariable(variableName, target) ?? "";
            if (decrypt) retVal = EncryptionService.Service.DecryptString(retVal);
            return retVal;
        }
        public void SetEnvironmentVariable(string variableName, string inputValue, bool encrypt = false, EnvironmentVariableTarget target = EnvironmentVariableTarget.User)
        {
            var val = encrypt ? EncryptionService.Service.DecryptString(inputValue) : inputValue;
            Environment.SetEnvironmentVariable(variableName, val, target);
        }
    }
}