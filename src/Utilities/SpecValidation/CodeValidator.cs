using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace csharp_framework_demo.Utilities.SpecValidation;

/// <summary>
/// Validates that generated code conforms to specifications
/// </summary>
public class CodeValidator
{
    /// <summary>
    /// Validates a Page Object class against its specification
    /// </summary>
    /// <param name="spec">The Page Object specification</param>
    /// <param name="projectRoot">Root directory of the project</param>
    /// <returns>Validation result</returns>
    public static ValidationResult ValidatePageObject(PageObjectSpec spec, string projectRoot)
    {
        var result = new ValidationResult();

        // Check if file exists
        var fullPath = Path.Combine(projectRoot, spec.ExpectedFile);
        if (!File.Exists(fullPath))
        {
            result.AddError("PO001", $"Page Object file not found: {spec.ExpectedFile}");
            return result;
        }

        // TODO: Future enhancements:
        // - Load and parse C# code
        // - Validate class name matches spec
        // - Validate namespace
        // - Validate constructor accepts IPage parameter
        // - Validate public methods match spec methods
        // - Validate method signatures (parameters, return types)
        // - Validate all methods are async

        result.AddWarning("PO-FUTURE", "Full code validation not yet implemented");

        return result;
    }

    /// <summary>
    /// Validates a Test class against its specification
    /// </summary>
    /// <param name="spec">The Test specification</param>
    /// <param name="projectRoot">Root directory of the project</param>
    /// <returns>Validation result</returns>
    public static ValidationResult ValidateTestClass(TestSpec spec, string projectRoot)
    {
        var result = new ValidationResult();

        // Determine expected file path based on test type
        var expectedFile = $"src/Tests/{spec.ClassName}.cs";
        var fullPath = Path.Combine(projectRoot, expectedFile);

        if (!File.Exists(fullPath))
        {
            result.AddError("TEST001", $"Test file not found: {expectedFile}");
            return result;
        }

        // TODO: Future enhancements:
        // - Load and parse C# code
        // - Validate class name matches spec
        // - Validate [AllureSuite] attribute value
        // - Validate [AllureFeature] attribute value
        // - Validate test methods have [Fact] attribute
        // - Validate test methods have required Allure attributes
        // - Validate test method names match spec scenarios
        // - For integration/E2E: validate IClassFixture<PlaywrightFixture>
        // - For unit: validate no PlaywrightFixture usage
        // - For E2E: validate AllureApi.Step() usage

        result.AddWarning("TEST-FUTURE", "Full code validation not yet implemented");

        return result;
    }

    /// <summary>
    /// Validates that a type has a specific attribute
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <param name="attributeType">The attribute type to look for</param>
    /// <returns>True if attribute is present</returns>
    public static bool HasAttribute(Type type, Type attributeType)
    {
        return type.GetCustomAttributes(attributeType, inherit: true).Any();
    }

    /// <summary>
    /// Gets the value of a specific attribute property
    /// </summary>
    /// <typeparam name="TAttribute">Attribute type</typeparam>
    /// <typeparam name="TValue">Property value type</typeparam>
    /// <param name="type">The type to check</param>
    /// <param name="propertyName">Property name to read</param>
    /// <returns>Property value or default</returns>
    public static TValue? GetAttributeValue<TAttribute, TValue>(Type type, string propertyName)
        where TAttribute : Attribute
    {
        var attribute = type.GetCustomAttribute<TAttribute>(inherit: true);
        if (attribute == null)
        {
            return default;
        }

        var property = typeof(TAttribute).GetProperty(propertyName);
        if (property == null)
        {
            return default;
        }

        var value = property.GetValue(attribute);
        return value is TValue typedValue ? typedValue : default;
    }

    /// <summary>
    /// Validates that a method signature matches expected parameters and return type
    /// </summary>
    /// <param name="method">The method to validate</param>
    /// <param name="expectedReturnType">Expected return type name</param>
    /// <param name="expectedParams">Expected parameter types</param>
    /// <returns>True if signature matches</returns>
    public static bool ValidateMethodSignature(MethodInfo method, string expectedReturnType, params Type[] expectedParams)
    {
        // Check return type
        if (method.ReturnType.Name != expectedReturnType)
        {
            return false;
        }

        // Check parameters
        var parameters = method.GetParameters();
        if (parameters.Length != expectedParams.Length)
        {
            return false;
        }

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType != expectedParams[i])
            {
                return false;
            }
        }

        return true;
    }
}
