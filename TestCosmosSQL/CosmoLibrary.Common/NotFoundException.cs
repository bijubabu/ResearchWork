using System;

namespace CosmoLibrary.Common
{
    public class ResourceNotFoundException : Exception
    {
        /// <summary>
        /// Validation Exception
        /// </summary>
        /// <param name="id">Id</param>
        public ResourceNotFoundException(string id)
        {
            Message = $"Resource no Found with Id: {id}";
        }

        /// <summary>
        /// Message
        /// </summary>
        public override string Message { get; }
    }
}
