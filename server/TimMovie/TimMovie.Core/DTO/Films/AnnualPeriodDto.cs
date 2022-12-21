namespace TimMovie.Core.DTO.Films;

public record AnnualPeriodDto(int FirstYear, int LastYear)
{
    public bool IsOneYear => FirstYear == LastYear;
}