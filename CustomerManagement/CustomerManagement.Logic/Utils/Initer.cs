using NullGuard;

[assembly: NullGuard(ValidationFlags.All)]
namespace CustomerManagement.Logic.Utils
{
    /// <summary>
    /// NullGuard checks whether or not you have null validations for the input parameters in your methods all over the assembly.
    /// If you don't have these validations it emits them when you compile the project.
    /// </summary>
    public static class Initer
    {
        public static void Init(string connectionString)
        {
            SessionFactory.Init(connectionString);
        }
    }
}
