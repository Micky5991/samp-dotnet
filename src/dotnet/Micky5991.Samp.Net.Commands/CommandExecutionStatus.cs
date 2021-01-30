namespace Micky5991.Samp.Net.Commands
{
    /// <summary>
    /// Defines types of command errors that could occur during execution.
    /// </summary>
    public enum CommandExecutionStatus
    {
        /// <summary>
        /// No error occured and command executed normally.
        /// </summary>
        Ok,

        /// <summary>
        /// An exception has been caught and has been processed.
        /// </summary>
        Exception,

        /// <summary>
        /// Passed arguments do not match with the defined types of the command.
        /// </summary>
        ArgumentTypeMismatch,

        /// <summary>
        /// Not enough arguments have been passed.
        /// </summary>
        MissingArgument,

        /// <summary>
        /// Too many arguments have been passed.
        /// </summary>
        TooManyArguments,

        /// <summary>
        /// You do not have permission to execute this command.
        /// </summary>
        NoPermission,
    }
}
