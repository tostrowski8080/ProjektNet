using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Mappers
{
    public interface IClientMapper
    {
        ClientDto ToDto(Client client);
        Client ToEntity(ClientCreateDto dto);
        Client ToEntity(ClientUpdateDto dto);
    }
}
