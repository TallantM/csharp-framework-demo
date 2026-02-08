using Xunit;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;
using csharp_framework_demo.Utilities.SpecValidation;
using System.IO;

namespace csharp_framework_demo.Tests;

/// <summary>
/// Conformance tests that validate spec-code alignment
/// These meta-tests ensure the Spec-Driven Development framework is properly set up
/// </summary>
[AllureSuite("Conformance Tests")]
[AllureFeature("Spec-Driven Development")]
public class SpecConformanceTests
{
    private readonly string _projectRoot;

    public SpecConformanceTests()
    {
        _projectRoot = SpecParser.GetProjectRoot();
    }

    #region Meta-Framework Validation

    [Fact]
    [AllureDescription("Validates that META-FRAMEWORK.md exists and contains required sections")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Meta-Framework")]
    public void MetaFramework_Spec_Exists_And_IsValid()
    {
        // Arrange
        var metaFrameworkPath = Path.Combine(_projectRoot, "specs", "META-FRAMEWORK.md");

        // Act & Assert
        Assert.True(File.Exists(metaFrameworkPath), $"META-FRAMEWORK.md not found at {metaFrameworkPath}");

        var content = File.ReadAllText(metaFrameworkPath);
        Assert.False(string.IsNullOrWhiteSpace(content), "META-FRAMEWORK.md is empty");

        // Verify required sections exist
        var requiredSections = new[]
        {
            "## Overview",
            "## Spec-to-Code Mapping Rules",
            "## Generation Templates",
            "## Conformance Validation Rules",
            "## Drift Detection Strategy",
            "## Approval Workflow",
            "## CI/CD Integration"
        };

        foreach (var section in requiredSections)
        {
            Assert.Contains(section, content, System.StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    [AllureDescription("Validates that PROJECT-SPEC.md exists and defines architectural constraints")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Project-Spec")]
    public void ProjectSpec_Exists_And_IsValid()
    {
        // Arrange
        var projectSpecPath = Path.Combine(_projectRoot, "specs", "PROJECT-SPEC.md");

        // Act & Assert
        Assert.True(File.Exists(projectSpecPath), $"PROJECT-SPEC.md not found at {projectSpecPath}");

        var content = File.ReadAllText(projectSpecPath);
        Assert.False(string.IsNullOrWhiteSpace(content), "PROJECT-SPEC.md is empty");

        // Verify required sections exist
        var requiredSections = new[]
        {
            "## System Purpose",
            "## Architectural Constraints",
            "## Design Patterns",
            "## Cross-Cutting Invariants",
            "## Quality Gates"
        };

        foreach (var section in requiredSections)
        {
            Assert.Contains(section, content, System.StringComparison.OrdinalIgnoreCase);
        }

        // Verify key technologies are documented
        Assert.Contains(".NET 8.0", content);
        Assert.Contains("Playwright", content);
        Assert.Contains("xUnit", content);
        Assert.Contains("Page Object Model", content);
    }

    #endregion

    #region Template Validation

    [Fact]
    [AllureDescription("Validates that all spec templates exist in specs/templates/")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Templates")]
    public void SpecTemplates_AllExist()
    {
        // Arrange
        var templatesPath = Path.Combine(_projectRoot, "specs", "templates");

        var requiredTemplates = new[]
        {
            "page-objects.template.md",
            "unit-tests.template.md",
            "integration-tests.template.md",
            "workflows.template.md"
        };

        // Act & Assert
        Assert.True(Directory.Exists(templatesPath), $"Templates directory not found at {templatesPath}");

        foreach (var template in requiredTemplates)
        {
            var templatePath = Path.Combine(templatesPath, template);
            Assert.True(File.Exists(templatePath), $"Template not found: {template}");

            var content = File.ReadAllText(templatePath);
            Assert.False(string.IsNullOrWhiteSpace(content), $"Template is empty: {template}");
        }
    }

    [Fact]
    [AllureDescription("Validates that page-objects template has required structure")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Templates", "Page Object")]
    public void PageObjectTemplate_HasRequiredStructure()
    {
        // Arrange
        var templatePath = Path.Combine(_projectRoot, "specs", "templates", "page-objects.template.md");

        // Act
        var content = File.ReadAllText(templatePath);

        // Assert
        var requiredSections = new[]
        {
            "## Page Overview",
            "## Page Elements (Selectors)",
            "## Behavioral Contracts (Methods)",
            "## Mapping to Code"
        };

        foreach (var section in requiredSections)
        {
            Assert.Contains(section, content, System.StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    [AllureDescription("Validates that unit-tests template has required structure")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Templates", "Unit Tests")]
    public void UnitTestsTemplate_HasRequiredStructure()
    {
        // Arrange
        var templatePath = Path.Combine(_projectRoot, "specs", "templates", "unit-tests.template.md");

        // Act
        var content = File.ReadAllText(templatePath);

        // Assert
        var requiredSections = new[]
        {
            "## Test Suite Overview",
            "## Test Configuration",
            "## Test Scenarios",
            "## Mocking Guidelines"
        };

        foreach (var section in requiredSections)
        {
            Assert.Contains(section, content, System.StringComparison.OrdinalIgnoreCase);
        }

        // Verify Moq usage is documented
        Assert.Contains("Mock<IPage>", content);
        Assert.Contains("mockPage.Verify", content);
    }

    [Fact]
    [AllureDescription("Validates that integration-tests template has required structure")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Templates", "Integration Tests")]
    public void IntegrationTestsTemplate_HasRequiredStructure()
    {
        // Arrange
        var templatePath = Path.Combine(_projectRoot, "specs", "templates", "integration-tests.template.md");

        // Act
        var content = File.ReadAllText(templatePath);

        // Assert
        var requiredSections = new[]
        {
            "## Test Suite Overview",
            "## Test Configuration",
            "## Test Scenarios",
            "## Assertion Guidelines"
        };

        foreach (var section in requiredSections)
        {
            Assert.Contains(section, content, System.StringComparison.OrdinalIgnoreCase);
        }

        // Verify PlaywrightFixture usage is documented
        Assert.Contains("IClassFixture<PlaywrightFixture>", content);
        Assert.Contains("Assertions.Expect", content);
    }

    [Fact]
    [AllureDescription("Validates that workflows template has required structure")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Templates", "E2E Tests")]
    public void WorkflowsTemplate_HasRequiredStructure()
    {
        // Arrange
        var templatePath = Path.Combine(_projectRoot, "specs", "templates", "workflows.template.md");

        // Act
        var content = File.ReadAllText(templatePath);

        // Assert
        var requiredSections = new[]
        {
            "## Test Suite Overview",
            "## Test Configuration",
            "## Workflow Scenarios",
            "## Allure Step Guidelines"
        };

        foreach (var section in requiredSections)
        {
            Assert.Contains(section, content, System.StringComparison.OrdinalIgnoreCase);
        }

        // Verify AllureApi.Step usage is documented
        Assert.Contains("AllureApi.Step", content);
        Assert.Contains("End-to-End Tests", content);
    }

    #endregion

    #region Directory Structure Validation

    [Fact]
    [AllureDescription("Validates that specs directory structure exists")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Directory Structure")]
    public void SpecsDirectory_HasCorrectStructure()
    {
        // Arrange & Act & Assert
        var specsPath = Path.Combine(_projectRoot, "specs");
        Assert.True(Directory.Exists(specsPath), "specs/ directory does not exist");

        var templatesPath = Path.Combine(specsPath, "templates");
        Assert.True(Directory.Exists(templatesPath), "specs/templates/ directory does not exist");

        // Future: When features are added, validate specs/features/ directory exists
    }

    [Fact]
    [AllureDescription("Validates that SpecValidation utilities exist")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Conformance", "Utilities")]
    public void SpecValidationUtilities_Exist()
    {
        // Arrange
        var specValidationPath = Path.Combine(_projectRoot, "src", "Utilities", "SpecValidation");

        // Act & Assert
        Assert.True(Directory.Exists(specValidationPath), "SpecValidation directory does not exist");

        var requiredFiles = new[]
        {
            "SpecModels.cs",
            "SpecParser.cs",
            "CodeValidator.cs",
            "ValidationResult.cs"
        };

        foreach (var file in requiredFiles)
        {
            var filePath = Path.Combine(specValidationPath, file);
            Assert.True(File.Exists(filePath), $"SpecValidation utility not found: {file}");
        }
    }

    #endregion

    #region Future Feature Validation (Commented Out)

    // The tests below will be enabled when feature specs are created

    /*
    [Fact]
    [AllureDescription("Validates that Page Object specs match generated classes")]
    public void PageObjectSpecs_MatchGeneratedClasses()
    {
        // TODO: Implement when feature specs exist
        // 1. Find all page-objects.md files in specs/features/
        // 2. Parse each spec using SpecParser
        // 3. Validate corresponding Page Object class exists
        // 4. Validate class structure matches spec
    }

    [Fact]
    [AllureDescription("Validates that unit test specs match generated tests")]
    public void UnitTestSpecs_MatchGeneratedTests()
    {
        // TODO: Implement when feature specs exist
        // 1. Find all unit-tests.md files in specs/features/
        // 2. Parse each spec using SpecParser
        // 3. Validate corresponding test class exists
        // 4. Validate test methods match spec scenarios
    }

    [Fact]
    [AllureDescription("Validates that integration test specs match generated tests")]
    public void IntegrationTestSpecs_MatchGeneratedTests()
    {
        // TODO: Implement when feature specs exist
    }

    [Fact]
    [AllureDescription("Validates that workflow specs match generated tests")]
    public void WorkflowSpecs_MatchGeneratedTests()
    {
        // TODO: Implement when feature specs exist
    }
    */

    #endregion
}
