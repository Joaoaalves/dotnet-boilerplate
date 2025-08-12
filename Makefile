include .env
.PHONY: docs clean-docs

docs:
	docfx metadata docs/docfx.json
	docfx build docs/docfx.json

ifeq ($(OS),Windows_NT)
clean-docs:
	if exist docs\_site rmdir /s /q docs\_site docs\api
else
clean-docs:
	rm -rf docs/_site docs\api
endif

###
### MIGRATIONS
###
migrate-up:
	docker compose -f docker-compose.dev.yml run --rm \
		-e DATABASE_CONNECTION_STRING=${DATABASE_CONNECTION_STRING} \
		api sh -c "/root/.dotnet/tools/dotnet-ef database update --project ./src/Project.Infrastructure --startup-project ./src/Project.Api"

migrate-add:
ifndef name
	$(error Migration name not set. Usage: make migrate-add name=MigrationName)
endif
ifeq ($(OS),Windows_NT)
	set DATABASE_CONNECTION_STRING=$(DATABASE_CONNECTION_STRING) && \
	dotnet ef migrations add $(name) --project ./src/Project.Infrastructure --startup-project ./src/Project.Api
else
	DATABASE_CONNECTION_STRING=$(DATABASE_CONNECTION_STRING) \
	dotnet ef migrations add $(name) --project ./src/Project.Infrastructure --startup-project ./src/Project.Api
endif