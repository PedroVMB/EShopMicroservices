namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);


internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handle called with {@request}", request);
        
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException();
        }
        
        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new DeleteProductResult(true);
    }
}