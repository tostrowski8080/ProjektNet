using WorkshopManager.DTOs;
using WorkshopManager.Models;
using Riok.Mapperly.Abstractions;

namespace WorkshopManager.Mappers
{

    [Mapper]
    public partial class ClientMapper : IClientMapper
    {
        public partial ClientDto ToDto(Client client);
        public partial Client ToEntity(ClientCreateDto dto);
    }
}
