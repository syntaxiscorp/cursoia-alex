# Test runner script for DevSecOpsDemo (PowerShell)
# Usage: .\test.ps1 [options]

param(
    [string]$Configuration = "Release",
    [string]$Verbosity = "normal",
    [switch]$Coverage,
    [switch]$Watch,
    [string]$Project = "",
    [switch]$Help
)

# Colors for output
$Red = "Red"
$Green = "Green"
$Yellow = "Yellow"
$Blue = "Blue"

if ($Help) {
    Write-Host "DevSecOpsDemo Test Runner" -ForegroundColor $Blue
    Write-Host "==============================="
    Write-Host ""
    Write-Host "Usage: .\test.ps1 [options]"
    Write-Host ""
    Write-Host "Options:"
    Write-Host "  -Configuration <config>   Build configuration (Debug/Release, default: Release)"
    Write-Host "  -Verbosity <level>        Verbosity level (quiet/minimal/normal/detailed, default: normal)"
    Write-Host "  -Coverage                 Enable code coverage collection"
    Write-Host "  -Watch                    Run tests in watch mode"
    Write-Host "  -Project <project>        Run tests for specific project only"
    Write-Host "  -Help                     Show this help message"
    exit 0
}

Write-Host "DevSecOpsDemo Test Runner" -ForegroundColor $Blue
Write-Host "==============================="

Write-Host "Configuration:" -ForegroundColor $Blue
Write-Host "  - Build Configuration: $Configuration"
Write-Host "  - Verbosity: $Verbosity"
Write-Host "  - Code Coverage: $(if ($Coverage) { 'Enabled' } else { 'Disabled' })"
Write-Host "  - Watch Mode: $(if ($Watch) { 'Enabled' } else { 'Disabled' })"
Write-Host "  - Project Filter: $(if ($Project) { $Project } else { 'All projects' })"
Write-Host ""

# Build solution first
Write-Host "Building solution..." -ForegroundColor $Yellow
$buildResult = dotnet build .\DevSecOpsDemo.sln --configuration $Configuration --verbosity quiet
$buildExitCode = $LASTEXITCODE

if ($buildExitCode -ne 0) {
    Write-Host "Build failed!" -ForegroundColor $Red
    exit $buildExitCode
}

Write-Host "Build successful!" -ForegroundColor $Green
Write-Host ""

# Prepare test command
$testArgs = @(
    "test",
    ".\DevSecOpsDemo.sln",
    "--configuration", $Configuration,
    "--no-build",
    "--verbosity", $Verbosity
)

# Add project filter if specified
if ($Project) {
    $testArgs += "--filter"
    $testArgs += "FullyQualifiedName~$Project"
}

# Add coverage collection if enabled
if ($Coverage) {
    Write-Host "Code coverage enabled" -ForegroundColor $Yellow
    $testArgs += "--collect:XPlat Code Coverage"
    $testArgs += "--settings"
    $testArgs += "CodeCoverage.runsettings"
    $testArgs += "--results-directory"
    $testArgs += ".\TestResults"
}

# Handle watch mode
if ($Watch) {
    Write-Host "Watch mode enabled - Press Ctrl+C to stop" -ForegroundColor $Yellow
    $watchArgs = @(
        "watch", "test",
        ".\DevSecOpsDemo.sln",
        "--configuration", $Configuration,
        "--verbosity", $Verbosity
    )
    Write-Host "Running tests in watch mode..." -ForegroundColor $Blue
    & dotnet $watchArgs
} else {
    Write-Host "Running tests..." -ForegroundColor $Blue
    Write-Host "Command: dotnet $($testArgs -join ' ')"
    Write-Host ""
    
    # Execute tests
    & dotnet $testArgs
    $testExitCode = $LASTEXITCODE
    
    if ($testExitCode -eq 0) {
        Write-Host ""
        Write-Host "All tests passed!" -ForegroundColor $Green
        
        # Generate coverage report if coverage was enabled
        if ($Coverage) {
            Write-Host "Generating coverage report..." -ForegroundColor $Blue
            
            # Check if reportgenerator tool is installed
            try {
                $null = Get-Command reportgenerator -ErrorAction Stop
            }
            catch {
                Write-Host "Installing ReportGenerator tool..." -ForegroundColor $Yellow
                dotnet tool install -g dotnet-reportgenerator-globaltool
            }
            
            # Generate HTML coverage report
            $reportArgs = @(
                "-reports:.\TestResults\**\coverage.cobertura.xml",
                "-targetdir:.\TestResults\CoverageReport",
                "-reporttypes:Html",
                "-verbosity:Info"
            )
            
            & reportgenerator $reportArgs
            
            Write-Host "Coverage report generated: .\TestResults\CoverageReport\index.html" -ForegroundColor $Green
            
            # Try to open the report in the default browser
            if (Test-Path ".\TestResults\CoverageReport\index.html") {
                Write-Host "Opening coverage report in browser..." -ForegroundColor $Blue
                Start-Process ".\TestResults\CoverageReport\index.html"
            }
        }
    } else {
        Write-Host ""
        Write-Host "Tests failed with exit code: $testExitCode" -ForegroundColor $Red
        exit $testExitCode
    }
}

Write-Host ""
Write-Host "Test execution completed!" -ForegroundColor $Blue