using System;

namespace SpeechToTextConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"caminho para o arquivo WAV";
            if (args.Length > 0)
                path = args[0];

            if (!string.IsNullOrEmpty(path))
            {
                var recognition = new Recognition(path);
                recognition.OnFinish += Recognition_OnFinish;
                recognition.Execute();
                Console.ReadLine();
            }
        }

        private static void Recognition_OnFinish(object sender, EventArgs e)
            => Environment.Exit(0);
    }
}
