using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BLL.Features.TestContext.Queries
{
    /// <summary>
    /// Get query
    /// </summary>
    public class TestGetQuery : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class TestGetQueryHandler : IRequestHandler<TestGetQuery, int>
    {
        private readonly ILogger<TestGetQueryHandler> _logger;

        public TestGetQueryHandler(ILogger<TestGetQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<int> Handle(TestGetQuery request, CancellationToken cancellationToken)
        {
            return 42;
        }
    }
}
