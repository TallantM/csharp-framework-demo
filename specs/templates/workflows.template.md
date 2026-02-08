# {Feature} - End-to-End Workflow Specification

<!--
INSTRUCTIONS:
- Replace {Feature} with the feature name (e.g., "Authentication", "Shopping Cart")
- Define complete user workflows that span multiple pages/actions
- Use user story format and clear steps
- Focus on WHAT the workflow does, not HOW to code it
- Each workflow should tell a complete user story
-->

## Test Suite Overview

**Test Class**: {Feature}WorkflowTests
**What We're Testing**: Complete {feature} workflows from end to end
**Test Type**: End-to-End Tests
**Allure Suite**: "End-to-End Tests"
**Allure Feature**: "{Feature}"

### Purpose
Test complete user journeys involving {feature}. These verify the entire flow works correctly from start to finish.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: {Base URL of application}
**Test Users**: {List test accounts or data needed}

---

## Workflow Scenarios

### Workflow: {Main Happy Path Name}

**User Story**: {As a [user], I want to [action], so that [benefit]}

**Steps**:
1. {First step description}
2. {Second step description}
3. {Third step description}
4. {Verification step}

**Test Data**:
- {data1}: {value}
- {data2}: {value}

**Expected Outcome**:
- {What should happen at the end}
- {Additional expected state}

**Severity**: Critical
**Tags**: Smoke, {Feature}

---

### Workflow: {Alternative Success Path}

**User Story**: {As a [user], I want to [alternative action], so that [benefit]}

**Steps**:
1. {Step 1}
2. {Step 2}
3. {Step 3}

**Test Data**:
- {data}

**Expected Outcome**:
- {Expected result}

**Severity**: Normal
**Tags**: Regression, {Feature}

---

### Workflow: {Error Scenario}

**User Story**: {When I [invalid action], I should see [error feedback]}

**Steps**:
1. {Setup step}
2. {Perform invalid action}
3. {Verify error}

**Test Data**:
- {invalid data}

**Expected Outcome**:
- {Error message shows}
- {User stays on page/sees correct feedback}

**Severity**: Critical
**Tags**: Smoke, Negative, Validation

---

## Reporting

**Allure Steps**: Each workflow step should be wrapped in `AllureApi.Step()` for granular reporting

---

## Coverage

**What's covered**:
- {Main flow}
- {Alternative flows}
- {Error scenarios}
- {Edge cases}

**Scenarios Tested**:
- ✅ {Scenario 1}
- ✅ {Scenario 2}
- ✅ {Scenario 3}

---

## Notes

**External Dependency**: {List any external dependencies like test site availability}
**Test Data**: {Where test data comes from}
