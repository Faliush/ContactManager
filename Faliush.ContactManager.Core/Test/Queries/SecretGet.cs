using MediatR;

namespace Faliush.ContactManager.Core.Test.Queries;

public record SecretGetRequest() : IRequest<string>;

public class SecretGetRequestHandler : IRequestHandler<SecretGetRequest, string>
{
    public async Task<string> Handle(SecretGetRequest request, CancellationToken cancellationToken)
    {
        return "This is secret";
    }
}
