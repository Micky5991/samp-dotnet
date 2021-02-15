namespace Micky5991.Samp.Net.Commands.Data.Results
{
    /// <summary>
    /// Result object that tells how the command exection resulted.
    /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether the command has been executed successfully or not.
        /// </summary>
        public bool Succeeded => this.Status == CommandExecutionStatus.Ok;

        /// <summary>
        /// Gets the resulting status of the command execution.
        /// </summary>
        public CommandExecutionStatus Status { get; }

        /// <summary>
        /// Gets the message the executor left. Can be empty.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CommandResult"/> with the <see cref="Status"/> set to <see cref="CommandExecutionStatus.Ok"/>.
        /// </summary>
        /// <returns>Created successful result.</returns>
        public static CommandResult Success()
        {
            return new CommandResult(CommandExecutionStatus.Ok, string.Empty);
        }

        /// <summary>
        /// Creates a new instance of <see cref="CommandResult"/> with the <see cref="Status"/> set to <paramref name="status"/>.
        /// </summary>
        /// <param name="status">Status of the result.</param>
        /// <param name="message">Message that should be saved.</param>
        /// <returns>Created failed result.</returns>
        public static CommandResult Failed(CommandExecutionStatus status, string message = "")
        {
            return new CommandResult(status, message);
        }
    }
}
