using AutoMapper;
using BLL.Interfaces;
using DAL.Models;
using DTOs;
using Repositories;

namespace BLL.Services
{
    public class PartnersService : IPartnersService
    {
        private readonly PartnersRepository _partnersRepository;
        private readonly IMapper _mapper;

        public PartnersService(PartnersRepository partnersRepository, IMapper mapper)
        {
            _partnersRepository = partnersRepository;
            _mapper = mapper;
        }

        public async Task<int> Create(PartnersDto.Add dto)
        {
            var entity = await _partnersRepository.Create(_mapper.Map<Partners>(dto));
            return entity.Id;
        }
    }
}
