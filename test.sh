#!/bin/bash
# Test runner script for DevSecOpsDemo
# Usage: ./test.sh [options]

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}🧪 DevSecOpsDemo Test Runner${NC}"
echo "==============================="

# Default values
CONFIGURATION="Release"
VERBOSITY="normal"
COVERAGE=false
WATCH=false
PROJECT=""

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        -c|--configuration)
            CONFIGURATION="$2"
            shift 2
            ;;
        -v|--verbosity)
            VERBOSITY="$2"
            shift 2
            ;;
        --coverage)
            COVERAGE=true
            shift
            ;;
        --watch)
            WATCH=true
            shift
            ;;
        -p|--project)
            PROJECT="$2"
            shift 2
            ;;
        -h|--help)
            echo "Usage: $0 [options]"
            echo "Options:"
            echo "  -c, --configuration <config>   Build configuration (Debug/Release, default: Release)"
            echo "  -v, --verbosity <level>        Verbosity level (quiet/minimal/normal/detailed, default: normal)"
            echo "  --coverage                     Enable code coverage collection"
            echo "  --watch                        Run tests in watch mode"
            echo "  -p, --project <project>        Run tests for specific project only"
            echo "  -h, --help                     Show this help message"
            exit 0
            ;;
        *)
            echo -e "${RED}❌ Unknown option: $1${NC}"
            exit 1
            ;;
    esac
done

echo -e "${BLUE}📋 Configuration:${NC}"
echo "  - Build Configuration: $CONFIGURATION"
echo "  - Verbosity: $VERBOSITY"
echo "  - Code Coverage: $([ "$COVERAGE" = true ] && echo "Enabled" || echo "Disabled")"
echo "  - Watch Mode: $([ "$WATCH" = true ] && echo "Enabled" || echo "Disabled")"
echo "  - Project Filter: ${PROJECT:-"All projects"}"
echo ""

# Build solution first
echo -e "${YELLOW}🏗️ Building solution...${NC}"
dotnet build ./DevSecOpsDemo.sln --configuration $CONFIGURATION --verbosity quiet

if [ $? -ne 0 ]; then
    echo -e "${RED}❌ Build failed!${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Build successful!${NC}"
echo ""

# Prepare test command
TEST_CMD="dotnet test ./DevSecOpsDemo.sln --configuration $CONFIGURATION --no-build --verbosity $VERBOSITY"

# Add project filter if specified
if [ ! -z "$PROJECT" ]; then
    TEST_CMD="$TEST_CMD --filter \"FullyQualifiedName~$PROJECT\""
fi

# Add coverage collection if enabled
if [ "$COVERAGE" = true ]; then
    echo -e "${YELLOW}📊 Code coverage enabled${NC}"
    TEST_CMD="$TEST_CMD --collect:\"XPlat Code Coverage\" --settings CodeCoverage.runsettings --results-directory ./TestResults"
fi

# Add watch mode if enabled
if [ "$WATCH" = true ]; then
    echo -e "${YELLOW}👀 Watch mode enabled - Press Ctrl+C to stop${NC}"
    TEST_CMD="dotnet watch test ./DevSecOpsDemo.sln --configuration $CONFIGURATION --verbosity $VERBOSITY"
fi

echo -e "${BLUE}🧪 Running tests...${NC}"
echo "Command: $TEST_CMD"
echo ""

# Execute tests
if [ "$WATCH" = true ]; then
    eval $TEST_CMD
else
    eval $TEST_CMD
    TEST_EXIT_CODE=$?
    
    if [ $TEST_EXIT_CODE -eq 0 ]; then
        echo ""
        echo -e "${GREEN}✅ All tests passed!${NC}"
        
        # Generate coverage report if coverage was enabled
        if [ "$COVERAGE" = true ]; then
            echo -e "${BLUE}📊 Generating coverage report...${NC}"
            
            # Check if reportgenerator tool is installed
            if ! command -v reportgenerator &> /dev/null; then
                echo -e "${YELLOW}⚠️ Installing ReportGenerator tool...${NC}"
                dotnet tool install -g dotnet-reportgenerator-globaltool
            fi
            
            # Generate HTML coverage report
            reportgenerator \
                -reports:"./TestResults/**/coverage.cobertura.xml" \
                -targetdir:"./TestResults/CoverageReport" \
                -reporttypes:Html \
                -verbosity:Info
            
            echo -e "${GREEN}✅ Coverage report generated: ./TestResults/CoverageReport/index.html${NC}"
        fi
    else
        echo ""
        echo -e "${RED}❌ Tests failed with exit code: $TEST_EXIT_CODE${NC}"
        exit $TEST_EXIT_CODE
    fi
fi

echo ""
echo -e "${BLUE}🎉 Test execution completed!${NC}"