# {PageName} Page Object - Integration Tests Specification

<!--
INSTRUCTIONS:
- Replace {PageName} with your page name
- Replace {Feature} with the Allure feature name
- Define test scenarios that validate Page Object with real browser
- Focus on WHAT to test, not HOW to code
- Include both positive and negative scenarios
-->

## Test Suite Overview

**Test Class**: {PageName}PageIntegrationTests
**What We're Testing**: {PageName}Page with real Playwright browser
**Test Type**: Integration Tests
**Allure Suite**: "Integration Tests"
**Allure Feature**: "{Feature}"

### Purpose
Verify {PageName}Page works with a real browser and the actual website. Tests check that selectors are correct and behavior works as expected.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: {URL of the page being tested}

---

## Test Scenarios

### Test: {Method} With Valid Input

**What we verify**: {Brief description of what should happen}

**Test Data**:
- {param1}: {value}
- {param2}: {value}

**Steps**:
1. Navigate to page
2. Call {Method}
3. Check expected outcome

**Expected**:
- {Expected state 1}
- {Expected state 2}

**Severity**: {Critical/Normal}
**Tags**: Integration, {PositiveArea}

---

### Test: {Method} With Invalid Input

**What we verify**: {What should happen with bad input}

**Test Data**:
- {param1}: {invalid value}

**Steps**:
1. Navigate to page
2. Call {Method} with invalid data
3. Check error appears

**Expected**:
- {Error indicator shows}
- {Stays on current page/shows message}

**Severity**: {Critical/Normal}
**Tags**: Integration, Negative, Validation

---

## Coverage

**What's covered**:
- Page Object + Playwright integration
- Correct selectors
- Success scenarios
- Error scenarios

**What's NOT covered**:
- Complete user workflows (E2E tests handle that)
