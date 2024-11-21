using MediatR;
using Project2API.Application.Repositories;
using Project2API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2API.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        public GetByIdProductQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }
        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
            Project2API.Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id, false 
                /* tracking mekanizmasından koparabiliriz false vererek*/);
            return new()
            {
                Name=product.Name,
                Stock=product.Stock,
                Price=product.Price,
            };
        }
    }
}
