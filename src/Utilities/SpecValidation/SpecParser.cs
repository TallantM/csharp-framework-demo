using System;
using System.IO;
using System.Linq;

namespace csharp_framework_demo.Utilities.SpecValidation;

/// <summary>
/// Parses specification files (markdown) and extracts structured data
/// </summary>
public class SpecParser
{
    /// <summary>
    /// Validates that a specification file exists and is readable
    /// </summary>
    /// <param name="filePath">Path to the spec file</param>
    /// <returns>True if file exists and is readable</returns>
    public static bool SpecFileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// Validates that a specification file has minimum required content
    /// </summary>
    /// <param name="filePath">Path to the spec file</param>
    /// <returns>True if file has content</returns>
    public static bool HasContent(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        var content = File.ReadAllText(filePath);
        return !string.IsNullOrWhiteSpace(content);
    }

    /// <summary>
    /// Validates that a specification file contains required sections
    /// </summary>
    /// <param name="filePath">Path to the spec file</param>
    /// <param name="requiredSections">Required section headers</param>
    /// <returns>True if all required sections are present</returns>
    public static bool HasRequiredSections(string filePath, params string[] requiredSections)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        var content = File.ReadAllText(filePath);

        return requiredSections.All(section =>
            content.Contains(section, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Parses a Page Object specification file
    /// </summary>
    /// <param name="filePath">Path to the page object spec file</param>
    /// <returns>Parsed PageObjectSpec</returns>
    /// <remarks>
    /// Current implementation is basic validation.
    /// Future enhancement: Full markdown parsing to extract methods, parameters, etc.
    /// </remarks>
    public static PageObjectSpec? ParsePageObjectSpec(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        var content = File.ReadAllText(filePath);

        // Basic parsing - extract page name from filename
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var pageName = fileName.Replace("page-objects", "").Replace(".template", "").Trim();

        return new PageObjectSpec
        {
            FilePath = filePath,
            PageName = pageName,
            ClassName = $"{pageName}Page",
            ExpectedFile = $"src/Utilities/PageObjects/{pageName}Page.cs"
        };

        // TODO: Future enhancement - parse markdown to extract:
        // - Methods from "Behavioral Contracts" section
        // - Method signatures, parameters, return types
        // - Selectors from "Page Elements" section
    }

    /// <summary>
    /// Parses a Test specification file
    /// </summary>
    /// <param name="filePath">Path to the test spec file</param>
    /// <param name="testType">Type of test (Unit, Integration, E2E)</param>
    /// <returns>Parsed TestSpec</returns>
    /// <remarks>
    /// Current implementation is basic validation.
    /// Future enhancement: Full markdown parsing to extract scenarios, assertions, etc.
    /// </remarks>
    public static TestSpec? ParseTestSpec(string filePath, TestType testType)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        var content = File.ReadAllText(filePath);

        // Basic parsing - extract class name from filename
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var feature = fileName.Replace("unit-tests", "")
                             .Replace("integration-tests", "")
                             .Replace("workflows", "")
                             .Replace(".template", "")
                             .Trim();

        var allureSuite = testType switch
        {
            TestType.Unit => "Unit Tests",
            TestType.Integration => "Integration Tests",
            TestType.E2E => "End-to-End Tests",
            _ => "Unknown"
        };

        var className = testType switch
        {
            TestType.Unit => $"{feature}PageUnitTests",
            TestType.Integration => $"{feature}PageIntegrationTests",
            TestType.E2E => $"{feature}WorkflowTests",
            _ => "Unknown"
        };

        return new TestSpec
        {
            FilePath = filePath,
            ClassName = className,
            TestType = testType,
            AllureSuite = allureSuite,
            AllureFeature = feature
        };

        // TODO: Future enhancement - parse markdown to extract:
        // - Test scenarios from spec
        // - Expected method names
        // - Given/When/Then steps
        // - Expected assertions
    }

    /// <summary>
    /// Gets the project root directory (where .sln file is located)
    /// </summary>
    public static string GetProjectRoot()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        // Search for .sln file in current and parent directories
        while (!string.IsNullOrEmpty(currentDirectory))
        {
            if (Directory.GetFiles(currentDirectory, "*.sln").Any())
            {
                return currentDirectory;
            }

            var parent = Directory.GetParent(currentDirectory);
            currentDirectory = parent?.FullName ?? string.Empty;
        }

        // Fallback: return current directory
        return Directory.GetCurrentDirectory();
    }
}
