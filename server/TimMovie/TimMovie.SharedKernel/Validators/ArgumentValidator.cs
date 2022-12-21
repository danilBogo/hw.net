namespace TimMovie.SharedKernel.Validators;

public static class ArgumentValidator
{
    /// <summary>
    /// Проверяет объект на null, выкидывает <see cref="ArgumentNullException"/> если null.
    /// </summary>
    public static void ThrowExceptionIfNull(object obj, string paramName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}