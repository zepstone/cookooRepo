using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Origin.IO {

    public enum FilesType {
        Configuration,
        Data,
    }

    static class Consts {

        public static readonly string ConfigurationDirName = "Configuration";
        public static readonly string DatDirName = "Data";

        public readonly static string exeLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

        public static readonly string ConfigurationPath = Path.Combine(exeLocation, ConfigurationDirName);
        public static readonly string DatPath = Path.Combine(exeLocation, DatDirName);

    }
}
