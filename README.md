# ArrayIOExtensionsLibProject

# ArrayIOExtensionsLib

## Overview

`ArrayIOExtensionsLib` is a .NET library that provides extension methods for saving and loading single or multi-dimensional arrays of simple types (such as strings and primitives) to and from text files. This library is designed to work with structured data, handling `null` values and supporting easy serialization and deserialization.

## Key Features

- **Save and Load Multi-dimensional Arrays**: Save arrays of any rank (1D, 2D, etc.) to text files, where each element is stored as a separate line.
- **Support for Simple Types**: Works with arrays of strings, primitives (`int`, `double`, etc.), and custom object types that include only simple properties.
- **Fixed-length Arrays**: The library works with fixed-length arrays. The programmer must anticipate the maximum number of records the array will hold at runtime, as arrays cannot dynamically resize. If the array's size needs to be expanded, a new, larger array must be created, and existing data should be copied over to the new array. This can be done by manually resizing the array and transferring data, or by implementing logic to periodically check if the array is full and perform the resize operation as needed.
- **Null Handling**: `null` values are serialized as `"null"` in the text file and correctly deserialized back into the array.
- **Customizable Encoding**: Allows specifying encoding when saving and loading files (defaults to UTF-8).

## Important Notice

> **Note:** All array fields must be properly initialized before using the extension methods in this library. Uninitialized arrays or `null` values may lead to unexpected behavior during serialization and deserialization.

## Installation

To use this library in your project:

1. **Download or clone** the project. (https://github.com/periczeljkosmederevo/ArrayIOExtensionsLibProject.git)
2. **Build** the project to generate the `.dll` file.
3. **Reference** the generated `.dll` in your own project:
   - **In Visual Studio**: Right-click your project > *Add* > *Reference* > *Browse* to select the generated `.dll`.
   - **In .NET CLI**: Run the following command:
     ```csharp
     dotnet add reference /path/to/ArrayIOExtensions.dll
     ```

## Usage

### 1. Save an Array to a Text File

```csharp
int[,] myArray = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
myArray.SaveToTextFile("output.txt");
```

### 2. Load an Array from a Text File

```csharp
int[,] myArray = new int[3, 2];
myArray.LoadFromTextFile("output.txt");
```

### 3. Saving Custom Types

```csharp
public class Customer
{
    public string? Name { get; set; }
    public string? Address { get; set; }
}

Customer[] customers = new Customer[2];
customers[0] = new Customer { Name = "John", Address = "123 Street" };
customers[1] = new Customer { Name = "Jane", Address = "456 Avenue" };

customers.SaveToTextFile("customers.txt");
```

### 4. Loading Custom Types

```csharp

Customer[] customers = new Customer[2];
customers.LoadFromTextFile("customers.txt");
```

## Licence
This project is licensed under the Creative Commons Zero (CC0) License. 
To the extent possible under law, the author(s) have dedicated all copyright 
and related rights to this software to the public domain worldwide.

For more details, see the LICENSE.

## Contributing
Contributions are welcome! 

# ArrayIOExtensionsLib.Tests

This project contains unit tests for the `ArrayIOExtensionsLib`, which provides extension methods to save and load multi-dimensional arrays to and from text files in .NET. These tests ensure that the library functions as expected for different data types, including strings and custom objects like the `Person` class.

## Features

- **Test methods for saving and loading arrays:**
  - Validates that the library can save 2D arrays of strings and custom objects like `Person` to text files.
  - Tests loading arrays from text files and ensures that the saved data is correctly deserialized.
  
- **Support for complex types:**
  - Tests saving and loading arrays of custom objects such as `Person`, verifying properties such as `Name`, `Age`, and `IsActive`.
  
- **Handles empty arrays and null values:**
  - Ensures proper handling of empty arrays and checks default values when arrays are populated with uninitialized objects.

## Test Cases

- **SaveToTextFileTest**: 
  Verifies that a 2D string array is saved to a text file and that the file contents match the expected values.

- **LoadFromTextFileTest**: 
  Ensures that data loaded from the text file into an array matches the original array.

- **SaveAndLoadPersonArrayTest**: 
  Validates saving and loading arrays of `Person` objects, ensuring that all properties are serialized and deserialized correctly.

- **SaveEmptyArrayOfPersonsTest**: 
  Ensures that an empty array of `Person` objects is saved correctly, with default values for each property.

- **SaveAndLoadEmptyPersonArrayTest**: 
  Tests the handling of arrays of `Person` objects with uninitialized (default) values, ensuring that properties have correct defaults when loaded from a text file.

## Person Class

The `Person` class used in the tests has the following properties:

```csharp
public class Person
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public bool? IsActive { get; set; }
}
```
## Cleanup
Each test automatically deletes the test file (testArray.txt) after execution to ensure that no temporary files remain on the file system.

## Requirements
- .NET SDK 8.0
- MSTest for running the unit tests

## How to Run the Tests
- Clone the repository containing the ArrayIOExtensionsLib project.
- https://github.com/periczeljkosmederevo/ArrayIOExtensionsLibProject
- Build the solution using your preferred IDE (e.g., Visual Studio) or using the .NET CLI.

Run the tests:
- In Visual Studio: Use the built-in Test Explorer.
- Using the .NET CLI: Run dotnet test from the command line in the test project directory.

## Licence
This project is licensed under the Creative Commons Zero (CC0) License. 
To the extent possible under law, the author(s) have dedicated all copyright 
and related rights to this software to the public domain worldwide.

For more details, see the LICENSE.

## Contributing
Contributions are welcome! 
Feel free to submit issues or pull requests to help improve the project.

## Contact
For any questions or issues, 
feel free to reach out at periczeljkosmederevo@yahoo.com.
Feel free to submit issues or pull requests to help improve the project.


