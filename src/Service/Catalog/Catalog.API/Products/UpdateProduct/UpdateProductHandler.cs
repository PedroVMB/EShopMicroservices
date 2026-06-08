using Catalog.API.GetProductByCategory;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid id, string Name, List<string> Category, string Description, string ImageFile, decimal price) 
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) 
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Request}", request);
        
        var product = await session.LoadAsync<Product>(request.id,  cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException();
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