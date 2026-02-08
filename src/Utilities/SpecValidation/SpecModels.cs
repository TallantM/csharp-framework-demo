using System.Collections.Generic;

namespace csharp_framework_demo.Utilities.SpecValidation;

/// <summary>
/// Base interface for all spec types
/// </summary>
public interface ISpec
{
    /// <summary>
    /// Gets the spec file path
    /// </summary>
    string FilePath { get; }
}

/// <summary>
/// Represents a parsed Page Object specification
/// </summary>
public class PageObjectSpec : ISpec
{
    /// <summary>
    /// Gets or sets the spec file path
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the page name (e.g., "Login", "Inventory")
    /// </summary>
    public string PageName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expected class name (e.g., "LoginPage")
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expected file path for the generated class
    /// </summary>
    public string ExpectedFile { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list of methods defined in the spec
    /// </summary>
    public List<MethodSpec> Methods { get; } = new();
}

/// <summary>
/// Represents a parsed Test specification (unit, integration, or E2E)
/// </summary>
public class TestSpec : ISpec
{
    /// <summary>
    /// Gets or sets the spec file path
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expected class name (e.g., "LoginPageUnitTests")
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the test type (Unit, Integration, E2E)
    /// </summary>
    public TestType TestType { get; set; }

    /// <summary>
    /// Gets or sets the Allure suite name
    /// </summary>
    public string AllureSuite { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Allure feature name
    /// </summary>
    public string AllureFeature { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list of test scenarios defined in the spec
    /// </summary>
    public List<ScenarioSpec> Scenarios { get; } = new();
}

/// <summary>
/// Represents a method specification in a Page Object
/// </summary>
public class MethodSpec
{
    /// <summary>
    /// Gets or sets the method name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list of parameters
    /// </summary>
    public List<ParameterSpec> Parameters { get; } = new();

    /// <summary>
    /// Gets or sets the return type
    /// </summary>
    public string ReturnType { get; set; } = "Task";

    /// <summary>
    /// Gets or sets a value indicating whether the method is async
    /// </summary>
    public bool IsAsync { get; set; } = true;

    /// <summary>
    /// Gets or sets the method description/purpose
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Represents a method parameter specification
/// </summary>
public class ParameterSpec
{
    /// <summary>
    /// Gets or sets the parameter name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parameter type
    /// </summary>
    public string Type { get; set; } = "string";
}

/// <summary>
/// Represents a test scenario specification
/// </summary>
public class ScenarioSpec
{
    /// <summary>
    /// Gets or sets the scenario name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the scenario description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets the Given/When/Then steps
    /// </summary>
    public List<string> GivenWhenThen { get; } = new();

    /// <summary>
    /// Gets the expected assertions
    /// </summary>
    public List<string> ExpectedAssertions { get; } = new();

    /// <summary>
    /// Gets or sets the expected test method name
    /// </summary>
    public string ExpectedMethodName { get; set; } = string.Empty;
}

/// <summary>
/// Enumeration of test types
/// </summary>
public enum TestType
{
    /// <summary>
    /// Unit test (uses Moq, no browser)
    /// </summary>
    Unit,

    /// <summary>
    /// Integration test (real browser, single page)
    /// </summary>
    Integration,

    /// <summary>
    /// End-to-end workflow test (multi-page user journey)
    /// </summary>
    E2E
}
