Write-Host "Running dotnet format..."
dotnet format --verify-no-changes
if ($LASTEXITCODE -ne 0) {
    Write-Host "Code is not formatted. Run 'dotnet format' before committing."
    exit 1
}

Write-Host "Running dotnet test..."
dotnet test --no-build --verbosity minimal
if ($LASTEXITCODE -ne 0) {
    Write-Host "Tests failed. Fix them before committing."
    exit 1
}

Write-Host "Generating documentation..."
docfx docs/docfx.json > $null 2>&1
git add docs/_site

$diff = git diff --quiet --exit-code docs/_site
if (-not $?) {
    Write-Host "Documentation is outdated. Run 'make docs' to update it."
    exit 1
}

exit 0
