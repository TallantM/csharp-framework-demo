# Agent Patterns for Spec-Driven Development

This document contains reusable agent patterns for implementing Spec-Driven Development (SDD) in this C# Playwright test automation framework.

## Overview

These agent patterns automate the SDD workflow:
1. **Spec Generation Agent** - Creates specification files from existing code
2. **Code Generation Agent** - Generates C# code from specification files

Both agents follow the patterns defined in `specs/META-FRAMEWORK.md` and use the templates in `specs/templates/`.

---

## Pattern 1: Spec Generation Agent

### Purpose
Reverse-engineers specification files from existing code to establish a baseline or document legacy features.

### When to Use
- Documenting existing features that don't have specs yet
- Creating baseline specs for legacy code
- Establishing spec patterns for a new project

### Agent Configuration
- **Type:** `general-purpose`
- **Expected Duration:** ~5-20 minutes depending on feature count
- **Output:** 4 spec files per feature in `specs/features/{feature}/`

### Agent Prompt Template

```
You are tasked with creating Spec-Driven Development (SDD) specification files for features in this C# Playwright test automation framework.

## Context
We've successfully established the SDD meta-framework and created specs for the Authentication feature as a baseline. Now we need specs for the remaining features.

## Your Task
Create specification files for these features in order:
1. **{Feature1}** ({PageClass})
2. **{Feature2}** ({PageClass})
3. **{Feature3}** ({PageClass})
[Add more as needed]

## Pattern to Follow
For EACH feature, create 4 spec files in `specs/features/{feature}/`:
- `page-objects.md` - Page Object specification
- `unit-tests.md` - Unit test scenarios
- `integration-tests.md` - Integration test scenarios
- `workflows.md` - E2E workflow scenarios

## Resources Available
1. **Templates:** Use `specs/templates/*.template.md` as guides
2. **Example:** Study `specs/features/authentication/` to match the style
3. **Existing Code:** Read existing Page Object files in `src/Pages/` to understand what exists

## Critical Guidelines
1. **Concise & Natural:** Use human-friendly language, not formal documentation style
2. **WHAT not HOW:** Describe behavior, not implementation details
3. **No Code Dumps:** Avoid putting code examples in specs
4. **Reverse-Engineer:** Base specs on existing code in `src/Pages/` and `src/Tests/`
5. **Match Authentication Style:** Keep same tone/structure as authentication specs (80-125 lines per file)

## Step-by-Step Process
For each feature:
1. Read the existing Page Object code (e.g., `src/Pages/{PageClass}.cs`)
2. Read existing tests that use that page
3. Create the 4 spec files following the template structure
4. Keep specs concise (target 80-280 lines each)
5. Move to next feature

## Important Notes
- Do NOT generate any C# code - only create spec files
- Do NOT modify existing code files
- Focus on documenting what currently exists
- Use the same section structure as authentication specs
- Report back when all features are complete

Start with the first feature and work through all features systematically.
```

### Example Usage

```typescript
// In Claude Code conversation:
Task({
  subagent_type: "general-purpose",
  description: "Create specs for remaining features",
  prompt: [Use template above with specific features listed]
})
```

### Expected Output
- 4 specification files per feature
- Total lines: ~400-800 per feature (all 4 files combined)
- Consistent style matching existing authentication specs
- Natural language, no code examples

---

## Pattern 2: Code Generation Agent

### Purpose
Generates C# code (Page Objects and test classes) from specification files following SDD principles.

### When to Use
- After specs have been written and reviewed
- Implementing new features spec-first
- Updating code to match evolved specs

### Agent Configuration
- **Type:** `general-purpose`
- **Expected Duration:** ~10-30 minutes depending on feature count
- **Output:** Page Object classes and test classes in appropriate directories

### Agent Prompt Template

```
You are tasked with generating C# code from Spec-Driven Development (SDD) specification files for this Playwright test automation framework.

## Context
We have complete specification files for multiple features. Your job is to generate the corresponding C# Page Object classes and test classes.

## Your Task
Generate code for these features in order:
1. **{Feature1}** - specs in `specs/features/{feature1}/`
2. **{Feature2}** - specs in `specs/features/{feature2}/`
3. **{Feature3}** - specs in `specs/features/{feature3}/`
[Add more as needed]

## Pattern to Follow
For EACH feature, generate/update:
1. **Page Object** - `src/Pages/{FeatureName}Page.cs`
   - Based on `specs/features/{feature}/page-objects.md`
   - Follow existing Page Object patterns in the codebase
   - Use Playwright's IPage, ILocator interfaces
   - Implement all methods described in spec

2. **Unit Tests** - `src/Tests/Unit/{FeatureName}PageTests.cs`
   - Based on `specs/features/{feature}/unit-tests.md`
   - Use xUnit and Moq for mocking
   - Test all scenarios described in spec
   - Mock IPage and ILocator dependencies

3. **Integration Tests** - `src/Tests/Integration/{FeatureName}Tests.cs`
   - Based on `specs/features/{feature}/integration-tests.md`
   - Use real Playwright browser (via PlaywrightFixture)
   - Test all scenarios with real browser
   - Use Playwright assertions

4. **Workflow Tests** - `src/Tests/Workflows/{FeatureName}WorkflowTests.cs`
   - Based on `specs/features/{feature}/workflows.md`
   - Use real Playwright browser (via PlaywrightFixture)
   - Implement complete E2E scenarios
   - Use Allure attributes for reporting

## Critical Guidelines
1. **Read Specs Carefully:** Implement exactly what specs describe, no more, no less
2. **Follow Existing Patterns:** Match coding style of existing classes
3. **Use Proper Namespaces:** Follow project namespace conventions
4. **Add Allure Attributes:** Use [AllureSuite], [AllureFeature], [AllureDescription], etc.
5. **AAA Pattern:** Arrange-Act-Assert for all tests
6. **Async/Await:** All Playwright operations are async
7. **Check Existing Code:** Read similar files before generating to match style

## File Organization
```
src/
├── Pages/
│   └── {FeatureName}Page.cs
├── Tests/
│   ├── Unit/
│   │   └── {FeatureName}PageTests.cs
│   ├── Integration/
│   │   └── {FeatureName}Tests.cs
│   └── Workflows/
│       └── {FeatureName}WorkflowTests.cs
```

## Step-by-Step Process
For each feature:
1. Read all 4 spec files for the feature
2. Read existing similar Page Objects (e.g., LoginPage.cs) to understand patterns
3. Generate Page Object class matching spec
4. Generate unit test class matching spec scenarios
5. Generate integration test class matching spec scenarios
6. Generate workflow test class matching spec scenarios
7. Verify all using statements are correct
8. Move to next feature

## Important Notes
- If a file already exists, UPDATE it to match the spec (don't duplicate)
- Preserve any existing code not covered by specs
- Use appropriate using statements (Microsoft.Playwright, Xunit, Moq, Allure.Net.Commons, etc.)
- All test methods should have proper Allure attributes
- Follow the Page Object Model pattern strictly
- Report back when all features are complete

Start with the first feature and work through all features systematically.
```

### Example Usage

```typescript
// In Claude Code conversation:
Task({
  subagent_type: "general-purpose",
  description: "Generate code from feature specs",
  prompt: [Use template above with specific features listed]
})
```

### Expected Output
- Page Object class for each feature
- Unit test class for each feature (~9-12 test methods)
- Integration test class for each feature (~8-11 test methods)
- Workflow test class for each feature (~8-9 test methods)
- All code compiles and follows existing patterns
- All Allure attributes properly applied

---

## Workflow: Complete SDD Process

### End-to-End Flow

```
1. Write/Generate Specs
   └─> Use Spec Generation Agent (Pattern 1)
   └─> OR write specs manually using templates

2. Review Specs
   └─> Human reviews for accuracy
   └─> Make any necessary adjustments

3. Generate Code
   └─> Use Code Generation Agent (Pattern 2)
   └─> Agent creates Page Objects and test classes

4. Validate
   └─> Run conformance tests (not yet implemented for feature-level)
   └─> Run unit tests locally
   └─> Run integration tests locally
   └─> Fix any issues

5. Commit Together
   └─> Commit specs and code in same commit
   └─> Both artifacts stay synchronized

6. CI/CD
   └─> GitHub Actions runs all tests
   └─> Conformance tests validate meta-framework
   └─> Tests validate functionality
```

### Quality Checklist

Before committing:
- [ ] All 4 spec files exist for each feature
- [ ] Specs are concise and natural language
- [ ] All 4 code files exist for each feature (Page Object + 3 test classes)
- [ ] Code matches spec descriptions
- [ ] All tests compile
- [ ] All tests pass locally
- [ ] Allure attributes applied
- [ ] Code follows existing patterns

---

## Tips & Best Practices

### For Spec Generation
- Start with one example feature (like Authentication) to establish the pattern
- Keep specs short and focused on WHAT not HOW
- Use tables for elements/selectors - they're easy to read
- Natural language > technical jargon

### For Code Generation
- Always read existing code first to match style
- Verify using statements are complete
- Run tests incrementally (don't wait until all features are done)
- Update specs if you discover gaps during implementation

### For Both
- Use the `resume` parameter to continue long-running agents
- Monitor agent output for errors
- Don't hesitate to stop and correct if agent goes off-track
- Save agent IDs for resumption if needed

---

## Maintenance

### When to Update This Document
- New agent patterns are discovered
- Existing patterns evolve
- Common issues/solutions are identified
- Project structure changes

### Version History
- **v1.0** (2026-02-08) - Initial documentation with Spec Generation and Code Generation patterns
