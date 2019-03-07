using EventFlow.Commands;

namespace EventFlow.Documentation.Topics.Commands
{
    public class UserUpdatePasswordCommand : Command<UserAggregate, UserId>
    {
        public UserUpdatePasswordCommand(
            UserId id,
            Password newPassword,
            Password oldPassword)
            : base(id)
        {
            NewPassword = newPassword;
            OldPassword = oldPassword;
        }

        public Password NewPassword { get; }
        public Password OldPassword { get; }
    }
}