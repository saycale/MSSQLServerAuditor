using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace MSSQLServerAuditor.Model.Settings
{
    /// <summary>
    /// Stuct image.
    /// </summary>
    public struct NamedImage
    {
        /// <summary>
        /// Object of the picture.
        /// </summary>
        public readonly Image Picture;

        /// <summary>
        /// Name of the picture.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Initializing object Image.
        /// </summary>
        /// <param name="picture">Object of the picture.</param>
        /// <param name="name">Name of the picture.</param>
        public NamedImage(Image picture, string name)
        {
            Picture = picture;
            Name = name;
        }
    }

    /// <summary>
    /// Resources application.
    /// </summary>
    [Obfuscation(Exclude = true)]
    public static class ApplicationResources
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Database node image reference name
        /// </summary>
        public static string DatabaseNodeImageRefName = "databaseSubNode";
        /// <summary>
        /// Connection node image reference name
        /// </summary>
        public static string ConnectionNodeImageRefName = "connection";
        /// <summary>
        /// Raw data node image reference name
        /// </summary>
        public static string RawDataNodeImageRefName = "savedReport";

        private static readonly List<string> ImgExtensions = new List<string> { ".png", ".bmp" };

        /// <summary>
        /// Method for obtaining the list of images.
        /// </summary>
        /// <returns>List of images.</returns>
        public static IEnumerable<NamedImage> GetImages()
        {
            var result = new List<NamedImage>();

            foreach (var file in new DirectoryInfo(Path.Combine(Application.StartupPath, "Images")).GetFiles())
            {
                if (!ImgExtensions.Contains(Path.GetExtension(file.Name)))
                {
                    continue;
                }

                try
                {
                    result.Add( new NamedImage( Image.FromFile(file.FullName), Path.GetFileNameWithoutExtension(file.Name) ));
                }
                catch (IOException ex)
                {
                    log.Error(ex);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return result;
        }
    }
}
