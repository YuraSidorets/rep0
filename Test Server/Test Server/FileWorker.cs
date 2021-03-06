﻿using System;
using System.Collections.Generic;
using System.IO;

namespace DB_Test
{
    /// <summary>
    /// Gets information about file
    /// </summary>
    static class FileWorker
    {
        public static Dictionary<string, object> GetFileInfo(string path)
        {
            Dictionary<string, object> fileInformation = new Dictionary<string, object>();
            string fileName = Path.GetFileNameWithoutExtension(path);
            fileInformation.Add("Name", fileName);

            string fileType = Path.GetExtension(path);
            fileInformation.Add("Type", fileType);

            DateTime fileCreationDate = File.GetCreationTime(path);
            fileInformation.Add("Date", fileCreationDate);

            FileInfo fi = new FileInfo(path);
            double fileSize = fi.Length;
            fileInformation.Add("Size", fileSize.ToString());

            return fileInformation;
        }

        public static byte[] GetBytes(string path)
        {
            byte[] output = null;
            try
            {
                output = File.ReadAllBytes(path);
            }
            catch (IOException e)
            {
               
            }
            return output;
        }

        /// <summary>
        /// Gets file from db and creates it in pathOfCreatedFile
        /// </summary>
        /// <param name="id">Id of the file</param>
        /// <param name="pathOfCreatedFile">Directory of file to create</param>
        public static void GetFileFromDB(int id, string pathOfCreatedFile)
        {
            object[] arr = DBWorker.GetFileToWrite(id);
            string name = arr[0].ToString();
            string type = arr[1].ToString();
            byte[] bytes = arr[2] as byte[];
            string path = string.Format("{0}{1}{2}", pathOfCreatedFile, name, type);
            FileStream fs = File.Create(path);
            BinaryWriter bwr = new BinaryWriter(fs);
            bwr.Write(bytes);
            bwr.Close();
        }
    }
}
