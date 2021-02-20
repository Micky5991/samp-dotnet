using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Micky5991.Samp.Net.Framework.Interfaces
{
    public interface IStartup : ISampExtension
    {
        public void SetupConfiguration(IConfigurationBuilder configurationBuilder);

        public IEnumerable<ISampExtension> SetupExtensions(IConfiguration configuration);

        void Start(IServiceProvider serviceProvider, IConfiguration configuration);
    }
}
