using System.ComponentModel.DataAnnotations;

namespace TimMovie.SharedKernel.Classes;

public class DateOnlyAttribute : RangeAttribute
{
    public DateOnlyAttribute()
        : base(typeof(DateTime), DateTime.Now.AddYears(-200).ToShortDateString(), DateTime.Now.ToShortDateString())
    {
    }
}