using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Origin.IO {

    public enum IOTypeEnum {
        Binary,
        XML,
        Text,
    }

    public class IOHelper<T> where T : new() {

        private XmlSerializer xmlSerializer;
        private BinaryFormatter binarySerializer;

        private IOTypeEnum SerlizationType { get; set; }

        /// <summary>
        /// If you use with IOTypeEnum.Text the T is must be List<string>.
        /// </summary>
        /// <param name="a_SerlizationType"></param>
        public IOHelper(IOTypeEnum a_SerlizationType) {
            SerlizationType = a_SerlizationType;
            CreateSerializer();
        }

        public void Save(string a_fullPath,T a_object) {
            Stream stream = File.OpenWrite(a_fullPath);

            switch (SerlizationType) {
                case IOTypeEnum.Binary:
                    binarySerializer.Serialize(stream, a_object);
                    break;
                case IOTypeEnum.XML:
                    xmlSerializer.Serialize(stream, a_object);
                    break;
                case IOTypeEnum.Text:
                    stream.Close();
                    File.WriteAllLines(a_fullPath, ((List<string>)(object)a_object).ToArray(), Encoding.UTF8);
                    break;
            }
            stream.Close();
        }

        public void Save(string a_fileName,FilesType a_type,T a_object) {

            string fallPath = string.Empty;

            switch (a_type) {
                case FilesType.Configuration:
                    fallPath = Path.Combine(Consts.ConfigurationPath, a_fileName);
                    break;
                case FilesType.Data:
                    fallPath = Path.Combine(Consts.DatPath, a_fileName);
                    break;
            }

            Save(fallPath, a_object);
        }

        public T Load(string a_fullPath) {

            Stream stream = File.OpenRead(a_fullPath);

            switch (SerlizationType) {
                case IOTypeEnum.Binary:
                    return (T)binarySerializer.Deserialize(stream);
                case IOTypeEnum.XML:
                    return (T)xmlSerializer.Deserialize(stream);
                case IOTypeEnum.Text:
                    return (T)(object)(((string[])File.ReadAllLines(a_fullPath, Encoding.UTF8)).ToList<string>());
                default :
                    throw new Exception("");
            }

            stream.Close();
        }

        /// <summary>
        /// If file dontExsist, this method creating the file with defalut object and return it.
        /// </summary>
        /// <param name="a_fileName"></param>
        /// <param name="a_type"></param>
        /// <returns></returns>
        public T Load(string a_fileName, FilesType a_type) {

            string fallPath = string.Empty;

            switch (a_type) {
                case FilesType.Configuration:
                    fallPath = Path.Combine( Consts.ConfigurationPath, a_fileName);
                    break;
                case FilesType.Data:
                    fallPath = Path.Combine(Consts.DatPath, a_fileName);
                    break;
            }

            if (!File.Exists(fallPath)) {
                T newIns = (T)Activator.CreateInstance(typeof(T));
                Save(fallPath, newIns);
            }

           return Load(fallPath);
        }

        private void CreateSerializer() {

            switch (SerlizationType) {
                case IOTypeEnum.Binary:
                    binarySerializer = new BinaryFormatter();
                    break;

                case IOTypeEnum.XML:
                    xmlSerializer = new XmlSerializer(typeof(T));
                    break;
                case IOTypeEnum.Text:

                    if (!(typeof(T).IsGenericType &&
                               typeof(T).GetGenericArguments()[0] == typeof(string))){
                        throw new Exception("If you use IOTypeEnum.Text, The T is must be List<string>");
                    }

                    break;

                default:
                    throw new Exception("can't create Serializer , Unknown type :" + SerlizationType.ToString());
            }

        }
    }
}
