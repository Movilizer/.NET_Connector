using MWS.Movilizer;
using System.Collections.Generic;

namespace MWS.Templates
{
    public class MoveletSetTemplate
    {
        public MovilizerMoveletSet _moveletSet;
        List<MoveletTemplate> _movelets;

        public MoveletSetTemplate()
        {
            _moveletSet = new MovilizerMoveletSet();

            // init movelet array
            _movelets = new List<MoveletTemplate>();
        }

        public MoveletTemplate GetMovelet(string mKey)
        {
            foreach (MoveletTemplate mTemplate in _movelets)
            {
                if (mKey.Equals(mTemplate._movelet.moveletKey))
                {
                    return mTemplate;
                }
            }
            return null;
        }

        public void AddMovelet(MoveletTemplate m)
        {
            _movelets.Add(m);
        }

        public MovilizerMoveletSet ToMoveletSet()
        {
            int mCount = 0;
            _moveletSet.movelet = new MovilizerMovelet[_movelets.Count];

            foreach (MoveletTemplate mTemplate in _movelets)
            {
                _moveletSet.movelet[mCount++] = mTemplate.ToMovelet();
            }
            return _moveletSet;
        }

        public void SerializeToFile(string path)
        {
            foreach (MoveletTemplate mTemplate in _movelets)
            {
                mTemplate.SerializeToFile(path);
            }
        }
    }
}
