using WorkshopManager.DTOs;
using WorkshopManager.Models;
using Riok.Mapperly.Abstractions;

namespace WorkshopManager.Mappers
{
    [Mapper]
    public partial class ClientMapper : IClientMapper
    {
        [MapperIgnoreTarget(nameof(Client.Vehicles))]
        public partial ClientDto ToDto(Client client);

        [MapperIgnoreTarget(nameof(Client.Id))]
        [MapperIgnoreTarget(nameof(Client.Vehicles))]
        public partial Client ToEntity(ClientCreateDto dto);

        [MapperIgnoreTarget(nameof(Client.Vehicles))]
        public partial Client ToEntity(ClientUpdateDto dto);
    }
}
