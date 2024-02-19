namespace EasySave.Tests
{
    public class UnitTestExample
    {
        [Fact]
        public void saveFile_WhenFileHasBeenSaved_ReturnsTrue_Example()
        {
            // Simulate a file being saved
            bool fileSaved = true;

            // Assert that the file has been saved
            Assert.True(fileSaved);
        }

        [Fact]
        public void saveFile_WhenFileHasNotBeenSaved_ReturnsFalse_Example()
        {
            // Simulate a file not being saved
            bool fileSaved = false;

            // Assert that the file has not been saved
            Assert.False(fileSaved);
        }

        [Fact]
        public void check_FileName_Example()
        {
            // Simulate a scenario where the file name is "Hi"
            string expectedFileName = "Hi";

            // Retrieve the actual file name
            string actualFileName = "Hi";

            // Assert that the actual file name matches the expected file name
            Assert.Equal(expectedFileName, actualFileName);
        }
    }
}
