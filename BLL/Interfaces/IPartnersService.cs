using DTOs;

namespace BLL.Interfaces
{
    public interface IPartnersService
    {
        Task<int> Create(PartnersDto.Add dto);
    }
}
