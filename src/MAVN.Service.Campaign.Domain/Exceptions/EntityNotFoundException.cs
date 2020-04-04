using System;

namespace MAVN.Service.Campaign.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("Entity not found")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
