using System.ComponentModel.DataAnnotations;
using DTOs;
using DAL;
using DAL.ApplicationDbContext;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DAL.Models;
using Resources.Words;
using MediatR;

namespace BLL.Features.TestContext.Commands
{
    public class TestCreateCommand : IRequest<int>
    {
        /// <summary>
        /// Test dto
        /// </summary>
        [Required]
        public TestDto TestDto { get; set; }
    }

    /// <summary>
    /// Create command handler
    /// </summary>
    public class TestCreateCommandHandler : IRequestHandler<TestCreateCommand, int>
    {
        private readonly AppMsSqlDbContext _context;
        private readonly ILogger<TestCreateCommandHandler> _logger;
        private readonly IMapper _mapper;

        public TestCreateCommandHandler(AppMsSqlDbContext context, 
            ILogger<TestCreateCommandHandler> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Logic of creating
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(TestCreateCommand command, CancellationToken cancellationToken)
        {
            _logger.LogError(Resource.asdasdasdsadsad);

            var model = _mapper.Map<TestModel>(command.TestDto);
            var entity = await _context.TestModels.AddAsync(model);
            await _context.SaveChangesAsync();
            return entity.Entity.Id; 
        }
    }
}
