# .NET Boilerplate Project

## Running in Development Mode

To start the API and the documentation locally with hot reload, run:

```bash
docker-compose -f docker-compose.dev.yml up --build
```

- The API will be available at: `http://localhost:5000`
- The documentation will be served at: `http://localhost:5001`

---

## Generating Documentation Manually

To generate the updated documentation using docfx, use the command below:

```bash
make docs
```

> This will compile the documentation into the `docs/_site` folder.

---

## Setting Up Git Hooks for Pre-Commit Validation

This project uses custom Git hooks to ensure that:

- The code is formatted (`dotnet format`)
- Tests are passing (`dotnet test`)
- The documentation is up to date (`docfx docs/docfx.json`)

### How to Enable the Hooks in Your Local Environment

1. Configure Git to use the `.githooks` folder with the versioned hooks from the project:

```bash
git config core.hooksPath .githooks
```

2. Make sure that the `.githooks/pre-commit` script (and the helper `.sh` and `.ps1` files) are present in your clone.

3. Now, when you try to commit, the hook will run the checks. If any of them fail, the commit will be aborted.

---

## Environment Variables

Create a `.env` file at the root of the project to configure the API and documentation domain.

Example content of `.env` in `.env.example`

---

## Using the Domain for Documentation

- The documentation will be served at `docs.{DOMAIN}`, as configured in `.env`.
- The API will be exposed via HTTPS on the configured domain.

---

## Observability

If you want to enable and run the observability module, edit the .env file and set it to true, then start the docker with the correct profile

```bash
docker compose -f docker-compose.dev.yml --profile observability up
```

---

If you have questions or need help, open an issue in the repository!
