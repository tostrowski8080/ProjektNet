using WorkshopManager.DTOs;
using WorkshopManager.Models;
using Riok.Mapperly.Abstractions;

namespace WorkshopManager.Mappers
{
    [Mapper]
    public partial class ClientMapper : IClientMapper
    {
        [MapperIgnoreTarget(nameof(Client.Id))]
        [MapperIgnoreTarget(nameof(Client.Vehicles))]
        public partial ClientDto ToDto(Client client);
        public partial Client ToEntity(ClientCreateDto dto);
    }
}
