using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Munizoft.Identity.Infrastructure.Services
{
    public abstract class BaseService<TService>
    {
        protected readonly ILogger<TService> _logger;
        protected readonly IMapper _mapper;
        protected readonly IdentityOptions _options;

        public BaseService(
            ILogger<TService> logger,
            IMapper mapper,
            IOptions<IdentityOptions> options
            )
        {
            _logger = logger;
            _mapper = mapper;
            _options = options.Value;
        }
    }
}