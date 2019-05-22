

namespace UnityWeld.Binding.Adapters
{
    [Adapter(typeof(float), typeof(int))]
    public class SingleToIntAdapter : IAdapter
    {
        public object Convert(object valueIn, AdapterOptions options)
        {
            return (int)valueIn;
        }
    }
}