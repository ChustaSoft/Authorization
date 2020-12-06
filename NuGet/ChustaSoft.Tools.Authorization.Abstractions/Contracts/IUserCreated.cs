namespace ChustaSoft.Tools.Authorization
{
    /// <summary>
    /// Contract for custom implementation on actions after UserCreated
    /// </summary>
    public interface IUserCreated
    {

        /// <summary>
        /// Behaviour to be implemented that will be raised after user succesfully created
        /// </summary>
        /// <param name="sender">Event Sender</param>
        /// <param name="e">Event arguments with UserId and Parameters</param>
        void DoAfter(object sender, UserEventArgs e);

    }
}
