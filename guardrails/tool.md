# Backend Tools and External Services

## Core Stack

- ASP.NET Core Web API.
- C#.
- PostgreSQL.
- Entity Framework Core with Npgsql.
- FluentValidation.
- JWT bearer authentication with refresh tokens.
- Redis Cache.
- Hangfire for background jobs and scheduled work.
- SignalR for live status events.
- backend email service abstraction.
- xUnit.
- Testcontainers for integration tests where practical.

Use `../../naitrust-api-old` only as a behavior reference for auth, QoreID, payment boundaries, verification flows, and tests. Do not copy the old Node/Express structure into the new .NET API.

Project decision: ASP.NET Core + PostgreSQL + Entity Framework Core. Entity Framework Core is the only approved backend ORM. Do not use Prisma or Drizzle in the API.

## Old Code References

Important old folders:

- `../../naitrust-api-old/src/config`
- `../../naitrust-api-old/src/db`
- `../../naitrust-api-old/src/middleware`
- `../../naitrust-api-old/src/routes`
- `../../naitrust-api-old/src/controllers`
- `../../naitrust-api-old/src/services`
- `../../naitrust-api-old/src/services/third-party`
- `../../naitrust-api-old/src/utils`
- `../../naitrust-api-old/src/__tests__`

These are reference material only. Port useful ideas into .NET-native code.

## External Services

Verification:

- QoreID.
- Prembly.
- CAC/business verification provider.
- BVN/NIN/passport/driver's license verification provider through approved partners.
- Face/liveness verification provider through QoreID or another approved provider.

Payments and protected fund movement:

- regulated partner bank.
- Providus Bank as the development partner adapter.
- Kora/Korapay adapter placeholder.
- Wema Bank adapter placeholder.
- Anchor adapter placeholder.
- other licensed Nigerian payment partner behind the same port.

Storage:

- ImageKit or object storage for evidence files.

Communication:

- email provider behind the backend email service abstraction.
- Termii for SMS/OTP where suitable.

AI:

- OpenAI for internal risk summaries, dispute summaries, fraud explanations, and admin assistance.
- OpenAI Responses API for structured AI assessments and assistants.
- OpenAI embeddings for similarity search across disputes, fraud patterns, evidence, and transaction history.
- OpenAI moderation/safety tooling for user-generated text and public profile content.
- AI must not make final dispute or compliance decisions.

Infrastructure:

- PostgreSQL for source-of-truth data.
- Redis Cache for rate limiting, transient sessions, idempotency support, and performance-sensitive reads.
- Hangfire for outbox processing, notifications, reconciliation, webhook retries, auto-confirm windows, and scheduled jobs.
- Transactional outbox for reliable event dispatch.
- SignalR for real-time notifications.
- Entity Framework Core for database access and migrations.
- Serilog for structured logging.
- OpenTelemetry for tracing.
- Render for API hosting/deployment.

## Environment Variables

Expected groups:

- app: `ASPNETCORE_ENVIRONMENT`, `PORT`, `API_BASE_URL`, `WEB_BASE_URL`
- database: `DATABASE_URL`
- auth: `JWT_SECRET`, `JWT_EXPIRES_IN`, `REFRESH_TOKEN_SECRET`
- redis: `REDIS_URL`
- hangfire: `HANGFIRE_DASHBOARD_ENABLED`, `HANGFIRE_DASHBOARD_PATH`
- payment partner: Providus Bank keys, webhook secret, and future partner-specific secret keys
- verification: QoreID/Prembly keys
- storage: ImageKit or storage keys
- communication: email provider host/API key/from address and SMS keys
- render: Render service environment variables should be configured in Render dashboard or `render.yaml`; never commit secrets
- AI: `OPENAI_API_KEY`

Never commit real secrets.
