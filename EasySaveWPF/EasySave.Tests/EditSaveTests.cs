using Xunit;
using System.IO;
using EasySaveClasses.ViewModelNS;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace EasySave.Tests
{

    public class EditSaveTests
    {
        // Test case for Create method when a new folder is successfully created
        //[Fact]
        //public void Create_NewFolderCreated_ReturnsValidPath()
        //{
        //    var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
        //    var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
        //    var dirPath = Path.GetDirectoryName(codeBasePath);

        //    var newPath = Path.Combine(dirPath, "Resources"); // Corrected: Removed "./" from the path

        //    // Arrange
        //    string sourceFolder = Path.Combine(newPath, "Folder1"); // Use Path.Combine for consistency
        //    string destinationDirectory = Path.Combine(newPath, "Folder2"); // Use Path.Combine for consistency

        //    // Act
        //    string result = EditSave.Create(sourceFolder, destinationDirectory);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.True(Directory.Exists(result));

        //    if (Directory.Exists(result))
        //        Directory.Delete(result, recursive: true);
        //}

        // Test case for Create method when destination directory doesn't exist
        //[Fact]
        //public void Create_DestinationDirectoryNotExists_ReturnsNull()
        //{
        //    // Arrange
        //    string sourceFolder = "source";
        //    string destinationDirectory = "nonexistent_directory";
        //    int saveType = 1;

        //    // Act
        //    string result = EditSave.Create(sourceFolder, destinationDirectory, saveType);

        //    // Assert
        //    Assert.Null(result);
        //}

        // Test case for Delete method when folder exists and gets deleted
        [Fact]
        public void Delete_FolderExists_DeletesFolder()
        {
            // Arrange
            string folderToDelete = "folder_to_delete";
            Directory.CreateDirectory(folderToDelete);

            // Act
            bool result = EditSave.Delete(folderToDelete);

            // Assert
            Assert.True(result);
            Assert.False(Directory.Exists(folderToDelete));
        }

        // Test case for Delete method when folder doesn't exist
        [Fact]
        public void Delete_FolderNotExists_ReturnsFalse()
        {
            // Arrange
            string nonExistentFolder = "nonexistent_folder";

            // Act
            bool result = EditSave.Delete(nonExistentFolder);

            // Assert
            Assert.False(result);
        }
    }
}

