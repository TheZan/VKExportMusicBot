# VKExportMusicBot

VKExportMusicBot - это простейший телеграмм бот для экспорта музыки из vk.

# Описание
Бот разработан на языке C# .NET Core. Данный бот позволяет экспортировать аудиофайлы по ссылке на страницу vk или через поиск по ключевому слову.
![N|Solid](https://i.imgur.com/EmdnPFN.png)
### Инструкция
Для запуска бота требуется создать файл auth.txt в каталоге с программой.
Заполнить его в таком формате с разделителем ';':
```sh
loginVk;passwordVk;tokenTelegram
```
### Credits
* [vknet](https://github.com/vknet/vk) - api for vk.com
* [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot) - .NET Client for Telegram Bot API
