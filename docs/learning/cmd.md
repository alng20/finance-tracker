# dotnet commands

### Compiles the project or solutions
```bash
dotnet build
```

### Show all NuGet packages for current project
```bash
dotnet package list
dotnet list src/FinanceTracker.Domain package
```

### Downloads and restores dependencies and NuGet packages listed in the project
```bash
dotnet restore
```

### Compiles and immediately executes the application from source code
```bash
dotnet run
```

### Executes unit tests using the designated test runner
```bash
dotnet test
```

### Cleans the build outputs (removes files from the bin and obj directories)

```bash
dotnet clean
```

### Install NuGet packages
```bash
dotnet add src/FinanceTracker.Infrastructure package Microsoft.EntityFrameworkCore.Design
```

