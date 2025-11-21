# Versioning Strategy

This project uses automatic versioning based on the branch and commit history.

## Version Formats

### Develop Branch (Alpha Packages)
- **Format**: `{BASE_VERSION}-alpha.{COMMIT_COUNT}+{SHORT_SHA}`
- **Example**: `1.0.0-alpha.42+a1b2c3d`
- **Trigger**: Push to `develop` branch
- **Purpose**: Pre-release testing and validation

### Master Branch (Release Packages)
- **Format**: `{BASE_VERSION}.{COMMIT_COUNT}`
- **Example**: `1.0.0.123`
- **Trigger**: Push to `master` branch
- **Purpose**: Stable production releases

### Tagged Releases
- **Format**: Version from git tag (e.g., `v1.2.0` → `1.2.0`)
- **Trigger**: GitHub release published
- **Purpose**: Official versioned releases

### Manual Workflow Dispatch
- **Format**: User-specified version
- **Trigger**: Manual workflow run with version input
- **Purpose**: Custom version deployment

## How It Works

1. **CI Workflow** runs on every push to `develop` or `master` branches
   - Builds the project
   - Runs all tests
   - Generates code coverage reports

2. **CD Workflow** runs on push to `develop` or `master` branches
   - Automatically calculates the version based on the branch
   - Builds and packs the NuGet package
   - Publishes to NuGet.org and GitHub Packages

## NuGet Version Precedence

According to [SemVer 2.0.0](https://semver.org/):
- **Release versions** (e.g., `1.0.0.123`) are considered stable
- **Pre-release versions** (e.g., `1.0.0-alpha.42`) are considered unstable
- Users installing without version constraints will get the latest stable version (from master)
- Users can opt-in to alpha versions with: `dotnet add package BankingHelper --version 1.0.0-alpha.*`

## Updating Base Version

To update the base version:
1. Edit `BASE_VERSION` in `.github/workflows/cd.yml` (line ~38)
2. Update `<Version>` in `src/BankingHelper.Belgium/BankingHelper.Belgium.csproj`

## Example Workflow

```bash
# Work on feature
git checkout develop
git commit -m "Add new feature"
git push origin develop
# → Publishes: 1.0.0-alpha.42+a1b2c3d

# Merge to master for release
git checkout master
git merge develop
git push origin master
# → Publishes: 1.0.0.43

# Create official release (optional)
git tag v1.1.0
git push origin v1.1.0
# Create GitHub release
# → Publishes: 1.1.0
```
