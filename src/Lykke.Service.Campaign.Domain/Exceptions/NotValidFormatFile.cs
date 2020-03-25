using System;

namespace Lykke.Service.Campaign.Domain.Exceptions
{
    public class NotValidFormatFile : Exception
    {
        public NotValidFormatFile() : base("Not valid format")
        {
        }

        public NotValidFormatFile(string message) : base(message)
        {
        }
    }
}
