using System.Collections.Generic;
using AutoMapper;
using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Events.Players;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Commands.Services
{
    /// <inheritdoc />
    public class CommandService : ICommandService
    {
        private readonly IMapper mapper;

        private readonly IEventAggregator eventAggregator;

        private readonly ILogger<CommandService> logger;

        private readonly IEnumerable<ICommandHandler> commandHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper that converts parameters to actual values.</param>
        /// <param name="eventAggregator">EventAggregator that listens to certain command events.</param>
        /// <param name="logger">Logger used in this service.</param>
        /// <param name="commandHandlers">Commandhandlers to register into this command service.</param>
        public CommandService(IMapper mapper, IEventAggregator eventAggregator, ILogger<CommandService> logger, IEnumerable<ICommandHandler> commandHandlers)
        {
            this.mapper = mapper;
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.commandHandlers = commandHandlers;
        }

        /// <inheritdoc/>
        public void Start()
        {
            this.eventAggregator.Subscribe<PlayerCommandEvent>(this.OnPlayerCommand, threadTarget: ThreadTarget.PublisherThread);
        }

        private void OnPlayerCommand(PlayerCommandEvent eventdata)
        {
            eventdata.Cancelled = true;

            // void ApplyContext(IMappingOperationOptions options)
            // {
            //     options.Items[CommandConstants.CommandExecutorKey] = eventdata.Player;
            // }

            // var vehicle = this.mapper.Map(eventdata.CommandText, typeof(string), typeof(Vehicle), ApplyContext);

            // this.logger.LogInformation($"Player: {eventdata.}");
        }
    }
}
