.PHONY: docs clean-docs

docs:
	docfx docs/docfx.json

ifeq ($(OS),Windows_NT)
clean-docs:
	if exist docs\_site rmdir /s /q docs\_site
else
clean-docs:
	rm -rf docs/_site
endif
