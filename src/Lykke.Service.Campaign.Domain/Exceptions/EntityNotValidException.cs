using System;

namespace Lykke.Service.Campaign.Domain.Exceptions
{
    public class EntityNotValidException : Exception
    {
        public EntityNotValidException() : base("Entity not valid")
        {
        }

        public EntityNotValidException(string message) : base(message)
        {
        }
    }
}
