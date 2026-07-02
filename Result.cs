namespace OptiLauncher.Core.Models;

/// <summary>
/// Implementation of IResult for operation outcomes.
/// </summary>
public class Result : Core.Interfaces.IResult
{
    /// <inheritdoc/>
    public bool IsSuccess { get; private set; }
    
    /// <inheritdoc/>
    public string? ErrorMessage { get; private set; }

    private Result(bool isSuccess, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result Success() => new(true);

    /// <summary>
    /// Creates a failure result with an error message.
    /// </summary>
    /// <param name="errorMessage">The error description.</param>
    public static Result Failure(string errorMessage) => new(false, errorMessage);
}

/// <summary>
/// Implementation of IResult of T for typed operation outcomes.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public class Result<T> : Core.Interfaces.IResult<T>
{
    /// <inheritdoc/>
    public bool IsSuccess { get; private set; }
    
    /// <inheritdoc/>
    public string? ErrorMessage { get; private set; }
    
    /// <inheritdoc/>
    public T? Value { get; private set; }

    private Result(bool isSuccess, T? value = default, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    /// <param name="value">The result value.</param>
    public static Result<T> Success(T value) => new(true, value);

    /// <summary>
    /// Creates a failure result with an error message.
    /// </summary>
    /// <param name="errorMessage">The error description.</param>
    public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
}