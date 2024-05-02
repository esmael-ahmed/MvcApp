using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public static class DocumentSettings
    {
        // Upload
        public static string UploadFile(IFormFile file, string FolderName)
        {
            // 1- Get Located Folder Path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

            // 2- Get File Name & Make It Unique
            string FileName = $"{Guid.NewGuid()} {file.FileName}";

            // 3- Get File Path
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4- Save File As Stream
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);

            // 5- Return File Name
            return FileName;
        }

        // Delete
        public static void DeleteFile(string fileName, string folderName)
        {
            // 1- Get File Path
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);

            // 2- Check If File Exists Or Not
            if (File.Exists(filePath)) 
            {
                // if Exists Delete It
                File.Delete(filePath); 
            }
        }
    }
}
