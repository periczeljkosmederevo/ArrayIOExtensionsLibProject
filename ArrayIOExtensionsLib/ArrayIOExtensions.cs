using System.IO;
using System.Text;
using System;

namespace ArrayIOExtensionsLib
{
    /// <summary>
    /// Provides extension methods for saving and loading 
    /// single or multi-dimensional arrays to and from text files.
    /// IMPORTANT: Values in the array must be properly initialized
    /// before calling the save methods, especially when working with
    /// arrays of complex or custom types. If not properly initialized, 
    /// null values in objects may lead to incorrect written values 
    /// in the text file, as the object will not be serialized with 
    /// all its properties.
    /// </summary>
    public static class ArrayIOExtensions
    {
        #region Properties

        /// <summary>
        /// Represents the default encoding used for saving arrays to text files.
        /// </summary>
        private static readonly Encoding _defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Represents the default word used for null value.
        /// </summary>
        private static readonly string _nullRepresentation = "null";

        #endregion

        #region Save To Text File

        /// <summary>
        /// Saves a single or multi-dimensional array to a specified text file.
        /// Each element of the array is written to a new line in the file.
        /// IMPORTANT: Values in the array must be properly initialized before
        /// calling the save methods, especially when working with arrays
        /// of complex or custom types. If not properly initialized, 
        /// null values in objects may lead to incorrect written values
        /// in the text file, as the object will not be serialized 
        /// with all its properties.
        /// </summary>
        /// <param name="array">The single or multi-dimensional array to save.</param>
        /// <param name="filePath">The path of the file where the array will be saved.</param>
        /// <param name="encoding">Encoding used for saving arrays to text files.</param>
        /// <exception cref="ArgumentNullException">Thrown when the array is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the file path is null or whitespace.</exception>
        public static void SaveToTextFile(this Array array,
                                          string filePath,
                                          Encoding? encoding = null)
        {
            // Validate the array and file path before proceeding.
            ValidateArrayAndFilePath(array, filePath);

            try
            {
                // Check encoding and set the default encoding if not specified.
                encoding ??= _defaultEncoding;

                // Create a StreamWriter to write to the specified file using UTF-8 encoding.
                using var writer = new StreamWriter(filePath, false, encoding);

                // Start the process of saving the array to the text file.
                SaveArrayToTextFile(array, writer);
            }
            catch (Exception ex)
            {
                // Throw an exception if an error occurs during file writing.
                throw new ArgumentException(
                    $"An error occurred while saving to file: {ex.Message}");
            }
        }

        /// <summary>
        /// Initializes the dimensions array and starts the recursive saving of the array.
        /// </summary>
        /// <param name="array">The single or multi-dimensional array to save.</param>
        /// <param name="writer">The StreamWriter used for writing to the file.</param>
        private static void SaveArrayToTextFile(Array array, StreamWriter writer)
        {
            // Create an array to hold the length
            // of each dimension of the array.
            int[] dimensions = new int[array.Rank];
            for (int i = 0; i < array.Rank; i++)
            {
                dimensions[i] = array.GetLength(i);
            }

            // Get the element type of the array.
            var elementType = array.GetType().GetElementType();

            // Begin the recursive method to save
            // the array elements to the text file.
            SaveArrayToTextFileRecursive(array,
                                         writer,
                                         dimensions,
                                         new int[array.Rank],
                                         0,
                                         elementType!);
        }

        /// <summary>
        /// Recursively writes the contents of 
        /// the single or multi-dimensional array to the StreamWriter.
        /// </summary>
        /// <param name="array">The single or multi-dimensional array to save.</param>
        /// <param name="writer">The StreamWriter used for writing to the file.</param>
        /// <param name="dimensions">The lengths of each dimension of the array.</param>
        /// <param name="indices">The current indices in the array being processed.</param>
        /// <param name="currentIndex">The current dimension index being processed.</param>
        /// <param name="elementType">The type of the object to serialize.</param>
        private static void SaveArrayToTextFileRecursive(Array array,
                                                         StreamWriter writer,
                                                         int[] dimensions,
                                                         int[] indices,
                                                         int currentIndex,
                                                         Type elementType)
        {
            // Shortcut to handle single-dimensional arrays directly
            if (array.Rank == 1)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    // Get the value of the single-dimensional array
                    var value = array.GetValue(i);

                    // Serialize the value
                    SerializeObject(writer, value, elementType);
                }
                return; 
                // Exit early since we've already processed the entire array
            }

            // If we are at the last dimension, write the values to the file.
            if (currentIndex == array.Rank - 1)
            {
                for (int i = 0; i < dimensions[currentIndex]; i++)
                {
                    indices[currentIndex] = i; // Set the index for the last dimension.
                    var value = array.GetValue(indices);

                    SerializeObject(writer, value, elementType);
                }
            }
            else
            {
                // Iterate through the current dimension
                // and recurse into the next dimension.
                for (int i = 0; i < dimensions[currentIndex]; i++)
                {
                    indices[currentIndex] = i;
                    SaveArrayToTextFileRecursive(array,
                                                 writer,
                                                 dimensions,
                                                 indices,
                                                 currentIndex + 1,
                                                 elementType);
                }
            }
        }

        #endregion

        #region Load From Text File

        /// <summary>
        /// Loads data from a specified text file and populates the single or multi-dimensional array.
        /// Each line in the file corresponds to a single element of the array.
        /// </summary>
        /// <param name="array">The single or multi-dimensional array to populate.</param>
        /// <param name="filePath">The path of the file from which to load the array.</param>
        /// <param name="encoding">Encoding used for reading the text file.</param>
        /// <exception cref="ArgumentNullException">Thrown when the array is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the file path is null or whitespace.</exception>
        public static void LoadFromTextFile(this Array array,
                                            string filePath,
                                            Encoding? encoding = null)
        {
            // Validate the array and file path before proceeding.
            ValidateArrayAndFilePath(array, filePath);

            try
            {
                // Start the process of loading the array from the text file.
                LoadArrayFromTextFile(array, filePath, encoding);
            }
            catch (Exception ex)
            {
                // Throw an exception if an error occurs during file reading.
                throw new ArgumentException(
                    $"An error occurred while loading from file: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the single or multi-dimensional array from the specified file.
        /// Each line in the file corresponds to a single element of the array.
        /// </summary>
        /// <param name="array">The single or multi-dimensional array to populate.</param>
        /// <param name="filePath">The path of the file from which to load the array.</param>
        /// <param name="encoding">Encoding used for reading the text file.</param>
        private static void LoadArrayFromTextFile(Array array,
                                                  string filePath,
                                                  Encoding? encoding)
        {
            try
            {
                // Check encoding and set the default encoding if not specified.
                // Default encoding is UTF-8.
                encoding ??= Encoding.UTF8;

                // Read all lines from the specified file using the specified encoding.
                var lines = File.ReadAllLines(filePath, encoding);

                // Get the element type of the array.
                var elementType = array.GetType().GetElementType();

                // Validate that the number of lines read matches
                // the total number of elements in the array.
                // This also accounts for complex types with multiple properties.
                ValidateArrayAndFileLength(array.Length, lines.Length, elementType!);

                // Fill the array with the values from the lines.
                var lineIndex = 0;
                FillArray(array,
                          lines,
                          ref lineIndex,
                          new int[array.Rank],
                          0,
                          elementType!);
            }
            catch (Exception ex)
            {
                // Throw an exception if an error occurs during file reading.
                throw new ArgumentException(
                    $"An error occurred while loading from file: {ex.Message}");
            }
        }

        /// <summary>
        /// Fills the single or multi-dimensional array with values from the lines.
        /// </summary>
        /// <param name="array">The single or multi-dimensional array to populate.</param>
        /// <param name="lines">The lines from the file.</param>
        /// <param name="lineIndex">The current line index being processed.</param>
        /// <param name="indices">Current indices for the single or multi-dimensional array.</param>
        /// <param name="currentDimension">The current dimension being processed.</param>
        /// <param name="elementType">The type of elements in the array.</param>
        private static void FillArray(Array array,
                                      string[] lines,
                                      ref int lineIndex,  // Pass lineIndex by reference
                                      int[] indices,
                                      int currentDimension,
                                      Type elementType)
        {
            // If we reach the last dimension, fill the values directly.
            if (currentDimension == array.Rank - 1)
            {
                for (int i = 0; i < array.GetLength(currentDimension); i++)
                {
                    indices[currentDimension] = i;
                    if (lineIndex < lines.Length)
                    {
                        // Deserialize object from consecutive lines
                        object? deserializedValue = DeserializeObject(lines,
                                                                      ref lineIndex,
                                                                      elementType);

                        array.SetValue(deserializedValue, indices);
                    }
                }
            }
            else
            {
                // Recur for the next dimension.
                for (int i = 0; i < array.GetLength(currentDimension); i++)
                {
                    indices[currentDimension] = i;
                    FillArray(array,
                              lines,
                              ref lineIndex,  // Pass lineIndex by reference
                              indices,
                              currentDimension + 1,
                              elementType);
                }
            }
        }

        #endregion

        #region Serialization & Deserialization

        /// <summary>
        /// Serializes an object to the StreamWriter.
        /// </summary>
        /// <param name="writer">The StreamWriter used for writing to the file.</param>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="elementType">The type of the object to serialize.</param>
        private static void SerializeObject(StreamWriter writer,
                                            object? obj,
                                            Type elementType)
        {
            // Check if the object is null
            if (obj == null)
            {
                if (elementType == typeof(string) || elementType.IsPrimitive)
                {
                    writer.WriteLine(_nullRepresentation);
                    return;
                }

                // Attempt to create a new instance if it is a complex type
                // Assuming you know the type or have a way to get it
                obj = Activator.CreateInstance(elementType);
            }

            // Now serialize the object
            if (obj is string || obj!.GetType().IsPrimitive)
            {
                writer.WriteLine(obj.ToString());
            }
            else
            {
                // Write each property on a new line
                var properties = obj.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var value = 
                        property.GetValue(obj)?.ToString() ?? _nullRepresentation;
                    writer.WriteLine(value);
                }
            }
        }


        /// <summary>
        /// Deserializes an object from the lines.
        /// </summary>
        /// <param name="lines">The lines from the file.</param>
        /// <param name="lineIndex">The current line index being processed.</param>
        /// <param name="targetType">The type of the object to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        private static object? DeserializeObject(string[] lines,
                                                 ref int lineIndex,
                                                 Type targetType)
        {
            if (lineIndex >= lines.Length)
            {
                return null; 
                // or throw an exception, based on your design choice
            }

            if (targetType == typeof(string))
            {
                // Check if the current line is "null", and return null if it is
                if (lines[lineIndex] == _nullRepresentation)
                {
                    lineIndex++; // Move to the next line
                    return null;
                }

                return lines[lineIndex++];
            }

            if (targetType.IsPrimitive)
            {
                // Check if the current line is "null", and return null if it is
                if (lines[lineIndex] == _nullRepresentation)
                {
                    lineIndex++; // Move to the next line
                    return null;
                }

                return Convert.ChangeType(lines[lineIndex++], targetType);
            }

            // Create an instance of the target object
            var obj = Activator.CreateInstance(targetType);
            var properties = targetType.GetProperties();

            foreach (var property in properties)
            {
                if (lineIndex < lines.Length)
                {
                    // Read the current line
                    string lineValue = lines[lineIndex++];

                    // If the value is represented as 'null',
                    // assign null to the property
                    if (lineValue == _nullRepresentation)
                    {
                        property.SetValue(obj, null);
                    }
                    else
                    {
                        // Get the property type
                        Type propertyType = property.PropertyType;

                        // Check if the property type is nullable
                        bool isNullable = Nullable.GetUnderlyingType(propertyType) != null;

                        // Handle non-null values
                        Type? targetTypeToConvert = isNullable
                            ? Nullable.GetUnderlyingType(propertyType)
                            : propertyType;

                        // Convert the value to the correct type
                        var convertedValue = Convert.ChangeType(lineValue, targetTypeToConvert!);

                        // Set the property value
                        property.SetValue(obj, convertedValue);
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException(
                        $"Not enough lines to deserialize all properties.");
                }
            }

            return obj;
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates the input array and file path for null or invalid values.
        /// </summary>
        /// <param name="array">The array to validate.</param>
        /// <param name="filePath">The file path to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when the array is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the file path is null or whitespace.</exception>
        private static void ValidateArrayAndFilePath(Array array, string filePath)
        {
            if (array == null)
            {
                throw new ArgumentNullException(
                    nameof(array),
                    "Array cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(
                    "File path cannot be null or whitespace.",
                    nameof(filePath));
            }
        }

        /// <summary>
        /// Validates the number of lines read from the file 
        /// against the total elements in the array, 
        /// accounting for custom object properties.
        /// </summary>
        /// <param name="totalElements">Total number of elements in the array.</param>
        /// <param name="lines">Number of lines read from the file.</param>
        /// <param name="elementType">The type of elements in the array.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the number of lines does not match the number of elements in the array.
        /// </exception>
        private static void ValidateArrayAndFileLength(int totalElements,
                                                       int lines,
                                                       Type elementType)
        {
            int expectedLines = totalElements;

            // Check if the element type is a custom class (i.e., not primitive or string).
            if (!elementType.IsPrimitive && elementType != typeof(string))
            {
                // For custom types, calculate the expected number of lines
                // as the total number of elements multiplied by the number of properties.
                int propertyCount = elementType.GetProperties().Length;
                expectedLines *= propertyCount;
            }

            // Compare the number of lines in the file with the expected number of lines.
            if (lines != expectedLines)
            {
                throw new ArgumentException(
                    $"The number of lines in the file ({lines}) " +
                    $"does not match the expected number of lines " +
                    $"({expectedLines}) for the array.");
            }
        }

        #endregion
    }
}
