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

## Cleanup
Each test automatically deletes the test file (testArray.txt) after execution to ensure that no temporary files remain on the file system.

## Requirements
- .NET SDK 8.0
- MSTest for running the unit tests

## How to Run the Tests
- Clone the repository containing the ArrayIOExtensionsLib project.
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
