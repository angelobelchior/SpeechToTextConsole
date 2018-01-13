using Microsoft.CognitiveServices.SpeechRecognition;
using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Collections.Generic;

namespace SpeechToTextConsole
{
    public class Recognition : IDisposable
    {
        private static readonly string SPEECH_TO_TEXT_KEY;
        private static readonly string DEFAULT_LOCALE;
        private static readonly string OUTPUT_FOLDER;

        private readonly DataRecognitionClient _client;
        private readonly string _file;
        private readonly List<string> _phrases = new List<string>();

        public event EventHandler<EventArgs> OnFinish;

        static Recognition()
        {
            SPEECH_TO_TEXT_KEY = ConfigurationManager.AppSettings["SpeechToTextKey"];
            if (string.IsNullOrEmpty(SPEECH_TO_TEXT_KEY))
                throw new ArgumentNullException("SpeechToTextKey");

            DEFAULT_LOCALE = ConfigurationManager.AppSettings["DefaultLocale"];
            if (string.IsNullOrEmpty(SPEECH_TO_TEXT_KEY))
                throw new ArgumentNullException("DefaultLocale");

            OUTPUT_FOLDER = ConfigurationManager.AppSettings["OutputFolder"];
            if (string.IsNullOrEmpty(SPEECH_TO_TEXT_KEY))
                throw new ArgumentNullException("OutputFolder");
        }

        public Recognition(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException(nameof(file));

            this._file = file;

            this._client = SpeechRecognitionServiceFactory.CreateDataClient(
                SpeechRecognitionMode.LongDictation,
                DEFAULT_LOCALE,
                SPEECH_TO_TEXT_KEY
            );
            this._client.OnResponseReceived += OnResponseReceived;
            this._client.OnConversationError += OnConversationError;
        }

        public void Dispose()
        {
            try
            {
                this._client.Dispose();
            }
            catch { }
        }

        public void Execute()
        {
            using (var fileStream = new FileStream(this._file, FileMode.Open, FileAccess.Read))
            {
                var bytesRead = 0;
                var buffer = new byte[1024];

                try
                {
                    do
                    {
                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                        this._client.SendAudio(buffer, bytesRead);
                    }
                    while (bytesRead > 0);
                }
                finally
                {
                    this._client.EndAudio();
                }
            }
        }

        private void OnConversationError(object sender, SpeechErrorEventArgs e)
            => this.Log($"Error -> {e.SpeechErrorCode} - {e.SpeechErrorText}");

        private void OnResponseReceived(object sender, SpeechResponseEventArgs e)
        {
            this.Log(e.PhraseResponse.RecognitionStatus);
            if (e.PhraseResponse.RecognitionStatus == RecognitionStatus.RecognitionSuccess)
            {
                void add(RecognizedPhrase phrase)
                {
                    this._phrases.Add(phrase.LexicalForm);
                    this.Log(phrase.LexicalForm);
                };

                e.PhraseResponse
                 .Results
                 .ToList()
                 ?.ForEach(add);
            }

            if (e.PhraseResponse.RecognitionStatus == RecognitionStatus.EndOfDictation ||
                e.PhraseResponse.RecognitionStatus == RecognitionStatus.DictationEndSilenceTimeout)
            {
                this.WriteFile();
                this.OnFinish?.Invoke(this, new EventArgs());
            }
        }

        private void WriteFile()
        {
            try
            {
                var fileName = $"{Guid.NewGuid().ToString()}.txt";
                var path = Path.Combine(OUTPUT_FOLDER, fileName);
                if (this._phrases.Count > 0)
                {
                    var content = string.Join("\n", this._phrases);
                    File.AppendAllText(path, content);
                }
            }
            catch { }
        }

        private void Log(object message)
            => Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:dd")} - {message}");
    }
}
