namespace Micky5991.Samp.Net.Commands.Data.Results
{
    public class CommandResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        /// <param name="status">Descriptive status code for this result.</param>
        /// <param name="message">Possible message of this result.</param>
        public CommandResult(CommandExecutionStatus status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

        public bool Succeeded => this.Status == CommandExecutionStatus.Ok;

        public CommandExecutionStatus Status { get; }

        public string Message { get; }

        public static CommandResult Success()
        {
            return new CommandResult(CommandExecutionStatus.Ok, string.Empty);
        }

        public static CommandResult Failed(CommandExecutionStatus status, string message = "")
        {
            return new CommandResult(status, message);
        }
    }
}
