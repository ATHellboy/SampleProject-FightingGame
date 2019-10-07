using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Infrastructure.T4
{
    public partial class TagsGeneratorInterface : TagsGenerator
    {
        [MenuItem("Tools/Generate Tags")]
        public static void GenerateTags()
        {
            string outputPath = EditorUtility.SaveFilePanelInProject(title: "Save Location", defaultName: "Tags",
                extension: "cs", message: "Where do you want to save this script?");

            TagsGenerator generator = new TagsGenerator { Session = new ConcurrentDictionary<string, object>() };

            string className = Path.GetFileName(outputPath).Replace(".cs", "");

            generator.Session["m_ClassName"] = className;

            List<string> tags = new List<string>(InternalEditorUtility.tags);

            for (int i = tags.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(tags[i]))
                {
                    tags.RemoveAt(i);
                    continue;
                }
            }

            generator.Session["m_UnityTags"] = tags.ToArray();

            generator.Initialize();

            string classDef = generator.TransformText();

            File.WriteAllText(outputPath, classDef);

            AssetDatabase.Refresh();
        }
    }
}