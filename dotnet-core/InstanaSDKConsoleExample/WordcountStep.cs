
namespace InstanaSDKConsoleExample
{
    using Instana.Tracing.Sdk.Spans;
    using System.Collections.Generic;

    internal class WordcountStep : IStep
    {
        public string Execute(string input, Dictionary<string, object> protocol)
        {
            using (var span = CustomSpan.CreateEntry(this, CorrelationUtil.GetCorrelationFromCurrentContext))
            {
                span.WrapAction(() =>
                {
                    span.SetTag(new string[] { "Input" }, input);
                    string[] words = input.Split(new char[] { ',', ' ', '!', '.', '?' });
                    int icount = 0;
                    foreach (string word in words)
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            icount++;
                        }
                    }
                    protocol["Word-Count"] = icount;
                }, true);
            }
            return input;
        }
    }
}
