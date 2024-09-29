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
Feel free to submit issues or pull requests to help improve the project.

## Contact
For any questions or issues, 
feel free to reach out at periczeljkosmederevo@yahoo.com.
