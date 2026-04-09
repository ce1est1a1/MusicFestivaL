using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace MusicFestivalDeserializer
{
    public static class DeserializerService
    {
        public static FestivalData LoadJson(string path)
        {
            string jsonText = File.ReadAllText(path);

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.ReadCommentHandling = JsonCommentHandling.Skip;
            options.AllowTrailingCommas = true;

            FestivalData data = JsonSerializer.Deserialize<FestivalData>(jsonText, options);
            if (data == null)
            {
                throw new InvalidDataException("Не удалось десериализовать JSON-файл.");
            }

            NormalizeData(data);
            return data;
        }

        public static FestivalData LoadXml(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FestivalData));
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            try
            {
                object result = serializer.Deserialize(stream);
                FestivalData data = result as FestivalData;

                if (data == null)
                {
                    throw new InvalidDataException("Не удалось десериализовать XML-файл.");
                }

                NormalizeData(data);
                return data;
            }
            finally
            {
                stream.Dispose();
            }
        }

        private static void NormalizeData(FestivalData data)
        {
            if (data.Festival == null)
            {
                data.Festival = new Festival();
            }

            if (data.Artists == null)
            {
                data.Artists = new System.Collections.Generic.List<Artist>();
            }

            if (data.Stages == null)
            {
                data.Stages = new System.Collections.Generic.List<StageInfo>();
            }

            int artistIndex;
            for (artistIndex = 0; artistIndex < data.Artists.Count; artistIndex = artistIndex + 1)
            {
                Artist artist = data.Artists[artistIndex];
                if (artist.Performance == null)
                {
                    artist.Performance = new PerformanceInfo();
                }

                if (artist.ArtistStats == null)
                {
                    artist.ArtistStats = new ArtistStats();
                }
            }

            int stageIndex;
            for (stageIndex = 0; stageIndex < data.Stages.Count; stageIndex = stageIndex + 1)
            {
                StageInfo stage = data.Stages[stageIndex];
                if (stage.WorkTime == null)
                {
                    stage.WorkTime = new StageWorkTime();
                }

                if (stage.Manager == null)
                {
                    stage.Manager = new StageManager();
                }

                if (stage.Decor == null)
                {
                    stage.Decor = new StageDecor();
                }
            }
        }
    }
}
