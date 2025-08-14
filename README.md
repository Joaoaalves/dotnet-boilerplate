# .NET Boilerplate Project

## Running in Development Mode

To start the API and documentation locally with hot reload, run:

```bash
docker-compose -f docker-compose.dev.yml up --build
```

- The API will be available at: `http://localhost:5000`
- The documentation will be available at: `http://localhost:5001`

---

## Creating New Migrations

To create a new migration, run the following command:

```bash
make migrate-add name=MigrationName
```

Then, apply the migration to the database with:

```bash
make migrate-up
```

> **Note:** Make sure Docker is running before applying migrations.

---

## Generating Documentation Manually

To generate updated documentation using DocFX, run:

```bash
make docs
```

> This will compile the documentation into the `docs/_site` folder.

---

## Setting Up Git Hooks for Pre-Commit Validation

This project uses custom Git hooks to ensure:

- Code formatting is applied (`dotnet format`)
- All tests pass (`dotnet test`)
- Documentation is up to date (`docfx docs/docfx.json`)

### Enabling Hooks in Your Local Environment

1. Configure Git to use the custom `.githooks` directory:

```bash
git config core.hooksPath .githooks
```

2. Ensure that the `.githooks/pre-commit` script (and its helper `.sh` and `.ps1` files) are present in your local clone.

3. On commit, the hook will automatically run checks. If any check fails, the commit will be aborted.

---

## Environment Variables

Create a `.env` file at the root of the project to configure the API and documentation domains.

Refer to `.env.example` for an example configuration.

---

## Using a Custom Domain for Documentation

- Documentation will be available at `docs.{DOMAIN}`, as configured in the `.env` file.
- The API will be exposed via HTTPS using the configured domain.

---

## Observability

To enable the full observability stack — including monitoring (OpenTelemetry, Prometheus, Grafana) and logging (Serilog, Elasticsearch, Kibana) — run Docker with the `observability` profile:

```bash
docker compose -f docker-compose.dev.yml --profile observability up
```

### Enabling Monitoring Only

To enable only the monitoring stack, update your `.env` file accordingly and run:

```bash
docker compose -f docker-compose.dev.yml --profile monitoring up
```

### Enabling Logging Only

To enable only the logging stack, update your `.env` file accordingly and run:

```bash
docker compose -f docker-compose.dev.yml --profile logging up
```

---

## Need Help?

If you have any questions or run into issues, feel free to [open an issue](../../issues) in the repository!
