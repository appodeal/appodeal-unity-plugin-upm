namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class Failure
    {
        private Failure(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public string Type { get; }

        public string Message { get; }

        public static Failure Create(string type, string message) => new(type, message);
    }
}
