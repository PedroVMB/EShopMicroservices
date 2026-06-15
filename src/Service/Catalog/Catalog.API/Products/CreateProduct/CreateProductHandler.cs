

namespace Catalog.API.Products.CreateProduct;


public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string Imagefile,
    decimal Price
) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category name is required");
        RuleFor(x => x.Imagefile).NotEmpty().WithMessage("Image file is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is required");
    }
}

internal class CreateProductCommandHandler(IDocumentSession session) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            Imagefile = command.Imagefile,
            Price = command.Price
        };
        
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new CreateProductResult(product.Id);
    }
}