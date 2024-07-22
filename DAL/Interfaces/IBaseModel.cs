namespace DAL.Interfaces
{
    public interface IBaseModel<T>
    {
        public T Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
