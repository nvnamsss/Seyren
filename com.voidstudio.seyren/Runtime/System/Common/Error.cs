namespace Seyren.System.Common
{
    public class Error {
        public static Error None = new Error("No error.");
        
        private string error;
        public Error(string error) {
            this.error = error;
        }

        public string Message() {
            return error;
        }
    }
}
