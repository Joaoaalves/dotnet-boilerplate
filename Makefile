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


###
### TESTS
###
test-unit:
	dotnet test src/tests/Project.Tests.UnitTests/Project.Tests.UnitTests.csproj

test-integration:
	dotnet test src/tests/Project.Tests.Integration/Project.Tests.Integration.csproj
test:
	dotnet test src
coverage:
	dotnet build ./src
ifeq ($(OS),Windows_NT)
	if exist src\tests\coverage\test-results rmdir /s /q src\tests\coverage\test-results
	if exist src\tests\coverage\report rmdir /s /q src\tests\coverage\report
else
	rm -rf $(CURDIR)/src/tests/coverage/test-results $(CURDIR)/src/tests/coverage/report
endif
	dotnet test src/tests/Project.Tests.UnitTests/Project.Tests.UnitTests.csproj \
		--collect:"XPlat Code Coverage" \
		--no-build \
		--results-directory ./src/tests/coverage/test-results \
		--settings src/tests/coverage/runsettings.xml
	reportgenerator \
		-reports:./src/tests/coverage/test-results/**/coverage.cobertura.xml \
		-targetdir:./src/tests/coverage/report \
		-reporttypes:Html
ifeq ($(OS),Windows_NT)
	powershell -Command "Set-Location -Path '$(CURDIR)'; Invoke-Item 'src\tests\coverage\report\index.html'"
else
	xdg-open src/tests/coverage/report/index.html || open src/tests/coverage/report/index.html
endif