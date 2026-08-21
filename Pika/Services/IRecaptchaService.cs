namespace Pika.Services;
using System.Threading.Tasks;

public interface IRecaptchaService
{
    Task<bool> ValidateTokenAsync(string token);
}