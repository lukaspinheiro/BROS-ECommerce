namespace BROS_ECommerce.Domain.Interfaces.Crud
{
    public interface IAdicionar<TEntity> where TEntity : class
    {
        Task Adicionar(TEntity obj);
    }
}
