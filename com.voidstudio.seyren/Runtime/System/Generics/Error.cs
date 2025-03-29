using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Generics
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
