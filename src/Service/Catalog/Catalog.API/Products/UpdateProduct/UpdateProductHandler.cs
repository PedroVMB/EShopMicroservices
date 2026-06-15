using Catalog.API.GetProductByCategory;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid id, string Name, List<string> Category, string Description, string ImageFile, decimal price) 
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.id).NotEmpty().WithMessage("Product id is required");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .Length(2, 150).WithMessage("Product name must be between 2 and 150 characters");
        
        RuleFor(command => command.price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductCommandHandler(IDocumentSession session) 
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        
        var product = await session.LoadAsync<Product>(request.id,  cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(request.id);
        }

        product.Name = request.Name;
        product.Category = request.Category;
        product.Description = request.Description;
        product.Imagefile = request.ImageFile;
        product.Price = request.price;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new UpdateProductResult(true);
    }
}