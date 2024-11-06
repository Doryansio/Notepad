using System;
using System.IO;
using System.Xml.Serialization;

namespace Notepad.Objects
{
    public class TextFile
    {


        /// <summary>
        /// Chemin d'acces et nom du fichier.
        /// </summary>
        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; set; }



        /// <summary>
        /// Chemin d'acces et nom du fichier backup
        /// </summary>
        [XmlAttribute(AttributeName = "BackUpFileName")]
        public string BackUpFileName { get; set; } = string.Empty;

        
        /// <summary>
        /// Nom et extension du fichier. Le nom du fichier n'inclut pas le chemin d'acces.
        /// </summary>
        [XmlIgnore()]
        public string SafeFileName { get; set; }

        
        /// <summary>
        /// Nom et extension du fichier backup. Le nom du fichier n'inclut pas le chemin d'acces.
        /// </summary>
        [XmlIgnore()]
        public string SafeBackUpFileName { get; set; }



        /// <summary>
        /// Contenu du fichier.
        /// </summary>
        [XmlIgnore()]
        public string Content { get; set; } = string.Empty;

        


        /// <summary>
        /// Constructeur de la classe TextFile
        /// </summary>
        public TextFile()
        {

        }



        /// <summary>
        /// Constructeur de la classe TextFile
        /// </summary>
        /// <param name="fileName">Chemin d'accès et nom du fichier.</param>
        public TextFile(string fileName)
        {
            FileName = fileName;
            SafeFileName = Path.GetFileName(fileName);

            if (FileName.StartsWith("Sans Titre"))
            {
                SafeBackUpFileName = $"{FileName}@{DateTime.Now: dd-MM-yyyy-HH-mm-ss}";
                BackUpFileName = Path.Combine(Session.BackupPath, SafeBackUpFileName);
            }
        }
    }
}
