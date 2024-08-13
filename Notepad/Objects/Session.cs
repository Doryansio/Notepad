﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Notepad.Objects
{
    public class Session
    {
        private const string FILENAME = "session.xml";

        private static string _applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string _applicationPath = Path.Combine(_applicationDataPath, "Notepad.NET");



        private readonly XmlWriterSettings _writerSettings;

        public static string BackupPath = Path.Combine(_applicationPath, "Notepad.NET", "backup");

        /// <summary>
        ///  chemin d'acces et nom du fichier representant la session.
        /// </summary>
        public string Filename { get; } = Path.Combine(_applicationPath, FILENAME);

        [XmlAttribute(AttributeName = "ActiveIndex")]
        public int ActiveIndex { get; set; } = 0;
        [XmlElement(ElementName = "File")]
        public List<TextFile> TextFiles { get; set; }

        public Session()
        {
            TextFiles = new List<TextFile>();
            _writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = ("\t"),
                OmitXmlDeclaration = true
            };

            if (!Directory.Exists(_applicationPath))
            {
                Directory.CreateDirectory(_applicationPath);
            }
        }

        public void Save()
        {
            var emptyNameSpace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(Session));
            using (XmlWriter writter = XmlWriter.Create(Filename, _writerSettings))
            {
                serializer.Serialize(writter, this, emptyNameSpace);
            }
        }

        public async void BackupFile(TextFile file)
        {
            if (!Directory.Exists(BackupPath))
            {
                await Task.Run(() => Directory.CreateDirectory(BackupPath));
            }

            if(file.FileName.StartsWith("Sans Titre"))
            {
                using (StreamWriter writer = File.CreateText(file.BackUpFileName))
                {
                    await writer.WriteAsync(file.Content);
                }
            }
        }
    }
}
