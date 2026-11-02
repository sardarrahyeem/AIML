using System.Threading.RateLimiting;
using Microsoft.Extensions.AI;

public static class RateLimitingChatClientExtensions
{
    public static ChatClientBuilder UseRateLimiting(
        this ChatClientBuilder builder,
        RateLimiter rateLimiter) =>
        builder.Use(innerClient =>
            new RateLimitingChatClient(innerClient, rateLimiter)
        );
}