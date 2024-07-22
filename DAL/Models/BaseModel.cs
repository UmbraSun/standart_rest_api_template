using DAL.Interfaces;

namespace DAL.Models
{
    public class BaseModel<T> : IBaseModel<T>
        where T : IEquatable<T>
    {
        public T Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
