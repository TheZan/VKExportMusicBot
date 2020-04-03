using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using VkNet.AudioBypassService.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Bot
{
    class VK
    {
        VkApi api = new VkApi();
        public void Auth(string login, string password)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            api = new VkApi(services);
            api.Authorize(new ApiAuthParams
            {
                Login = login,
                Password = password
            });
        }

        public VkCollection<Audio> GetAudioFromProfile(int userId)
        {
            VkCollection<Audio> audio = api.Audio.Get(new AudioGetParams
            {
                OwnerId = userId
            });
            return audio;
        }

        public VkCollection<Audio> GetAudioSearch(string searchText)
        {
            VkCollection<Audio> audio = api.Audio.Search(new AudioSearchParams
            {
                Query = searchText,
                Count = 5,
                Autocomplete = true
            });
            return audio;
        }
    }
}
