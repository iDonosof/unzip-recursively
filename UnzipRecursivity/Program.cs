using System.IO.Compression;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System;

const string ZipLocationFolder = "C:\\Users\\IgnacioDonoso\\Downloads\\Cleveland";
const string PDFFolderRepository = "C:\\Users\\IgnacioDonoso\\Downloads\\Cleveland\\PDF's";
const string ZipNameFormat = "ok.cleveland.arrest.";

void ProcessZip(string rootFolder)
{
    foreach(string file in Directory.GetFiles(rootFolder))
    {
        Console.WriteLine($"Processing file: {file}");
        if (Regex.IsMatch(file, @"\.zip$"))
        {
            string extractedZip = Unzip(file);
            ProcessZip(extractedZip);
            Directory.Delete(extractedZip, true);
        }
        else
        {
            string fileName = Path.GetFileName(file);
            string newFilePath = $"{PDFFolderRepository}\\{fileName}";
            try
            {
                File.Move(file, newFilePath);
            }
            catch
            {
                Console.WriteLine($"File {fileName} already exists");
            }
        }
    }
}

void ProcessPDF(string rootFolder)
{
    foreach (string file in Directory.GetFiles(rootFolder))
    {
        Console.WriteLine($"Zipping file: {Path.GetFileName(file)}");
        Zip(file);
    }
}

string Unzip(string zipPath)
{
    string guid = Guid.NewGuid().ToString();
    string ZipExtracted = $"{ZipLocationFolder}\\{guid}";
    if(!Directory.Exists(ZipExtracted))
        Directory.CreateDirectory(ZipExtracted);
    ZipFile.ExtractToDirectory(zipPath, ZipExtracted);
    return ZipExtracted;
}

string Zip(string filePath)
{
    string pdfName = Path.GetFileName(filePath);
    string dateFromName = Regex.Match(pdfName, @"\d{3,}").Value;
    string newPath = $"{PDFFolderRepository}\\{ZipNameFormat}{dateFromName}";
    if (!Directory.Exists(newPath))
        Directory.CreateDirectory(newPath);
    File.Copy(filePath, $"{newPath}\\{pdfName}");
    ZipFile.CreateFromDirectory(newPath, newPath + ".zip");
    Directory.Delete(newPath, true);
    return newPath;
}

ProcessPDF(PDFFolderRepository);
Console.WriteLine("Process Finished");
Console.ReadLine();