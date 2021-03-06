﻿namespace Auditable.Writers.File
{
    using System;
    using Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class File : IWriterProvider, ISetupOptions<FileWriterOptions>
    {
        Action<FileWriterOptions> _options = options => { };

        public void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IWriter, FileWriter>();
            services.Configure(_options);
        }

        public void Setup(Action<FileWriterOptions> setup)
        {
            _options = setup;
        }
    }
}