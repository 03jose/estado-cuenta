using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace estadoCuentaAPI.HealthChecks
{
    public class CustomHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("La API está funcionando sin problemas"));
        }
    }
}
