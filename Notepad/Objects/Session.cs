using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public static string Filename { get; } = Path.Combine(_applicationPath, FILENAME);

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
        public static async Task<Session> Load()
        {
            var session = new Session();

            if (File.Exists(Filename))
            {
                var serializer = new XmlSerializer(typeof(Session));
                var streamReader = new StreamReader(Filename);

                try
                {
                    session = (Session)serializer.Deserialize(streamReader);

                    foreach (var file in session.TextFiles)
                    {
                        var fileName = file.FileName;
                        var backupFileName = file.BackUpFileName;
                        file.SafeFileName = Path.GetFileName(fileName);


                        //fichier existant sur le disque
                        if (File.Exists(fileName))
                        {
                            using (StreamReader reader = new StreamReader(fileName))
                            {
                                file.Content = await reader.ReadToEndAsync();
                            }
                        }

                        //fichier backup du dossier backup
                        if (File.Exists(backupFileName))
                        {
                            using (StreamReader reader = new StreamReader(backupFileName))
                            {
                                file.Content = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Une erreur s'est produite" + ex.Message);
                    
                }
                streamReader.Close();
            }
            return session;
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
