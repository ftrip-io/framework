using System;

namespace ftrip.io.framework.Utilities
{
    public static class EnvReader
    {
        public static string GetEnvVariableOrThrow(string envVariableName)
        {
            return Environment.GetEnvironmentVariable(envVariableName) ?? throw new Exception($"Environment variable {envVariableName} is not defined.");
        }
    }
}