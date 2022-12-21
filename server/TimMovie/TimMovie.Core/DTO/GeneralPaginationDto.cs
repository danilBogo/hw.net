namespace TimMovie.Core.DTO;

public class GeneralPaginationDto<TDto>
{
    public TDto DataDto { get; set; }
    public int AmountSkip { get; set; }
    public int AmountTake { get; set; }
}