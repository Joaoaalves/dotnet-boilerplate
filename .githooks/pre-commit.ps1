Write-Host "Rodando dotnet format..."
dotnet format --verify-no-changes
if ($LASTEXITCODE -ne 0) {
    Write-Host "Código não está formatado. Rode 'dotnet format' antes de commitar."
    exit 1
}

Write-Host "Rodando dotnet test..."
dotnet test --no-build --verbosity minimal
if ($LASTEXITCODE -ne 0) {
    Write-Host "Testes falharam. Corrija antes de commitar."
    exit 1
}

Write-Host "Gerando documentação..."
docfx docs/docfx.json > $null 2>&1
git add docs/_site

$diff = git diff --quiet --exit-code docs/_site
if (-not $?) {
    Write-Host "Documentação desatualizada. Rode 'make docs' para atualizar."
    exit 1
}

exit 0
