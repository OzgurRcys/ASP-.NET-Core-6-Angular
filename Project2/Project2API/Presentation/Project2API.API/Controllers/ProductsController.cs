using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2API.Application.Abstractions.Storage;
using Project2API.Application.Features.Commands.Product.CreateProduct;
using Project2API.Application.Features.Commands.Product.RemoveProduct;
using Project2API.Application.Features.Commands.Product.UpdateProduct;
using Project2API.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage;
using Project2API.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using Project2API.Application.Features.Commands.ProductImageFile.UploadProductImage;
using Project2API.Application.Features.Queries.Product.GetAllProduct;
using Project2API.Application.Features.Queries.Product.GetByIdProduct;
using Project2API.Application.Features.Queries.ProductImageFile.GetProductImages;
using Project2API.Application.Repositories;
using Project2API.Application.RequestParameters;
using Project2API.Application.ViewModels.Products;
using Project2API.Domain.Entities;
using Project2API.Persistence.Repositories;
using Project2API.Persistence.Services;
using System.Net;

namespace Project2API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IFileReadRepository _fileReadRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration;

        readonly IMediator _mediator;

        //readonly private IOrderWriteRepository _orderWriteRepository;
        //readonly private IOrderReadRepository _orderReadRepository;  
        //readonly private ICustomerWriteRepository _customerWriteRepository; test amaçlı yapılmıştı.

        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment,
            IFileWriteRepository fileWriteRepository,
            IFileReadRepository fileReadRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IProductImageFileReadRepository productImageFileReadRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IInvoiceFileReadRepository invoiceFileReadRepository,
            IStorageService storageService,
            IConfiguration configuration,
            IMediator mediator)

        //IOrderWriteRepository orderWriteRepository,
        //ICustomerWriteRepository customerWriteRepository,
        //IOrderReadRepository orderReadRepository)üstteki readonly ile başlayan yerleri yorum satırına aldığımız için buralarıda yorum satırıma almak zorundayız

        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;


            //_orderWriteRepository = orderWriteRepository;
            //_customerWriteRepository = customerWriteRepository;
            //_orderReadRepository = orderReadRepository; üst taraftaki açıklamanın aynısı
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);



            //Order order = await _orderReadRepository.GetByIdAsync("b94588ad-03a1-4273-6c26-08db3452b7a3");
            // order.Address = "İstanbul";
            // await _orderWriteRepository.SaveAsync();

            //var customerId = Guid.NewGuid();
            //await _customerWriteRepository.AddAsync(new() { Id = customerId, Name = "Ahmet" });

            //await _orderWriteRepository.AddAsync(new() { Description = "bla bla", Address = "Ankara, Çankaya", CustomerId= customerId });
            //await _orderWriteRepository.AddAsync(new() { Description = "bla bla2222", Address = "Ankara, Eryaman", CustomerId = customerId });
            //await _orderWriteRepository.SaveAsync();

            ////await _productWriteRepository.AddAsync(new()
            ////{ Name = "C Product", Price = 1.500F, Stock = 10, CreatedDate = DateTime.UtcNow });
            ////await _productWriteRepository.SaveAsync();


            ////////await _productWriteRepository.AddRangeAsync(new()
            ////////{
            ////////    new() {Id = Guid.NewGuid(), Name = "Product 1", Price = 100, CreatedDate = DateTime.UtcNow, Stock = 10,},
            ////////    new() {Id = Guid.NewGuid(), Name = "Product 2", Price = 200, CreatedDate = DateTime.UtcNow, Stock = 20,},
            ////////    new() {Id = Guid.NewGuid(), Name = "Product 3", Price = 300, CreatedDate = DateTime.UtcNow, Stock = 130,},
            ////////});
            ////////var count = await _productWriteRepository.SaveAsync();

            //////Product p = await _productReadRepository.GetByIdAsync("D1DE6FB7-E472-4885-A985-ECC32FDDAC5E", false);
            //////p.Name = "Erciyas"; //yukarıdaki durum false olduğundan herhangi bir değer güncellemesi yapılmaz
            ////////p.Name = "Özgür";
            //////await _productWriteRepository.SaveAsync();
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    Product product = await _productReadRepository.GetByIdAsync(id);
        //    return Ok(product);
        //}
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);

            //var datas = await _storageService.UploadAsync("files", Request.Form.Files);
            ////var datas = await _fileService.UploadAsync("resource/files", Request.Form.Files);
            //await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.pathOrContainerName,
            //    Storage = _storageService.StorageName
            //}).ToList());
            //await _productImageFileWriteRepository.SaveAsync();

            //////////await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //////////{
            //////////    FileName = d.fileName,
            //////////    Path = d.path,
            //////////    Price = new Random().Next()
            //////////}).ToList());
            //////////await _invoiceFileWriteRepository.SaveAsync();

            ////await _fileWriteRepository.AddRangeAsync(datas.Select(d => new Project2API.Domain.Entities.File()
            ////{
            ////    FileName = d.fileName,
            ////    Path = d.path,
            ////}).ToList());
            ////await _fileWriteRepository.SaveAsync();

            //////////////string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");

            //////////////if(!Directory.Exists(uploadPath))
            //////////////    Directory.CreateDirectory(uploadPath);

            //////////////Random r = new ();
            //////////////foreach (IFormFile file in Request.Form.Files)
            //////////////{
            //////////////    string fullPath = Path.Combine(uploadPath, $"{r.Next()}{Path.GetExtension(file.FileName)}");

            //////////////    using FileStream fileStream = new (fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

            //////////////    await file.CopyToAsync(fileStream);
            //////////////    await fileStream.FlushAsync();
            //////////////}

            //////var d1 = _fileReadRepository.GetAll(false);
            //////var d2 = _invoiceFileReadRepository.GetAll(false);
            //////var d3 = _productImageFileReadRepository.GetAll(false);

            return Ok();
        }

        [HttpGet("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
           List<GetProductImagesQueryResponse> response = await _mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
        }
        


        [HttpDelete("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery] string imageId)
        {
           removeProductImageCommandRequest.ImageId = imageId;
           RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery]ChangeShowCaseImageCommandRequest changeShowCaseImageCommandRequest)
        {
            ChangeShowCaseImageCommandResponse response = await _mediator.Send(changeShowCaseImageCommandRequest);
            return Ok();
        }
    }
}