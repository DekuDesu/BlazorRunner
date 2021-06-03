using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public enum LoadAssemblyResult
    {
        /// <summary>
        /// No attempt at loading an assembly was made yet
        /// </summary>
        none,
        /// <summary>
        /// No errors were encountered
        /// </summary>
        Success,
        // Loading Errors
        /// <summary>
        /// The bytes read from the file were null
        /// </summary>
        NoBytesRead,
        /// <summary>
        /// The bytes from the file were not in the expected COFF image format(a .DLL), C++ executable files might throw a BadImageFormatException. 
        /// This is most likely caused by the C++ compiler stripping the relocation addresses or the .reloc section from your executable file. To preserve the .reloc address for your C++ executable file, specify /fixed:no when you are linking.
        /// </summary>
        BadImageFormat,
        /// <summary>
        /// The path provided is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.
        /// </summary>
        PathWasInvalid,
        /// <summary>
        /// The path provided was null
        /// </summary>
        PathWasNull,
        /// <summary>
        /// The path provided was too long
        /// </summary>
        PathWasTooLong,
        /// <summary>
        /// The directory the file was located in was not found
        /// </summary>
        DirectoryNotFound,
        /// <summary>
        /// A general <see cref="System.IO.IOException"/> was encounted, make sure the file isn't open some where else.
        /// </summary>
        FailedToGetFileHandle,
        /// <summary>
        /// The file might be in an area where BlazorRunner cannot read it or something else went wrong.
        /// </summary>
        UnauthorizedAccess,
    }
}
