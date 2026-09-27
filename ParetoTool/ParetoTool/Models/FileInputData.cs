using ParetoTool.Interfaces;
using System.IO;

namespace ParetoTool.Models
{
    /// <summary>
    /// Represents a file-based input data source that can be associated with a Project. This class implements the IInputData interface and
    /// </summary>
    public class FileInputData : IInputData
    {

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        public File File { get; set; } = new File();


        /// <summary>
        /// Gets the type name of the input data source, which is "FileInputData" for this class.   
        /// </summary>
        public string Typename { get => nameof(FileInputData); }    

        /// <summary>
        /// Returns a string representation of the FileInputData object, including its Id and FilePath.
        /// </summary>
        /// <returns>A string representation of the FileInputData object.</returns>
        public override string ToString()
        {
            return $"FileInputData: Id={Id}, FilePath={File}";
        }

    }


    /// <summary>
    /// Represents a file with properties for Id, FilePath, and Delimiter. This class is used to encapsulate the details of a 
    /// file-based input data source within the ParetoTool application.
    /// </summary>
    public class File
    {

        /// <summary>
        /// Gets or sets the unique identifier for the file input data. This property is initialized with a new GUID when a File instance is created.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the file path of the input data source.
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets the file name extracted from the FilePath property. If the FilePath is null or empty, it returns an empty string.
        /// </summary>
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    return string.Empty;
                }
                else
                {
                    return Path.GetFileName(FilePath);
                }
            }
        }   

        /// <summary>
        /// Gets or sets the delimiter used in the file for separating values. This property is used to specify how the data in the file is structured.
        /// </summary>
        public string Delimiter { get; set; } = string.Empty;
        public override string ToString()
        {
            
            if (string.IsNullOrEmpty(FilePath))
            {
                return $"File: Id={Id}, FilePath=Not Set, Delimiter={Delimiter}";
            }
            else
            {
                return $"File: Id={Id}, FilePath={FilePath}, Delimiter={Delimiter}";
            }   

        }




    }

}
