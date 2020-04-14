using System.Collections.Generic;

namespace OrleansExample.Grains.Interfaces
{
    public class GrainMessage<T>
    {
        private Dictionary<string, string> headers = new Dictionary<string, string>();

        public GrainMessage()
        {

        }

        public GrainMessage(T content)
        {
            Content = content;
        }



        public T Content
        {
            get;
            set;
        }

        public Dictionary<string, string> Headers
        {
            get { return headers; }
        }
    }
}
