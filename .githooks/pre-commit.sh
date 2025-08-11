#!/bin/sh

echo "Rodando dotnet format..."
dotnet format --verify-no-changes
if [ $? -ne 0 ]; then
  echo "Código não está formatado. Rode 'dotnet format' antes de commitar."
  exit 1
fi

echo "Rodando dotnet test..."
dotnet test --no-build --verbosity minimal
if [ $? -ne 0 ]; then
  echo "Testes falharam. Corrija antes de commitar."
  exit 1
fi

echo "Gerando documentação..."
docfx docs/docfx.json > /dev/null 2>&1
git add docs/_site

if ! git diff --quiet --exit-code docs/_site; then
  echo "Documentação desatualizada. Rode 'make docs' para atualizar."
  exit 1
fi

exit 0
