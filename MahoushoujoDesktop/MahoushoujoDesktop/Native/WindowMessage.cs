using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;

namespace MahoushoujoDesktop . Native
{
    static class WindowMessage
    {
        public const int DisplayChange = 0x7e;
        public const int User = 0x400;

        public const int Custom_ShowWindow = User + 1;
    }
}
