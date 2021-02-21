using System;
using System.Threading.Tasks;
using Dawn;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Dialogs;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Services
{
    /// <inheritdoc />
    public class DialogHandler : IDialogHandler
    {
        private const string CurrentDialogKey = "SAMPNET_CURRENT_DIALOG";
        private const string LastDialogIdKey = "SAMPNET_LAST_DIALOG_ID";

        private readonly IEventAggregator eventAggregator;

        private readonly ILogger<DialogHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogHandler"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator that publishes needed events.</param>
        /// <param name="logger">Logger needed for this handler.</param>
        public DialogHandler(IEventAggregator eventAggregator, ILogger<DialogHandler> logger)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;
        }

        /// <inheritdoc />
        public void Attach()
        {
            this.eventAggregator.Subscribe<PlayerDialogResponseEvent>(this.OnPlayerDialogResponse);
        }

        /// <inheritdoc />
        public async Task<DialogResponseData> ShowDialogAsync(IPlayer player, IDialog dialog)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Argument(dialog, nameof(dialog)).NotNull();

            Guard.Disposal(player.Disposed, nameof(player));

            var taskCompletionSource = new TaskCompletionSource<DialogResponseData>();

            void CancelTaskCompletion()
            {
                this.logger.LogInformation("Cancelled.");

                if (taskCompletionSource.Task.IsCompleted)
                {
                    return;
                }

                taskCompletionSource.TrySetCanceled();
            }

            using (player.CancellationToken.Register(CancelTaskCompletion))
            {
                if (player.TryGetData(LastDialogIdKey, out int dialogId))
                {
                    dialogId += 7;
                }

                void ResponseHandler(PlayerDialogResponseEvent e)
                {
                    if (e.DialogId != dialogId)
                    {
                        this.logger.LogWarning($"Player {e.Player} waits for dialog {dialogId}, but received {e.DialogId}, ignoring.");

                        return;
                    }

                    taskCompletionSource.SetResult(new DialogResponseData(e.Response, e.ListItem, e.InputText));

                    player.TryRemoveData<Action<PlayerDialogResponseEvent>>(CurrentDialogKey, out _);
                }

                var builtDialog = dialog.Build();

                player.HideDialogs();
                player.SetData(CurrentDialogKey, (Action<PlayerDialogResponseEvent>)ResponseHandler);
                player.SetData(LastDialogIdKey, dialogId);

                player.ShowDialog(
                                  dialogId,
                                  builtDialog.Style,
                                  builtDialog.Caption,
                                  builtDialog.Info,
                                  builtDialog.LeftButton,
                                  builtDialog.RightButton);

                return await taskCompletionSource.Task;
            }
        }

        private void OnPlayerDialogResponse(PlayerDialogResponseEvent eventdata)
        {
            if (eventdata.Player.TryGetData<Action<PlayerDialogResponseEvent>>(CurrentDialogKey, out var action) == false)
            {
                return;
            }

            action!(eventdata);
        }
    }
}
