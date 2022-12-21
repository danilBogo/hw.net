namespace TimMovie.SharedKernel.Interfaces;

public interface IIdHolder<TId>
{
    TId Id { get; set; }
}