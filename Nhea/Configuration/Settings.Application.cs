﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nhea.Configuration.GenericConfigSection.ApplicationSection;

namespace Nhea.Configuration
{
    public enum EnvironmentType
    {
        Development,
        Integration,
        Uat,
        Staging,
        Production
    }

    public static partial class Settings
    {
        /// <summary>
        /// Nhea Data Configuration Section Helper Class
        /// </summary>
        public static class Application
        {
            private static ApplicationConfigSection config = ConfigFactory.GetConfigurationSection<ApplicationConfigSection>("nhea/application");

            public static EnvironmentType EnvironmentType
            {
                get
                {
                    return config.EnvironmentType;
                }
            }

            public static string Name
            {
                get
                {
                    return config.Name;
                }
            }
        }
    }
}