using System;

namespace ftrip.io.framework.messaging.Installers
{
    [Flags]
    public enum RabbitMQInstallerType
    {
        None = 0,
        Publisher = 1,
        Consumer = 2
    }

    public static class RabbitMQInstallerTypeExtensions
    {
        public static bool Is(this RabbitMQInstallerType type, RabbitMQInstallerType secondType)
        {
            return type.Equals(secondType);
        }

        public static bool IsIn(this RabbitMQInstallerType type, RabbitMQInstallerType types)
        {
            return (types & type) == type;
        }

        public static bool Has(this RabbitMQInstallerType types, RabbitMQInstallerType type)
        {
            return (types & type) == type;
        }
    }
}