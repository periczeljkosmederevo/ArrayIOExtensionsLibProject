﻿namespace ArrayIOExtensionsLib.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="ArrayIOExtensions"/> class.
    /// This class is responsible for testing the functionality of saving and
    /// loading single or multi-dimensional arrays to and from text files. 
    /// The tests validate that the data is correctly written to 
    /// and read from the file, ensuring data integrity and correct handling 
    /// of both string arrays and custom object arrays (<see cref="Person"/>). 
    /// Each test method follows the Arrange-Act-Assert 
    /// pattern for clear and organized test cases. 
    /// Cleanup is performed after each 
    /// test to maintain a consistent test environment.
    /// </summary>
    [TestClass()]
    public class ArrayIOExtensionsTests
    {
        // Change to a temp path if needed
        private const string TestFilePath = "testArray.txt";

        /// <summary>
        /// Tests the SaveToTextFile method for saving a 2D array to a text file.
        /// Verifies that the contents of the file match the expected values.
        /// </summary>
        [TestMethod()]
        public void SaveToTextFileTest()
        {
            // Arrange: Create a sample 2D array
            string[,] testArray = new string[,]
            {
                { "John", "Doe", "123456789", "123 Main St" },
                { "Jane", "Smith", "987654321", "456 Elm St" }
            };

            // Act: Save the array to a text file
            testArray.SaveToTextFile(TestFilePath);

            // Assert:
            // Verify that the file contents are as expected
            var expectedLines = new string[]
            {
                "John",
                "Doe",
                "123456789",
                "123 Main St",
                "Jane",
                "Smith",
                "987654321",
                "456 Elm St"
            };

            // Read the contents from the file
            var actualLines = File.ReadAllLines(TestFilePath);

            // Compare the contents
            CollectionAssert.AreEqual(
                expectedLines,
                actualLines,
                "The file contents do not match the expected values.");
        }

        /// <summary>
        /// Tests the LoadFromTextFile method for loading a 2D array from a text file.
        /// Compares the loaded array with the original array to ensure they match.
        /// </summary>
        [TestMethod()]
        public void LoadFromTextFileTest()
        {
            // Arrange: Create a sample 2D array and save it to a text file
            string[,] originalArray = new string[,]
            {
                { "John", "Doe", "123456789", "123 Main St" },
                { "Jane", "Smith", "987654321", "456 Elm St" }
            };
            originalArray.SaveToTextFile(TestFilePath);

            // Act:
            // Create an empty array with the same dimensions as the original
            // and load data from the file
            string[,] loadedArray = new string[2, 4];

            loadedArray.LoadFromTextFile(TestFilePath);

            // Assert: Verify that the loaded array matches the original
            for (int i = 0; i < originalArray.GetLength(0); i++)
            {
                for (int j = 0; j < originalArray.GetLength(1); j++)
                {
                    Assert.AreEqual(
                        originalArray[i, j],
                        loadedArray[i, j],
                        "Loaded value does not match original value.");
                }
            }
        }

        /// <summary>
        /// Tests saving and loading a 2D array of Person objects to and from a text file.
        /// Verifies that the loaded Person objects match the original objects in all properties.
        /// </summary>
        [TestMethod()]
        public void SaveAndLoadPersonArrayTest()
        {
            // Arrange
            var people = new Person[,]
            {
                { new Person { Name = "Alice", Age = 30, IsActive = true },
                    new Person { Name = "Bob", Age = 25, IsActive = false }
                },
                { new Person { Name = "Charlie", Age = 35, IsActive = true },
                    new Person { Name = "Diana", Age = 28, IsActive = false }
                }
            };

            // Act
            people.SaveToTextFile(TestFilePath);

            var loadedPeople = new Person[2, 2];
            loadedPeople.LoadFromTextFile(TestFilePath);

            // Assert
            Assert.AreEqual(people.GetLength(0), loadedPeople.GetLength(0));
            Assert.AreEqual(people.GetLength(1), loadedPeople.GetLength(1));

            for (int i = 0; i < people.GetLength(0); i++)
            {
                for (int j = 0; j < people.GetLength(1); j++)
                {
                    Assert.AreEqual(people[i, j].Name, loadedPeople[i, j].Name);
                    Assert.AreEqual(people[i, j].Age, loadedPeople[i, j].Age);
                    Assert.AreEqual(people[i, j].IsActive, loadedPeople[i, j].IsActive);
                }
            }
        }

        /// <summary>
        /// Tests saving an empty array of Person objects to a text file.
        /// Verifies that the saved data contains the expected number of properties 
        /// with their default values.
        /// </summary>
        [TestMethod()]
        public void SaveEmptyArrayOfPersonsTest()
        {
            // Arrange
            // 3 rows, 2 columns
            Person[,] emptyPeople = new Person[3, 2];

            // Act
            emptyPeople.SaveToTextFile(TestFilePath);

            // Read the saved data back
            var savedData = File.ReadAllLines(TestFilePath);
            // Get number of properties in the Person class
            int expectedNumberOfProperties = typeof(Person).GetProperties().Length;
            // total properties * total elements
            int expectedTotalValues = expectedNumberOfProperties * emptyPeople.Length;

            // Assert
            Assert.AreEqual(
                expectedTotalValues,
                savedData.Length,
                "The number of saved values should match the expected total.");

            // Check if all saved values are the correct
            // default representation for each property
            foreach (var property in typeof(Person).GetProperties())
            {
                // Determine the expected default value for the property
                string? expectedValue = property.PropertyType.IsValueType
                    ? Activator.CreateInstance(property.PropertyType)?.ToString() ?? "null"
                    : "null";

                // Verify if each value in the saved data
                // corresponds to the expected default value
                for (int i = 0; i < emptyPeople.GetLength(0); i++)
                {
                    // Calculate the correct index for saved data
                    int index = (i * expectedNumberOfProperties)
                                    + Array.IndexOf(typeof(Person).GetProperties(), property);

                    // Assert that the saved value matches the expected default value
                    Assert.AreEqual(
                        expectedValue,
                        savedData[index],
                            $"Property '{property.Name}' " +
                            $"should have a default value in the saved data.");
                }
            }
        }

        /// <summary>
        /// Tests saving and loading an empty array of Person objects to and from a text file.
        /// Verifies that the loaded Person objects are not null and have default values for their properties.
        /// </summary>
        [TestMethod()]
        public void SaveAndLoadEmptyPersonArrayTest()
        {
            // Arrange
            // Adjust dimensions as needed
            Person[,] emptyPeople = new Person[2, 3];
            Person[,] loadedData = new Person[2, 3];

            // Act
            emptyPeople.SaveToTextFile(TestFilePath);
            loadedData.LoadFromTextFile(TestFilePath);

            // Assert
            Assert.AreEqual(
                emptyPeople.GetLength(0),
                loadedData.GetLength(0),
                "The number of rows should match.");
            Assert.AreEqual(
                emptyPeople.GetLength(1),
                loadedData.GetLength(1),
                "The number of columns should match.");

            // Check that all properties of
            // loaded Person objects have default values
            foreach (var person in loadedData)
            {
                Assert.IsNotNull(person, "Loaded person object should not be null.");
                // Loop through each property of the Person class
                foreach (var property in typeof(Person).GetProperties())
                {
                    // Get the expected default value for the property type
                    object? expectedDefaultValue = property.PropertyType.IsValueType
                        // Get default for value types (e.g., int = 0, bool = false)
                        ? Activator.CreateInstance(property.PropertyType)
                        : null; // Reference types default to null

                    // Get the actual value of the property from the person object
                    var actualValue = property.GetValue(person);

                    // Compare the actual value with the expected default value
                    Assert.AreEqual(
                        expectedDefaultValue,
                        actualValue,
                        $"Property '{property.Name}' should have the default value.");
                }
            }
        }

        /// <summary>
        /// Cleans up after each test by deleting the test file if it exists.
        /// </summary>
        [TestCleanup()]
        public void Cleanup()
        {
            // Cleanup: Delete the test file after each test
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        /// <summary>
        /// Represents a person with a name, age, and active status.
        /// </summary>
        public class Person
        {
            public string? Name { get; set; }
            public int? Age { get; set; }
            public bool? IsActive { get; set; }
        }
    }
}
