using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CoreAPI.Helpers
{
    public class ApiResponse<T> where T: class
    {
        [DataMember]
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public T Result { get; set; }
    }
}
