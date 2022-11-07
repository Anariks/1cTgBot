using System.Text;
using Api.Configurations;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Helpers;

public class Authorization
{
    private readonly IOptionsMonitor<Client1cConfiguration> _1cCredentials;

    public Authorization(IOptionsMonitor<Client1cConfiguration> credentials)
    {
        this._1cCredentials = credentials;
    }

    public bool IsAuthorized(HttpRequest request)
    {
        var authorizationHeader = request.Headers["Authorization"].ToString();
        if (authorizationHeader != null && authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
        {
            var token = authorizationHeader.Substring("Basic ".Length).Trim();
            var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var credentials = credentialsAsEncodedString.Split(':');
            if (_1cCredentials.CurrentValue.Username == credentials[0] &&
                _1cCredentials.CurrentValue.Password == credentials[1])
                return true;
        }
        return false;
    }
}