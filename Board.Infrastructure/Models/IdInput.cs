namespace Board.Infrastructure.Models
{
    public class IdInput<T>
    {
        public IdInput(T id)
        {
            this.Id = id;
        }
        /// <summary>
        /// Id of the entity.
        /// </summary>
        public T Id { get; set; }

    }
}
