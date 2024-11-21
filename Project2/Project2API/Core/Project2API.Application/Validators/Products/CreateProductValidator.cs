using FluentValidation;
using Project2API.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2API.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().WithMessage("Lütfen ürün adını boş geçmeyiniz.").MaximumLength(150).MinimumLength(5).WithMessage("Ürün adını daha uzun giriniz.");

            RuleFor(p => p.Stock).NotEmpty().NotNull().WithMessage("Lütfen stok bilgisini giriniz.").Must(s => s >= 0).WithMessage("Ürün stok adedi negatif olamaz !");

            RuleFor(p => p.Price).NotEmpty().NotNull().WithMessage("Lütfen fiyat bilgisini giriniz.").Must(s => s >= 0).WithMessage("Ürün fiyatı negatif olamaz !");
        }
    }
}