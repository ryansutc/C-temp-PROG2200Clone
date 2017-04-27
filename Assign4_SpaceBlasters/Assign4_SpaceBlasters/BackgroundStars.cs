using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Assign4_SpaceBlasters
{
    /// <summary>
    /// A List of Background Stars allowing basic enumerable methods
    /// </summary>
    public class BackgroundStars: IEnumerator, IEnumerable
    {
        private BackgroundStar[] starList;
        int position = -1;

        public object Current
        {
            get { return starList[position]; }
        }

        public bool MoveNext()
        {
            position++;
            return (position < starList.Length);
        }

        public void Reset()
        {
            position = 0;
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        public void Remove(BackgroundStar star)
        {
            //remove a star after it has left the screen?

        }

    }
}
