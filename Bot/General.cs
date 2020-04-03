using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Bot
{
    class General
    {
        VK vk = new VK();
        public General(string login, string password)
        {
            Console.WriteLine("Инициализация...");
            Thread.Sleep(1000);
            vk.Auth(login, password);
            Console.WriteLine("Бот запущен и готов к работе...");
        }

        public List<AudioFile> StartExportFromProfile(int userId)
        {
            List<AudioFile> audioFile = new List<AudioFile>();
            try
            {
                var audio = vk.GetAudioFromProfile(userId);
                foreach (var item in audio)
                {
                    if (item.Url != null)
                    {
                        audioFile.Add(new AudioFile { performer = item.Artist, title = item.Title, url = DecodeAudioUrl(item.Url) });
                    }
                }
                return audioFile;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return audioFile;
        }

        public List<AudioFile> StartSearch(string searchText)
        {
            List<AudioFile> audioFile = new List<AudioFile>();
            try
            {
                var audio = vk.GetAudioSearch(searchText);
                foreach (var item in audio)
                {
                    if (item.Url != null)
                    {
                        audioFile.Add(new AudioFile { performer = item.Artist, title = item.Title, url = DecodeAudioUrl(item.Url) });
                    }
                }
                return audioFile;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return audioFile;
        }

        public static Uri DecodeAudioUrl(Uri audioUrl)
        {

            var segments = audioUrl.Segments.ToList();

            segments.RemoveAt((segments.Count - 1) / 2);
            segments.RemoveAt(segments.Count - 1);

            segments[segments.Count - 1] = segments[segments.Count - 1].Replace("/", ".mp3");

            return new Uri($"{audioUrl.Scheme}://{audioUrl.Host}{string.Join("", segments)}{audioUrl.Query}");
        }
    }
    class AudioFile
    {
        public string performer { get; set; }
        public string title { get; set; }
        public Uri url { get; set; }
    }
}
