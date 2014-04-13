using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Origin.IO {
    public static class FolderAndFilesCreator {

        public static bool IfFolderAndFilesExsists() { 
            bool ConfigurationDirEx = Directory.Exists(Path.Combine(Consts.exeLocation,Consts.ConfigurationDirName));
            bool DataDirEx = Directory.Exists(Path.Combine(Consts.exeLocation, Consts.DatDirName));
            
            return ConfigurationDirEx && DataDirEx;
        }

        public static void CreateFoldersAndFiles(){

            Directory.CreateDirectory(Path.Combine(Consts.exeLocation, Consts.ConfigurationDirName));
            Directory.CreateDirectory(Path.Combine(Consts.exeLocation, Consts.DatDirName));

        }
    }
}
