using System;

namespace MAVN.Service.Campaign.Domain.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string message)
            : base(message)
        {
        }

        public EntityAlreadyExistsException()
            : base("Entity already exists")
        {
        }

        public EntityAlreadyExistsException(Exception innerException)
            : base("Entity already exists", innerException)
        {
        }
    }
}
