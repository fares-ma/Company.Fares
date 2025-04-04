namespace Company.Fares.PL.Helpers
{
    public class DocumentSettings
    {
        // Upload any file

        public static string UploadFile(IFormFile file, string folderName) 
        {
            // 1 Get Folder Location

            //string folderPath = ""+folderName; 
            //var folderPath=Directory.GetCurrentDirectory() +"\\wwwroot\\files\\" +folderName;
            //D:\Route\C#\Assignments\MVC\Company.Route\Company.Route.PL\Files\
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            // 2 Get FileName and Make it Unique

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            // File Path
            var filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
        }

        // delete any file

        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }
    }
}
