using CommunityToolkit.Mvvm.Messaging.Messages;
using Incubator.Domain.Entities;

namespace Incubator.Desktop.Messages
{
    // Un mensaje que transporta un objeto Cliente
    public class ClientCreatedMessage : ValueChangedMessage<Client>
    {
        public ClientCreatedMessage(Client newClient) : base(newClient)
        {
        }
    }
}
