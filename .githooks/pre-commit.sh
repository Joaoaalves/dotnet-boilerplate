#!/bin/sh

echo "Running dotnet format..."
dotnet format --verify-no-changes
if [ $? -ne 0 ]; then
  echo "Code is not formatted. Run 'dotnet format' before committing."
  exit 1
fi

echo "Running dotnet test..."
dotnet test --no-build --verbosity minimal
if [ $? -ne 0 ]; then
  echo "Tests failed. Fix them before committing."
  exit 1
fi

echo "Generating documentation..."
docfx docs/docfx.json > /dev/null 2>&1
git add docs/_site

if ! git diff --quiet --exit-code docs/_site; then
  echo "Documentation is outdated. Run 'make docs' to update it."
  exit 1
fi

exit 0
